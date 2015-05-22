using Client.ViewModel;
using Client.ViewModel.SettingsViewModel;
using Shared;

namespace Client.View.UI
{
    /// <summary>
    /// Interaction logic for TaskBacklogView.xaml
    /// </summary>
    public partial class TaskBacklogView
    {
        private readonly IServiceRegistry serviceRegistry;

        /// <summary>
        /// Create a Task backlog view.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="bandId">The band's backlog to view.</param>
        public TaskBacklogView(IServiceRegistry serviceRegistry, int bandId)
        {
            this.serviceRegistry = serviceRegistry;

            var viewModel = new TaskBacklogViewModel(serviceRegistry, bandId);
            viewModel.OpenTaskDetailsViewRequested += OnOpenTaskBacklogViewRequested;

            DataContext = viewModel;

            InitializeComponent();
        }

        private void OnOpenTaskBacklogViewRequested(object sender, WindowRequestedEventArgs windowRequestedEventArgs)
        {
            var taskDetailsView = new TaskDetailsView(serviceRegistry, windowRequestedEventArgs.Task);

            taskDetailsView.ShowDialog();
        }
    }
}