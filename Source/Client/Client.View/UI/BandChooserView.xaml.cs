using System.ComponentModel;
using Client.ViewModel;
using Client.ViewModel.SettingsViewModel;
using Shared;
using Shared.Domain;

namespace Client.View.UI
{
    /// <summary>
    /// Interaction logic for BandChooserView.xaml
    /// </summary>
    public partial class BandChooserView
    {
        private readonly ApplicationExitHelper applicationExitHelper = new();
        private readonly IServiceRegistry serviceRegistry;

        /// <summary>
        /// Create a new view for the user to choose a <see cref="Band" /> to enter.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        public BandChooserView(IServiceRegistry serviceRegistry)
        {
            this.serviceRegistry = serviceRegistry;

            var viewModel = new BandChooserViewModel(serviceRegistry);
            viewModel.OpenMainViewRequested += OnOpenMainViewRequested;
            DataContext = viewModel;

            InitializeComponent();
        }

        private void OnOpenMainViewRequested(object sender, WindowRequestedEventArgs windowRequestedEventArgs)
        {
            applicationExitHelper.CanExitApplication = false;

            RemoveEventSubscribers();

            var mainView = new MainView(serviceRegistry, windowRequestedEventArgs.Band);
            Close();
            mainView.Show();
        }

        private void RemoveEventSubscribers()
        {
            var viewModel = DataContext as BandMakerViewModel;

            if (viewModel != null)
            {
                viewModel.OpenMainViewRequested -= OnOpenMainViewRequested;
            }
        }

        /// <summary>
        /// Try to close the application if set.
        /// </summary>
        /// <param name="e">The cancel event args.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            applicationExitHelper.TryExitApplication();
        }
    }
}