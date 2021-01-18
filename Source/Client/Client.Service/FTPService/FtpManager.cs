using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using log4net;
using Shared.Configuration;
using Utility;

namespace Client.Service.FTPService
{
    /// <summary>
    /// Access and modify files on an FTP.
    /// </summary>
    public class FtpManager : IFtpManager
    {
        private const int BufferSize = 2048;
        private static readonly ILog Log = LogManager.GetLogger(typeof(FtpManager));
        private readonly AppConfigManager configManager;

        public FtpManager(AppConfigManager configManager)
        {
            this.configManager = configManager;
        }

        public string Address
        {
            get => configManager.FindStoredValue("FTPHost");
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    configManager.UpdateSetting("FTPHost", value);
                }
            }
        }

        public string Username
        {
            get => configManager.FindStoredValue("FTPUsername");
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    configManager.UpdateSetting("FTPUsername", value);
                }
            }
        }

        public string Password
        {
            get => configManager.FindStoredValue("FTPPassword");
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    configManager.UpdateSetting("FTPPassword", value);
                }
            }
        }

        /// <summary>
        /// Updates for a data upload.
        /// </summary>
        public event EventHandler<DataSentEventArgs> DownloadedDataUpdate;

        /// <summary>
        /// Updates for a data download.
        /// </summary>
        public event EventHandler<DataSentEventArgs> UploadedDataUpdate;

        /// <summary>
        /// Upload a file to the server to store.
        /// </summary>
        /// <param name="fileToUpload">The file to upload's location on the client's computer.</param>
        /// <param name="fileName">The name of the file that will be uploaded.</param>
        /// <returns>The result of the upload.</returns>
        public Task<FtpStatusCode> UploadFileAsync(string fileToUpload, string fileName)
        {
            return Task.Factory.StartNew(() => UploadFile(fileToUpload, fileName));
        }

        /// <summary>
        /// Download a file from the server on a non blocking thread.
        /// </summary>
        /// <param name="fileToDownload">The name of the file to download from the server.</param>
        /// <param name="downloadLocation">The location to store the download on the PC.</param>
        /// <returns>The result of the download.</returns>
        public Task<FtpStatusCode> DownloadFileAsync(string fileToDownload, string downloadLocation)
        {
            return Task.Factory.StartNew(() => DownloadFile(fileToDownload, downloadLocation));
        }

        /// <summary>
        /// The current files on the FTP Server.
        /// </summary>
        /// <param name="directory">The root to find files.</param>
        /// <returns></returns>
        public string[] FilesOnServer(string directory)
        {
            var directoryList = new string[10];

            try
            {
                FtpWebRequest ftpRequest = CreateRequest(directory, WebRequestMethods.Ftp.ListDirectory);

                using (var ftpResponse = (FtpWebResponse) ftpRequest.GetResponse())
                {
                    Stream responseStream = ftpResponse.GetResponseStream();

                    if (responseStream != null)
                    {
                        using (var ftpReader = new StreamReader(responseStream))
                        {
                            string directoryRaw = null;

                            try
                            {
                                while (ftpReader.Peek() != -1)
                                {
                                    directoryRaw += ftpReader.ReadLine() + "|";
                                }

                                if (directoryRaw != null)
                                {
                                    directoryList = directoryRaw.Split("|".ToCharArray());
                                }

                                return directoryList;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                return new[] { "" };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return directoryList;
        }

        /// <summary>
        /// Rename a file on the FTP Server.
        /// </summary>
        /// <param name="name">The file to name.</param>
        /// <param name="newName">The new file name.</param>
        public FtpStatusCode Rename(string name, string newName)
        {
            try
            {
                FtpWebRequest ftpRequest = CreateRequest(name, WebRequestMethods.Ftp.Rename);

                ftpRequest.RenameTo = newName;

                using (var response = (FtpWebResponse) ftpRequest.GetResponse())
                {
                    return response.StatusCode;
                }
            }
            catch (WebException webException)
            {
                return HandleException(webException);
            }
        }

        /// <summary>
        /// Delete a file from the FTP Server.
        /// </summary>
        /// <param name="filename">The file to delete.</param>
        public FtpStatusCode Delete(string filename)
        {
            try
            {
                FtpWebRequest ftpRequest = CreateRequest(filename, WebRequestMethods.Ftp.DeleteFile);

                using (var ftpResponse = (FtpWebResponse) ftpRequest.GetResponse())
                {
                    return ftpResponse.StatusCode;
                }
            }
            catch (WebException webException)
            {
                return HandleException(webException);
            }
        }

        /// <summary>
        /// Create a directory on the FTP server.
        /// </summary>
        /// <param name="name">The name of the directory.</param>
        public FtpStatusCode CreateDirectory(string name)
        {
            try
            {
                FtpWebRequest ftpRequest = CreateRequest(name, WebRequestMethods.Ftp.MakeDirectory);

                using (var response = (FtpWebResponse) ftpRequest.GetResponse())
                {
                    return response.StatusCode;
                }
            }
            catch (WebException webException)
            {
                return HandleException(webException);
            }
        }

        /// <summary>
        /// Download a file from the FTP and store it in a location.
        /// </summary>
        /// <param name="remoteFile">The FTP file to get.</param>
        /// <param name="localFile">The location to store the downloaded file.</param>
        private FtpStatusCode DownloadFile(string remoteFile, string localFile)
        {
            try
            {
                FtpWebRequest filesizeRequest = CreateRequest(remoteFile, WebRequestMethods.Ftp.GetFileSize);

                long fileSize;

                using (WebResponse resp = filesizeRequest.GetResponse())
                {
                    fileSize = resp.ContentLength;
                }

                return DownloadWithUpdates(CreateRequest(remoteFile, WebRequestMethods.Ftp.DownloadFile), localFile, fileSize);
            }
            catch (WebException webException)
            {
                return TryDownloadWithoutUpdates(remoteFile, localFile, webException);
            }
        }

        private FtpStatusCode TryDownloadWithoutUpdates(string remoteFile, string localFile, WebException webException)
        {
            try
            {
                return DownloadWithoutUpdates(CreateRequest(remoteFile, WebRequestMethods.Ftp.DownloadFile), localFile);
            }
            catch (WebException)
            {
                return HandleException(webException);
            }
        }

        private FtpStatusCode DownloadWithUpdates(WebRequest downloadRequest, string localFile, long fileSize)
        {
            try
            {
                using (var responseFileDownload = (FtpWebResponse) downloadRequest.GetResponse())
                using (Stream responseStream = responseFileDownload.GetResponseStream())
                using (var writeStream = new FileStream(localFile, FileMode.Create))
                {
                    var buffer = new byte[BufferSize];

                    if (responseStream == null)
                    {
                        return FtpStatusCode.FileActionAborted;
                    }

                    int bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                    var bytes = 0;

                    while (bytesRead > 0)
                    {
                        writeStream.Write(buffer, 0, bytesRead);
                        bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                        bytes += bytesRead;

                        int bytesDownloaded = bytes;

                        EventUtility.SafeFireEvent(DownloadedDataUpdate, this, new DataSentEventArgs(bytesDownloaded, fileSize));
                    }

                    return FtpStatusCode.CommandOK;
                }
            }
            catch (WebException exception)
            {
                return HandleException(exception);
            }
        }

        private static FtpStatusCode DownloadWithoutUpdates(WebRequest uploadRequest, string localFile)
        {
            using (var response = (FtpWebResponse) uploadRequest.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (FileStream fileStream = File.Create(localFile))
            {
                responseStream?.CopyTo(fileStream);

                return response.StatusCode;
            }
        }

        private FtpStatusCode UploadFile(string localFile, string remoteFile)
        {
            try
            {
                FtpWebRequest ftpRequest = CreateRequest(remoteFile, WebRequestMethods.Ftp.UploadFile);

                using (FileStream inputStream = File.OpenRead(localFile))
                using (Stream outputStream = ftpRequest.GetRequestStream())
                {
                    var buffer = new byte[BufferSize];
                    var totalReadBytesCount = 0;

                    int readBytesCount;

                    while ((readBytesCount = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        outputStream.Write(buffer, 0, readBytesCount);
                        totalReadBytesCount += readBytesCount;

                        EventUtility.SafeFireEvent(UploadedDataUpdate, this, new DataSentEventArgs(totalReadBytesCount, inputStream.Length));
                    }
                }

                return FtpStatusCode.CommandOK;
            }
            catch (WebException exception)
            {
                return HandleException(exception);
            }
        }

        private static FtpStatusCode HandleException(WebException exception)
        {
            Log.Error(exception);

            if (exception.Status == WebExceptionStatus.ProtocolError)
            {
                var response = exception.Response as FtpWebResponse;

                return response?.StatusCode ?? FtpStatusCode.Undefined;
            }

            return FtpStatusCode.Undefined;
        }

        private FtpWebRequest CreateRequest(string file, string requestMethod)
        {
            var ftpRequest = (FtpWebRequest) WebRequest.Create(Address + "/" + file);
            ftpRequest.Credentials = new NetworkCredential(Username, Password);

            SetupRequestParameters(requestMethod, ftpRequest);

            return ftpRequest;
        }

        private static void SetupRequestParameters(string requestMethod, FtpWebRequest ftpRequest)
        {
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            ftpRequest.Method = requestMethod;
        }
    }
}