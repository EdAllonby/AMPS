using System;
using System.Net;
using System.Windows.Input;
using Client.Model.SettingsModel;
using Client.Service;
using Client.Service.FTPService;
using Client.ViewModel.Commands;
using Shared;
using Shared.Domain;
using Shared.Repository;
using Utility;

namespace Client.ViewModel.SettingsViewModel
{
    /// <summary>
    /// View model for Uploading a task.
    /// </summary>
    public sealed class UploadTaskViewModel : ViewModel
    {
        private readonly IClientService clientService;
        private UploadTaskModel uploadTaskModel;

        /// <summary>
        /// Initialise the view model with the client's <see cref="IServiceRegistry" />.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="taskToUpload">The <see cref="System.Threading.Tasks.Task" /> to upload.</param>
        public UploadTaskViewModel(IServiceRegistry serviceRegistry, Task taskToUpload) : base(serviceRegistry)
        {
            UploadTaskModel = new UploadTaskModel(taskToUpload);
            clientService = ServiceRegistry.GetService<IClientService>();
        }

        /// <summary>
        /// The underlying model.
        /// </summary>
        public UploadTaskModel UploadTaskModel
        {
            get { return uploadTaskModel; }
            set
            {
                if (Equals(value, uploadTaskModel)) return;
                uploadTaskModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Upload a <see cref="Task" /> to the server.
        /// </summary>
        public ICommand UploadTask
        {
            get { return new RelayCommand(UploadTaskToServer, CanUploadTask); }
        }

        /// <summary>
        /// Complete a task without uploading a file.
        /// </summary>
        public ICommand CompleteTaskWithoutUpload
        {
            get { return new RelayCommand(CompleteTaskAndClose, CanCompleteTaskAndClose); }
        }

        /// <summary>
        /// Browse a <see cref="Task" /> file to upload.
        /// </summary>
        public ICommand BrowseTaskToUpload
        {
            get { return new RelayCommand(OpenUploadFileDialog, CanOpenFileDialog); }
        }

        private bool CanCompleteTaskAndClose()
        {
            return !UploadTaskModel.IsUploading;
        }

        private void CompleteTaskAndClose()
        {
            UpdateTaskStatusToComplete();

            EventUtility.SafeFireEvent(CloseUploaderViewRequested, this);
        }

        private void OnUploadDataUpdate(object sender, DataSentEventArgs e)
        {
            UploadTaskModel.UploadSize = e.BytesSent;
            UploadTaskModel.TotalSize = e.TotalBytes;
        }

        /// <summary>
        /// Fires when a show FTP error is requested.
        /// </summary>
        public event EventHandler<FtpStatusEventArgs> ShowUploadStatus;

        /// <summary>
        /// Fires when close this view is requested.
        /// </summary>
        public event EventHandler CloseUploaderViewRequested;

        /// <summary>
        /// Fires when requesting to open file dialog.
        /// </summary>
        public event EventHandler OpenFileDialogRequested;

        private async void UploadTaskToServer()
        {
            IFtpManager ftpManager = ServiceRegistry.GetService<IFtpManager>();

            ftpManager.UploadedDataUpdate += OnUploadDataUpdate;

            UploadTaskModel.IsUploading = true;

            FtpStatusCode result = await ftpManager.UploadFileAsync(UploadTaskModel.FileToUploadLocation, UploadTaskModel.TaskFileName);

            if (result == FtpStatusCode.CommandOK)
            {
                UpdateTaskStatusToComplete();
            }

            UploadTaskModel.IsUploading = false;

            ftpManager.UploadedDataUpdate -= OnUploadDataUpdate;

            EventUtility.SafeFireEvent(ShowUploadStatus, this, new FtpStatusEventArgs(result));

            EventUtility.SafeFireEvent(CloseUploaderViewRequested, this);
        }

        private void UpdateTaskStatusToComplete()
        {
            IReadOnlyEntityRepository<Task> taskRepository = ServiceRegistry.GetService<RepositoryManager>().GetRepository<Task>();

            Task updatedTask = taskRepository.FindEntityById(UploadTaskModel.TaskId);

            updatedTask.IsCompleted = true;

            clientService.UpdateTask(updatedTask);
        }

        private bool CanUploadTask()
        {
            return !UploadTaskModel.IsUploading && !string.IsNullOrWhiteSpace(UploadTaskModel.FileToUploadLocation);
        }

        private bool CanOpenFileDialog()
        {
            return !UploadTaskModel.IsUploading;
        }

        private void OpenUploadFileDialog()
        {
            EventUtility.SafeFireEvent(OpenFileDialogRequested, this);
        }
    }
}