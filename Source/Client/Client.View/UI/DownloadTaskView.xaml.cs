using System;
using System.Net;
using System.Windows;
using Client.ViewModel.SettingsViewModel;
using Microsoft.Win32;
using Shared;
using Shared.Domain;

namespace Client.View.UI
{
    /// <summary>
    /// Interaction logic for DownloadTaskView.xaml
    /// </summary>
    public partial class DownloadTaskView
    {
        /// <summary>
        /// Create a new view for downloading a <see cref="Task" />.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="task">The <see cref="Task" /> to download.</param>
        public DownloadTaskView(IServiceRegistry serviceRegistry, Task task)
        {
            var viewModel = new DownloadTaskViewModel(serviceRegistry, task);

            viewModel.SaveFileDialogRequested += OnSaveFileDialogRequested;
            viewModel.ShowDownloadStatus += OnShowDownloadStatus;
            viewModel.CloseDownloaderViewRequested += OnCloseDownloaderViewRequested;


            DataContext = viewModel;

            InitializeComponent();
        }

        private void OnCloseDownloaderViewRequested(object sender, EventArgs e)
        {
            Close();
        }

        private static void OnShowDownloadStatus(object sender, FtpStatusEventArgs e)
        {
            if (e.StatusCode == FtpStatusCode.CommandOK || e.StatusCode == FtpStatusCode.ClosingData)
            {
                MessageBox.Show(string.Format("File successfully downloaded from server!"), string.Format("Response status code: {0}", ((int) e.StatusCode)), MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(string.Format("Could not download file because {0} error", e.StatusCode), ((int) e.StatusCode).ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnSaveFileDialogRequested(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Zip Files|*.zip";

            saveFileDialog.DefaultExt = ".zip";

            saveFileDialog.AddExtension = true;

            bool? showDialog = saveFileDialog.ShowDialog();

            if (showDialog != null && showDialog.Value)
            {
                DownloadTaskViewModel viewModel = (DownloadTaskViewModel) DataContext;

                viewModel.DownloadTaskModel.DownloadLocation = saveFileDialog.FileName;
            }
        }
    }
}