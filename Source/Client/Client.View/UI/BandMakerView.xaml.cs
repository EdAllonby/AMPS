using System.ComponentModel;
using Client.ViewModel;
using Client.ViewModel.SettingsViewModel;
using Shared;

namespace Client.View.UI
{
    /// <summary>
    /// Interaction logic for BandMakerView.xaml
    /// </summary>
    public partial class BandMakerView
    {
        private readonly ApplicationExitHelper applicationExitHelper = new();
        private readonly IServiceRegistry serviceRegistry;

        /// <summary>
        /// Create a new Band maker view to create or enter a Band.
        /// </summary>
        /// <param name="serviceRegistry"></param>
        public BandMakerView(IServiceRegistry serviceRegistry)
        {
            this.serviceRegistry = serviceRegistry;

            var viewModel = new BandMakerViewModel(serviceRegistry);
            viewModel.OpenMainViewRequested += OnOpenMainViewRequested;
            DataContext = viewModel;

            InitializeComponent();
        }

        private void RemoveEventSubscribers()
        {
            var viewModel = DataContext as BandMakerViewModel;

            if (viewModel != null)
            {
                viewModel.OpenMainViewRequested -= OnOpenMainViewRequested;
            }
        }

        private void OnOpenMainViewRequested(object sender, WindowRequestedEventArgs windowRequestedEventArgs)
        {
            applicationExitHelper.CanExitApplication = false;

            RemoveEventSubscribers();

            var mainView = new MainView(serviceRegistry, windowRequestedEventArgs.Band);
            Close();
            mainView.Show();
        }

        /// <summary>
        /// Attempt to close the application if instructed to.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            applicationExitHelper.TryExitApplication();
        }
    }
}