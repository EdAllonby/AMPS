using Client.ViewModel.SettingsViewModel;
using Shared;
using Shared.Domain;

namespace Client.View.UI
{
    /// <summary>
    /// Interaction logic for TaskDetailsView.xaml
    /// </summary>
    public partial class TaskDetailsView
    {
        /// <summary>
        /// Creates a new view to display <see cref="Task" /> details.
        /// </summary>
        /// <param name="sserviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="task">The Task to display details for.</param>
        public TaskDetailsView(IServiceRegistry sserviceRegistry, Task task)
        {
            var viewModel = new TaskDetailsViewModel(sserviceRegistry, task);

            DataContext = viewModel;

            InitializeComponent();
        }
    }
}