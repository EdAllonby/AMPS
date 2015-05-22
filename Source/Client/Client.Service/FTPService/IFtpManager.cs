using System;
using System.Net;
using System.Threading.Tasks;
using Shared;

namespace Client.Service.FTPService
{
    /// <summary>
    /// Access and modify files on an FTP.
    /// </summary>
    public interface IFtpManager : IService
    {
        /// <summary>
        /// Updates for a data upload.
        /// </summary>
        event EventHandler<DataSentEventArgs> UploadedDataUpdate;

        /// <summary>
        /// Updates for a data download.
        /// </summary>
        event EventHandler<DataSentEventArgs> DownloadedDataUpdate;

        /// <summary>
        /// Download a file from the server on a non blocking thread.
        /// </summary>
        /// <param name="fileToDownload">The name of the file to download from the server.</param>
        /// <param name="downloadLocation">The location to store the download on the PC.</param>
        /// <returns>The result of the download.</returns>
        Task<FtpStatusCode> DownloadFileAsync(string fileToDownload, string downloadLocation);

        /// <summary>
        /// Upload a file to the server to store.
        /// </summary>
        /// <param name="fileToUpload">The file to upload's location on the client's computer.</param>
        /// <param name="fileName">The name of the file that will be uploaded.</param>
        /// <returns>The result of the upload.</returns>
        Task<FtpStatusCode> UploadFileAsync(string fileToUpload, string fileName);

        /// <summary>
        /// The current files on the FTP Server.
        /// </summary>
        /// <param name="directory">The root to find files.</param>
        /// <returns></returns>
        string[] FilesOnServer(string directory);

        /// <summary>
        /// Rename a file on the FTP Server.
        /// </summary>
        /// <param name="name">The file to name.</param>
        /// <param name="newName">The new file name.</param>
        FtpStatusCode Rename(string name, string newName);

        /// <summary>
        /// Delete a file from the FTP Server.
        /// </summary>
        /// <param name="filename">The file to delete.</param>
        FtpStatusCode Delete(string filename);

        /// <summary>
        /// Create a directory on the FTP server.
        /// </summary>
        /// <param name="name">The name of the directory.</param>
        FtpStatusCode CreateDirectory(string name);
    }
}