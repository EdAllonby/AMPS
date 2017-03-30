using System;
using System.IO;
using System.Net;
using System.Windows;
using Client.ViewModel.SettingsViewModel;
using Microsoft.Win32;
using Shared;
using Shared.Domain;

namespace Client.View.UI
{
    /// <summary>
    /// Interaction logic for UploadTaskView.xaml
    /// </summary>
    public partial class UploadTaskView
    {
        /// <summary>
        /// Create a new Task maker view to create new tasks.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="taskToUpload">The <see cref="Task" /> to upload.</param>
        public UploadTaskView(IServiceRegistry serviceRegistry, Task taskToUpload)
        {
            var viewModel = new UploadTaskViewModel(serviceRegistry, taskToUpload);

            viewModel.OpenFileDialogRequested += OnOpenFileDialogRequested;
            viewModel.ShowUploadStatus += OnShowUploadStatus;
            viewModel.CloseUploaderViewRequested += OnCloseUploaderViewRequested;

            DataContext = viewModel;

            InitializeComponent();
        }

        private void OnCloseUploaderViewRequested(object sender, EventArgs e)
        {
            Close();
        }

        private static void OnShowUploadStatus(object sender, FtpStatusEventArgs e)
        {
            if (e.StatusCode == FtpStatusCode.CommandOK)
            {
                MessageBox.Show("File successfully uploaded to server!", $"Response status code: {(int) e.StatusCode}", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Could not upload file because {e.StatusCode} error", ((int) e.StatusCode).ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnOpenFileDialogRequested(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog { Filter = "Any file|*.*" };

            openFileDialog.Filter = "Zip Files|*.zip";

            bool? showDialog = openFileDialog.ShowDialog();

            if (showDialog != null && showDialog.Value)
            {
                var viewModel = (UploadTaskViewModel) DataContext;

                viewModel.UploadTaskModel.FileExtension = Path.GetExtension(openFileDialog.FileName);

                viewModel.UploadTaskModel.FileToUploadLocation = openFileDialog.FileName;
            }
        }
    }
}