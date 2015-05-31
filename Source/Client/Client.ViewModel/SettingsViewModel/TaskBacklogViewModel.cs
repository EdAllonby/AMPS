using System.Collections.Generic;
using Client.Model.SettingsModel;
using Shared;
using Shared.Domain;
using Shared.Repository;

namespace Client.ViewModel.SettingsViewModel
{
    /// <summary>
    /// Displays the current tasks in the <see cref="Band" />'s backlog.
    /// </summary>
    public sealed class TaskBacklogViewModel : TaskInformationViewModel
    {
        private TaskBacklogModel taskBacklogModel;

        /// <summary>
        /// Creates a new Backlog View Model.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="bandId">The <see cref="Band" />'s Id to display Backlog <see cref="Task" />s.</param>
        public TaskBacklogViewModel(IServiceRegistry serviceRegistry, int bandId) : base(serviceRegistry)
        {
            var userRepository = serviceRegistry.GetService<RepositoryManager>().GetRepository<User>();

            List<TaskModel> taskModels = new List<TaskModel>();

            var taskRepository = (TaskRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Task>();

            foreach (Task backlogTask in taskRepository.GetTasksInBandBacklog(bandId))
            {
                User assignedUser = userRepository.FindEntityById(backlogTask.AssignedUserId);
                TaskModel taskModel = new TaskModel(backlogTask, assignedUser);

                taskModels.Add(taskModel);
            }

            taskBacklogModel = new TaskBacklogModel(taskModels);
        }

        /// <summary>
        /// The model for this View Model.
        /// </summary>
        public TaskBacklogModel TaskBacklogModel
        {
            get { return taskBacklogModel; }
            set
            {
                if (Equals(value, taskBacklogModel)) return;
                taskBacklogModel = value;
                OnPropertyChanged();
            }
        }
    }
}