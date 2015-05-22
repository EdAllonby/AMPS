using System;
using System.Windows;
using System.Windows.Input;
using Client.ViewModel.LoginViewModel;
using Shared;

namespace Client.View.UI
{
    /// <summary>
    /// Interaction logic for LoginView.
    /// </summary>
    public partial class LoginView
    {
        private readonly IServiceRegistry serviceRegistry;

        /// <summary>
        /// Creates a new instance of a login screen for the user to login to the server.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        public LoginView(IServiceRegistry serviceRegistry)
        {
            this.serviceRegistry = serviceRegistry;

            var viewModel = new LoginViewModel(serviceRegistry);
            viewModel.OpenBandChooserViewRequested += OnOpenBandChooserViewRequested;
            viewModel.OpenBandMakerViewRequested += OnOpenBandMakerViewRequested;
            viewModel.LoginErrored += OnLoginError;
            DataContext = viewModel;

            InitializeComponent();
        }

        private static void OnLoginError(object sender, LoginErrorEventArgs e)
        {
            MessageBox.Show(e.ErrorDescription, e.Result.ToString());
        }

        private void OnOpenBandMakerViewRequested(object sender, EventArgs e)
        {
            var bandMakerView = new BandMakerView(serviceRegistry);
            RemoveEventSubscribers();
            Close();
            bandMakerView.Show();
        }

        private void OnOpenBandChooserViewRequested(object sender, EventArgs e)
        {
            var mainView = new BandChooserView(serviceRegistry);
            RemoveEventSubscribers();
            Close();
            mainView.Show();
        }

        private void RemoveEventSubscribers()
        {
            var viewModel = DataContext as LoginViewModel;

            if (viewModel != null)
            {
                viewModel.OpenBandChooserViewRequested -= OnOpenBandChooserViewRequested;
                viewModel.OpenBandMakerViewRequested -= OnOpenBandMakerViewRequested;
            }
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void PasswordBoxPreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
    }
}