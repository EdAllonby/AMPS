#region

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Client.Model.FilterType;
using Client.ViewModel.MainViewModel;
using log4net;
using Shared;
using Shared.Domain;

#endregion

namespace Client.ViewModel
{
    /// <summary>
    /// Filter displayed tasks based on the filter type..
    /// </summary>
    public static class DisplayedTaskFilter
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (DisplayedTaskFilter));

        /// <summary>
        /// <para>
        /// Filter the displayed tasks in the
        /// <see cref="System.Collections.ObjectModel.ObservableCollection`1" />
        /// </para>
        /// <para>.</para>
        /// </summary>
        /// <param name="serviceRegistry">
        /// The client's <see cref="IServiceRegistry" /> .
        /// </param>
        /// <param name="jamTasks">
        /// <see cref="Task" /> s in the current <see cref="Jam" /> .
        /// </param>
        /// <param name="selectedTaskFilter">The filter to use.</param>
        /// <param name="taskCategoriesToDisplay">
        /// The <see cref="Task" /> s to display.
        /// </param>
        /// <param name="displayedTasks">
        /// The <see cref="Task" /> s to be displayed, based on the
        /// <see cref="TaskFilter" /> .
        /// </param>
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
            foreach (var jamTask in jamTasks)
            {
                if (!DoesCollectionContainTask(tasksInCurrentJam, jamTask) && jamTask.IsCompleted && taskCategoriesToDisplay.Contains(jamTask.Category))
                {
                    Application.Current.Dispatcher.Invoke(() => tasksInCurrentJam.Add(new TaskItemViewModel(serviceRegistry, jamTask)));
                    Log.DebugFormat("Added task with id {0} to display.", jamTask.Id);
                }
                if (DoesCollectionContainTask(tasksInCurrentJam, jamTask) && (!jamTask.IsCompleted || !taskCategoriesToDisplay.Contains(jamTask.Category)))
                {
                    TryRemoveTaskFromViewModels(tasksInCurrentJam, jamTask);
                    Log.DebugFormat("Removed task with Id {0} from display.", jamTask.Id);
                }
            }
        }

        private static void PopulateUncompleted(IServiceRegistry serviceRegistry, IEnumerable<Task> jamTasks, ICollection<TaskCategory> taskCategoriesToDisplay, ICollection<TaskItemViewModel> tasksInCurrentJam)
        {
            foreach (var jamTask in jamTasks)
            {
                if (!DoesCollectionContainTask(tasksInCurrentJam, jamTask) && !jamTask.IsCompleted)
                {
                    Application.Current.Dispatcher.Invoke(() => tasksInCurrentJam.Add(new TaskItemViewModel(serviceRegistry, jamTask)));
                    Log.DebugFormat("Added task with id {0} to display.", jamTask.Id);
                }
                if (DoesCollectionContainTask(tasksInCurrentJam, jamTask) && (jamTask.IsCompleted || !taskCategoriesToDisplay.Contains(jamTask.Category)))
                {
                    TryRemoveTaskFromViewModels(tasksInCurrentJam, jamTask);
                    Log.DebugFormat("Removed task with Id {0} from display.", jamTask.Id);
                }
            }
        }

        private static void PopulateAll(IServiceRegistry serviceRegistry, IEnumerable<Task> jamTasks, ICollection<TaskCategory> taskCategoriesToDisplay, ICollection<TaskItemViewModel> tasksInCurrentJam)
        {
            foreach (var jamTask in jamTasks)
            {
                if (!DoesCollectionContainTask(tasksInCurrentJam, jamTask) && taskCategoriesToDisplay.Contains(jamTask.Category))
                {
                    Application.Current.Dispatcher.Invoke(() => tasksInCurrentJam.Add(new TaskItemViewModel(serviceRegistry, jamTask)));
                    Log.DebugFormat("Added task with id {0} to display.", jamTask.Id);
                }
                if (DoesCollectionContainTask(tasksInCurrentJam, jamTask) && taskCategoriesToDisplay.Contains(jamTask.Category))
                {
                    var currentTask = tasksInCurrentJam.First(model => model.TaskModel.TaskId.Equals(jamTask.Id));
                    currentTask.TaskModel.IsCompleted = jamTask.IsCompleted;
                    Log.DebugFormat("Updated task with id {0} to display.", jamTask.Id);
                }
                if (DoesCollectionContainTask(tasksInCurrentJam, jamTask) && !taskCategoriesToDisplay.Contains(jamTask.Category))
                {
                    TryRemoveTaskFromViewModels(tasksInCurrentJam, jamTask);

                    Log.DebugFormat("Removed task with Id {0} from display.", jamTask.Id);
                }
            }
        }

        private static void TryRemoveTaskFromViewModels(ICollection<TaskItemViewModel> tasksInCurrentJam, Task jamTask)
        {
            var viewModel = tasksInCurrentJam.FirstOrDefault(x => x.TaskModel.TaskId.Equals(jamTask.Id));

            if (viewModel != null)
            {
                viewModel.UnsubscribeEvents();

                Application.Current.Dispatcher.Invoke(() => tasksInCurrentJam.Remove(viewModel));
            }
        }

        private static bool DoesCollectionContainTask(IEnumerable<TaskItemViewModel> tasksInCurrentJam, Task jamTask)
        {
            return tasksInCurrentJam.Any(x => x.TaskModel.TaskId.Equals(jamTask.Id));
        }
    }
}