using System.Windows;
using Client.ViewModel;
using Client.ViewModel.MainViewModel;
using Shared;

namespace Client.View.UI
{
    /// <summary>
    /// Interaction logic for TaskItemView.xaml
    /// </summary>
    public partial class TaskItemView
    {
        private IServiceRegistry serviceRegistry;

        /// <summary>
        /// Creates a new Task Item User Control view.
        /// </summary>
        public TaskItemView()
        {
            InitializeComponent();

            DataContextChanged += TaskItemView_DataContextChanged;
        }

        private void TaskItemView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                // TODO: Reliant on this event for a view model.

                TaskItemViewModel viewModel = (TaskItemViewModel) DataContext;
                serviceRegistry = viewModel.ClientServiceRegistry;
                viewModel.OpenTaskDetailsViewRequested += OnOpenTaskBacklogViewRequested;
                viewModel.OpenUploadTaskViewRequested += viewModel_OpenUploadTaskViewRequested;
            }
        }

        private void viewModel_OpenUploadTaskViewRequested(object sender, WindowRequestedEventArgs windowRequestedEventArgs)
        {
            var uploadTaskView = new UploadTaskView(serviceRegistry, windowRequestedEventArgs.Task);
            uploadTaskView.ShowDialog();
        }

        private void OnOpenTaskBacklogViewRequested(object sender, WindowRequestedEventArgs windowRequestedEventArgs)
        {
            var taskDetailsView = new TaskDetailsView(serviceRegistry, windowRequestedEventArgs.Task);

            taskDetailsView.ShowDialog();
        }
    }
}