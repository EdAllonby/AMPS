using Client.Model.SettingsModel;
using Shared;
using Shared.Domain;
using Shared.Repository;

namespace Client.ViewModel.SettingsViewModel
{
    /// <summary>
    /// Displays the task details.
    /// </summary>
    public sealed class TaskDetailsViewModel : ViewModel
    {
        private TaskModel taskModel;

        /// <summary>
        /// Creates a new <see cref="Task" /> Details View Model.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry"/>.</param>
        /// <param name="task">The task to get the details for.</param>
        public TaskDetailsViewModel(IServiceRegistry serviceRegistry, Task task) : base(serviceRegistry)
        {
            IReadOnlyEntityRepository<User> userRepository = serviceRegistry.GetService<RepositoryManager>().GetRepository<User>();

            User assignedUser = userRepository.FindEntityById(task.AssignedUserId);

            TaskModel = new TaskModel(task, assignedUser);
        }

        /// <summary>
        /// The underlying model.
        /// </summary>
        public TaskModel TaskModel
        {
            get { return taskModel; }
            set
            {
                if (Equals(value, taskModel)) return;
                taskModel = value;
                OnPropertyChanged();
            }
        }
    }
}