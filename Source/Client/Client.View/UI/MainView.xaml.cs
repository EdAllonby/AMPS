using System;
using System.Windows;
using Client.Service;
using Client.View.UI.ToastNotification;
using Client.ViewModel;
using Client.ViewModel.MainViewModel;
using Shared;
using Shared.Domain;

namespace Client.View.UI
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView
    {
        private const double TopOffset = 20;
        private const double LeftOffset = 380;
        private readonly ApplicationExitHelper applicationExitHelper = new ApplicationExitHelper();
        private readonly Band band;
        private readonly Toasts growlNotifications = new Toasts();
        private readonly IServiceRegistry serviceRegistry;
        private readonly ToastNotificationManager toastNotifier;

        /// <summary>
        /// Creates a new instance of a Main View for a <see cref="Band" />.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="band">The <see cref="Band" /> context.</param>
        public MainView(IServiceRegistry serviceRegistry, Band band)
        {
            this.band = band;

            InitializeComponent();

            this.serviceRegistry = serviceRegistry;

            CreateUserListDock();
            CreateJamTasksDock();

            var viewModel = new MainViewModel(serviceRegistry, band);

            viewModel.OpenBandDetailsViewRequested += OnOpenBandDetailsViewRequested;
            viewModel.OpenCreateTaskViewRequested += OnOpenCreateTaskViewRequested;
            viewModel.OpenTaskBacklogViewRequested += OnOpenTaskBacklogViewRequested;
            viewModel.OpenJamMakerViewRequested += OnOpenJamMakerViewRequested;
            viewModel.OpenAboutBoxViewRequested += OnOpenAboutBoxRequested;
            viewModel.OnJamEnded += OnOpenJamEndedDialog;
            viewModel.CloseMainAndOpenLoginViewRequested += OnCloseMainAndOpenLoginViewRequested;

            DataContext = viewModel;

            toastNotifier = serviceRegistry.GetService<ToastNotificationManager>();
            toastNotifier.ToastNotificationRequested += NotificationReceived;
            growlNotifications.Top = SystemParameters.WorkArea.Top + TopOffset;
            growlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - LeftOffset;
        }

        private void NotificationReceived(object sender, TostNotificationEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                growlNotifications.AddNotification(new Notification(e.Message, "pack://application:,,,/Resources/notification-icon.png", "Mesage"));

            });
        }

        private void OnCloseMainAndOpenLoginViewRequested(object sender, EventArgs e)
        {
            applicationExitHelper.CanExitApplication = false;

            LoginView loginWindow = new LoginView(serviceRegistry);

            toastNotifier.ToastNotificationRequested -= NotificationReceived;
            growlNotifications.Close();
            Close();

            loginWindow.Show();
        }

        private static void OnOpenJamEndedDialog(object sender, JamEndedEventArgs e)
        {
            MessageBox.Show("Current jam has ended", string.Format("Jam with Id {0} has ended", e.FinishedJam.Id));
        }

        private void OnOpenBandDetailsViewRequested(object sender, WindowRequestedEventArgs windowRequestedEventArgs)
        {
            BandDetailsView bandDetailsView = new BandDetailsView(serviceRegistry, windowRequestedEventArgs.Band);
            bandDetailsView.ShowDialog();
        }

        private void OnOpenCreateTaskViewRequested(object sender, WindowRequestedEventArgs windowRequestedEventArgs)
        {
            TaskMakerView taskMakerView = new TaskMakerView(serviceRegistry, windowRequestedEventArgs.Band.Id);
            taskMakerView.ShowDialog();
        }

        private void OnOpenTaskBacklogViewRequested(object sender, WindowRequestedEventArgs windowRequestedEventArgs)
        {
            TaskBacklogView taskBacklogView = new TaskBacklogView(serviceRegistry, windowRequestedEventArgs.Band.Id);
            taskBacklogView.ShowDialog();
        }

        private void OnOpenJamMakerViewRequested(object sender, WindowRequestedEventArgs windowRequestedEventArgs)
        {
            JamMakerView jamMakerView = new JamMakerView(serviceRegistry, windowRequestedEventArgs.Band);
            jamMakerView.ShowDialog();
        }

        private static void OnOpenAboutBoxRequested(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void CreateUserListDock()
        {
            //TODO: Find a workaround for this. Currently creating a UserControl that has a constructor with a parameter can't be done in XAML, so do it here.
            UserListDock.Children.Add(new UserListView(serviceRegistry, band.Id));
        }

        private void CreateJamTasksDock()
        {
            JamTasksDock.Children.Add(new JamTasksView(serviceRegistry, band));
        }

        private void OnMainWindowClosed(object sender, EventArgs e)
        {
            applicationExitHelper.TryExitApplication();
        }
    }
}