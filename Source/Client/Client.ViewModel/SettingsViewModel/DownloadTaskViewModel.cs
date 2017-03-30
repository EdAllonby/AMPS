using System;
using System.Net;
using System.Windows.Input;
using Client.Model.SettingsModel;
using Client.Service.FTPService;
using Client.ViewModel.Commands;
using Shared;
using Shared.Domain;
using Utility;

namespace Client.ViewModel.SettingsViewModel
{
    public sealed class DownloadTaskViewModel : ViewModel
    {
        private DownloadTaskModel downloadTaskModel;

        public DownloadTaskViewModel(IServiceRegistry serviceRegistry, Task taskToDownload) : base(serviceRegistry)
        {
            DownloadTaskModel = new DownloadTaskModel(taskToDownload);
        }

        /// <summary>
        /// The underlying model.
        /// </summary>
        public DownloadTaskModel DownloadTaskModel
        {
            get { return downloadTaskModel; }
            set
            {
                if (Equals(value, downloadTaskModel)) return;
                downloadTaskModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Browse a <see cref="Task" /> file to upload.
        /// </summary>
        public ICommand BrowseDownloadLocation => new RelayCommand(OpenSaveFileDialog, CanOpenSaveFileDialog);

        /// <summary>
        /// Download a <see cref="Task" /> from the server.
        /// </summary>
        public ICommand DownloadTask => new RelayCommand(DownloadTaskFromServer, CanDownloadTask);

        /// <summary>
        /// Fires when a show FTP error is requested.
        /// </summary>
        public event EventHandler<FtpStatusEventArgs> ShowDownloadStatus;

        /// <summary>
        /// Fires when close this view is requested.
        /// </summary>
        public event EventHandler CloseDownloaderViewRequested;

        /// <summary>
        /// Fires when requesting to open save file dialog.
        /// </summary>
        public event EventHandler SaveFileDialogRequested;

        private void OpenSaveFileDialog()
        {
            EventUtility.SafeFireEvent(SaveFileDialogRequested, this);
        }

        private bool CanOpenSaveFileDialog()
        {
            return !DownloadTaskModel.IsDownloading;
        }

        private bool CanDownloadTask()
        {
            return !DownloadTaskModel.IsDownloading && !string.IsNullOrWhiteSpace(DownloadTaskModel.DownloadLocation);
        }

        private async void DownloadTaskFromServer()
        {
            var ftpManager = ServiceRegistry.GetService<IFtpManager>();

            ftpManager.DownloadedDataUpdate += OnDownloadedDataUpdate;

            DownloadTaskModel.IsDownloading = true;

            FtpStatusCode result = await ftpManager.DownloadFileAsync(DownloadTaskModel.TaskFileName, DownloadTaskModel.DownloadLocation);

            DownloadTaskModel.IsDownloading = false;

            ftpManager.DownloadedDataUpdate -= OnDownloadedDataUpdate;

            EventUtility.SafeFireEvent(ShowDownloadStatus, this, new FtpStatusEventArgs(result));

            EventUtility.SafeFireEvent(CloseDownloaderViewRequested, this);
        }

        private void OnDownloadedDataUpdate(object sender, DataSentEventArgs e)
        {
            DownloadTaskModel.DownloadedSize = e.BytesSent;
            DownloadTaskModel.TotalSize = e.TotalBytes;
        }
    }
}