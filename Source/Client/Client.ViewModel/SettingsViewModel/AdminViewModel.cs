using System.Collections.Generic;
using System.Linq;
using Client.Model.SettingsModel;
using Shared;
using Shared.Domain;
using Shared.Repository;

namespace Client.ViewModel.SettingsViewModel
{
    /// <summary>
    /// A view model for the admin view.
    /// </summary>
    public sealed class AdminViewModel : TaskInformationViewModel
    {
        private AdminModel adminModel;

        /// <summary>
        /// Initialise the view model with the client's <see cref="IServiceRegistry" />.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="band">The band that this admin panel should show.</param>
        public AdminViewModel(IServiceRegistry serviceRegistry, Band band) : base(serviceRegistry)
        {
            var userRepository = serviceRegistry.GetService<RepositoryManager>().GetRepository<User>();

            List<TaskModel> taskModels = new List<TaskModel>();

            var taskRepository = (TaskRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Task>();

            var jamRepository = (JamRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Jam>();

            Jam currentJam = jamRepository.GetCurrentActiveJamInBand(band.Id);

            if (taskRepository.GetTasksInJam(currentJam.Id) != null)
            {
                foreach (Task completedJamTasks in taskRepository.GetTasksInJam(currentJam.Id).Where(task => task.IsCompleted))
                {
                    User assignedUser = userRepository.FindEntityById(completedJamTasks.AssignedUserId);

                    TaskModel taskModel = new TaskModel(completedJamTasks, assignedUser);

                    taskModels.Add(taskModel);
                }
            }

            adminModel = new AdminModel(taskModels);
        }

        /// <summary>
        /// The underlying model.
        /// </summary>
        public AdminModel AdminModel
        {
            get { return adminModel; }
            set
            {
                if (Equals(value, adminModel)) return;
                adminModel = value;
                OnPropertyChanged();
            }
        }
    }
}