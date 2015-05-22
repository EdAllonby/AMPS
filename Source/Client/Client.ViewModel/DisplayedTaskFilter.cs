using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Client.Model.FilterType;
using Client.ViewModel.MainViewModel;
using log4net;
using Shared;
using Shared.Domain;

namespace Client.ViewModel
{
    /// <summary>
    /// Filter displayed tasks based on the filter type..
    /// </summary>
    public static class DisplayedTaskFilter
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (DisplayedTaskFilter));

        /// <summary>
        /// Filter the displayed tasks in the <see cref="ObservableCollection{TaskItemViewModel}" />.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="jamTasks"><see cref="Task" />s in the current <see cref="Jam" />.</param>
        /// <param name="selectedTaskFilter">The filter to use.</param>
        /// <param name="taskCategoriesToDisplay">The <see cref="Task" />s to display.</param>
        /// <param name="displayedTasks">The <see cref="Task" />s to be displayed, based on the <see cref="TaskFilter" />.</param>
        public static void FilterTasks(IServiceRegistry serviceRegistry, IEnumerable<Task> jamTasks, TaskFilter selectedTaskFilter, ICollection<TaskCategory> taskCategoriesToDisplay, ObservableCollection<TaskItemViewModel> displayedTasks)
        {
            switch (selectedTaskFilter)
            {
                case TaskFilter.Completed:
                    PopulateCompleted(serviceRegistry, jamTasks, taskCategoriesToDisplay, displayedTasks);
                    break;

                case TaskFilter.Uncompleted:
                    PopulateUncompleted(serviceRegistry, jamTasks, taskCategoriesToDisplay, displayedTasks);
                    break;

                case TaskFilter.All:
                    PopulateAll(serviceRegistry, jamTasks, taskCategoriesToDisplay, displayedTasks);
                    break;
            }
        }

        private static void PopulateCompleted(IServiceRegistry serviceRegistry, IEnumerable<Task> jamTasks, ICollection<TaskCategory> taskCategoriesToDisplay, ICollection<TaskItemViewModel> tasksInCurrentJam)
        {
            foreach (Task jamTask in jamTasks)
            {
                var taskModel = new TaskItemViewModel(serviceRegistry, jamTask);

                if (!tasksInCurrentJam.Contains(taskModel) && jamTask.IsCompleted && taskCategoriesToDisplay.Contains(jamTask.Category))
                {
                    Application.Current.Dispatcher.Invoke(() => tasksInCurrentJam.Add(taskModel));
                    Log.DebugFormat("Added task with id {0} to display.", jamTask.Id);
                }
                if (tasksInCurrentJam.Contains(taskModel) && (!jamTask.IsCompleted || !taskCategoriesToDisplay.Contains(jamTask.Category)))
                {
                    Application.Current.Dispatcher.Invoke(() => tasksInCurrentJam.Remove(taskModel));
                    Log.DebugFormat("Removed task with Id {0} from display.", jamTask.Id);
                }
            }
        }

        private static void PopulateUncompleted(IServiceRegistry serviceRegistry, IEnumerable<Task> jamTasks, ICollection<TaskCategory> taskCategoriesToDisplay, ICollection<TaskItemViewModel> tasksInCurrentJam)
        {
            foreach (Task jamTask in jamTasks)
            {
                var taskModel = new TaskItemViewModel(serviceRegistry, jamTask);

                if (!tasksInCurrentJam.Contains(taskModel) && !jamTask.IsCompleted)
                {
                    Application.Current.Dispatcher.Invoke(() => tasksInCurrentJam.Add(taskModel));
                    Log.DebugFormat("Added task with id {0} to display.", jamTask.Id);
                }
                if (tasksInCurrentJam.Contains(taskModel) && (jamTask.IsCompleted || !taskCategoriesToDisplay.Contains(jamTask.Category)))
                {
                    Application.Current.Dispatcher.Invoke(() => tasksInCurrentJam.Remove(taskModel));
                    Log.DebugFormat("Removed task with Id {0} from display.", jamTask.Id);
                }
            }
        }

        private static void PopulateAll(IServiceRegistry serviceRegistry, IEnumerable<Task> jamTasks, ICollection<TaskCategory> taskCategoriesToDisplay, ICollection<TaskItemViewModel> tasksInCurrentJam)
        {
            foreach (Task jamTask in jamTasks)
            {
                var taskModel = new TaskItemViewModel(serviceRegistry, jamTask);

                if (!tasksInCurrentJam.Contains(taskModel) && taskCategoriesToDisplay.Contains(jamTask.Category))
                {
                    Application.Current.Dispatcher.Invoke(() => tasksInCurrentJam.Add(taskModel));
                    Log.DebugFormat("Added task with id {0} to display.", jamTask.Id);
                }
                if (tasksInCurrentJam.Contains(taskModel) && taskCategoriesToDisplay.Contains(jamTask.Category))
                {
                    TaskItemViewModel currentTask = tasksInCurrentJam.First(model => model.Equals(taskModel));
                    currentTask.TaskModel.IsCompleted = taskModel.TaskModel.IsCompleted;
                    Log.DebugFormat("Updated task with id {0} to display.", jamTask.Id);
                }
                if (tasksInCurrentJam.Contains(taskModel) && !taskCategoriesToDisplay.Contains(jamTask.Category))
                {
                    Application.Current.Dispatcher.Invoke(() => tasksInCurrentJam.Remove(taskModel));
                    Log.DebugFormat("Removed task with Id {0} from display.", jamTask.Id);
                }
            }
        }
    }
}