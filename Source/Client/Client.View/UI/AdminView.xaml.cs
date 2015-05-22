using Client.ViewModel;
using Client.ViewModel.SettingsViewModel;
using Shared;
using Shared.Domain;

namespace Client.View.UI
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView
    {
        private readonly IServiceRegistry serviceRegistry;

        /// <summary>
        /// Create an Admin view.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="band">The band's admin panel to view.</param>
        public AdminView(IServiceRegistry serviceRegistry, Band band)
        {
            this.serviceRegistry = serviceRegistry;
            var viewModel = new AdminViewModel(serviceRegistry, band);

            viewModel.OpenTaskDetailsViewRequested += OnOpenTaskDetailsViewRequested;
            viewModel.OpenDownloadTaskViewRequested += OnOpenDownloadTaskViewRequested;

            DataContext = viewModel;

            InitializeComponent();
        }

        private void OnOpenDownloadTaskViewRequested(object sender, WindowRequestedEventArgs windowRequestedEventArgs)
        {
            var downloadTaskView = new DownloadTaskView(serviceRegistry, windowRequestedEventArgs.Task);
            downloadTaskView.ShowDialog();
        }

        private void OnOpenTaskDetailsViewRequested(object sender, WindowRequestedEventArgs windowRequestedEventArgs)
        {
            var taskDetailsView = new TaskDetailsView(serviceRegistry, windowRequestedEventArgs.Task);

            taskDetailsView.ShowDialog();
        }
    }
}