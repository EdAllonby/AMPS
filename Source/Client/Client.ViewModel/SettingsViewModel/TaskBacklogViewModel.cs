using System.Collections.Generic;
using Client.Model.SettingsModel;
using Shared;
using Shared.Domain;

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
        /// <param name="band">The <see cref="Band" /> to display Backlog <see cref="Task" />s.</param>
        public TaskBacklogViewModel(IServiceRegistry serviceRegistry, Band band) : base(serviceRegistry)
        {
            var taskModels = new List<TaskModel>();

            foreach (Task backlogTask in band.Backlog)
            {
                var taskModel = new TaskModel(backlogTask);

                taskModels.Add(taskModel);
            }

            taskBacklogModel = new TaskBacklogModel(taskModels);
        }

        /// <summary>
        /// The model for this View Model.
        /// </summary>
        public TaskBacklogModel TaskBacklogModel
        {
            get => taskBacklogModel;
            set
            {
                if (Equals(value, taskBacklogModel)) return;
                taskBacklogModel = value;
                OnPropertyChanged();
            }
        }
    }
}