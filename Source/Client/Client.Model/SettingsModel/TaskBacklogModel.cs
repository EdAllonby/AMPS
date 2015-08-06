using System.Collections.Generic;
using Shared.Domain;

namespace Client.Model.SettingsModel
{
    /// <summary>
    /// Models data for a <see cref="Task" /> backlog.
    /// </summary>
    public sealed class TaskBacklogModel
    {
        private readonly List<TaskModel> taskModels;

        /// <summary>
        /// Creates a new backlog data model.
        /// </summary>
        /// <param name="taskModels">The <see cref="Task" />s to display on the model.</param>
        public TaskBacklogModel(List<TaskModel> taskModels)
        {
            this.taskModels = taskModels;
        }

        /// <summary>
        /// The <see cref="Task" />s to display on the model.
        /// </summary>
        public IEnumerable<TaskModel> TaskModels => taskModels;
    }
}