using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Client.Model.FilterType;
using Client.ViewModel.Commands;
using Shared;
using Shared.Domain;
using Shared.Repository;
using Utility;

namespace Client.ViewModel.MainViewModel
{
    /// <summary>
    /// The View Model for the Jam Tasks view.
    /// </summary>
    public sealed class JamTasksViewModel : ViewModel
    {
        private readonly JamRepository jamRepository;
        private readonly Band managedBand;
        private readonly ICollection<TaskCategory> taskCategoriesFiltered = new List<TaskCategory>();
        private readonly TaskRepository taskRepository;
        private int completedPoints;
        private int remainingPoints;
        private List<string> selectedTaskCategories;
        private TaskFilter selectedTaskFilter = TaskFilter.All;
        private List<string> taskCategories;
        private int totalPoints;

        /// <summary>
        /// Create a new View Model for the <see cref="Jam" /> Tasks View.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="managedBand">The <see cref="Band" /> context.</param>
        public JamTasksViewModel(IServiceRegistry serviceRegistry, Band managedBand) : base(serviceRegistry)
        {
            this.managedBand = managedBand;
            taskRepository = (TaskRepository) ServiceRegistry.GetService<RepositoryManager>().GetRepository<Task>();
            jamRepository = (JamRepository) ServiceRegistry.GetService<RepositoryManager>().GetRepository<Jam>();

            DisplayedTasksInCurrentJam = new ObservableCollection<TaskItemViewModel>();

            RedisplayTasks();

            TaskCategories = new List<string>();

            List<string> defaultSelectedTaskCategories = new List<string> {"All"};

            foreach (TaskCategory taskCategory in EnumUtility.EnumToEnumerable<TaskCategory>())
            {
                TaskCategories.Add(taskCategory.ToString());
                defaultSelectedTaskCategories.Add(taskCategory.ToString());
            }

            SelectedTaskCategories = defaultSelectedTaskCategories;

            jamRepository.EntityAdded += OnJamAdded;
            taskRepository.EntityUpdated += OnTaskUpdated;
            UpdatePoints();
        }

        /// <summary>
        /// The task filters available.
        /// </summary>
        public static IEnumerable<TaskFilter> TaskFilters => EnumUtility.EnumToEnumerable<TaskFilter>();

        /// <summary>
        /// The task catgories available.
        /// </summary>
        public List<string> TaskCategories
        {
            get { return taskCategories; }
            set
            {
                taskCategories = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The selected task categories to display.
        /// </summary>
        public List<string> SelectedTaskCategories
        {
            get { return selectedTaskCategories; }
            set
            {
                taskCategoriesFiltered.Clear();

                selectedTaskCategories = value;

                foreach (string category in selectedTaskCategories)
                {
                    TaskCategory taskCategory;

                    if (Enum.TryParse(category, out taskCategory))
                    {
                        taskCategoriesFiltered.Add(taskCategory);
                    }
                }

                RedisplayTasks();

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The current task filter.
        /// </summary>
        public TaskFilter SelectedTaskFilter
        {
            get { return selectedTaskFilter; }
            set
            {
                if (Equals(value, selectedTaskFilter)) return;

                selectedTaskFilter = value;
                RedisplayTasks();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The current Tasks in the Jam.
        /// </summary>
        // ReSharper disable CollectionNeverQueried.Global
        public ObservableCollection<TaskItemViewModel> DisplayedTasksInCurrentJam { get; }

        // ReSharper restore CollectionNeverQueried.Global
        /// <summary>
        /// The total points in this <see cref="Jam" />.
        /// </summary>
        public int CompletedPoints
        {
            get { return completedPoints; }
            set
            {
                if (Equals(value, completedPoints)) return;
                completedPoints = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The total points in this <see cref="Jam" />.
        /// </summary>
        public int RemainingPoints
        {
            get { return remainingPoints; }
            set
            {
                if (Equals(value, remainingPoints)) return;
                remainingPoints = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The total points in this <see cref="Jam" />.
        /// </summary>
        public int TotalPoints
        {
            get { return totalPoints; }
            set
            {
                if (Equals(value, totalPoints)) return;
                totalPoints = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Open the admin view.
        /// </summary>
        public ICommand OpenAdminView => new RelayCommand(OpenNewAdminView, CanOpenAdminView);

        /// <summary>
        /// Fires when an admin screen is requested to open.
        /// </summary>
        public event EventHandler<WindowRequestedEventArgs> OpenAdminViewRequested;

        private void OpenNewAdminView()
        {
            EventUtility.SafeFireEvent(OpenAdminViewRequested, this, new WindowRequestedEventArgs(managedBand));
        }

        private bool CanOpenAdminView()
        {
            return true;
        }

        private void OnTaskUpdated(object sender, EntityChangedEventArgs<Task> e)
        {
            RedisplayTasks();
        }

        private void OnJamAdded(object sender, EntityChangedEventArgs<Jam> e)
        {
            if (e.Entity.Band.Equals(managedBand))
            {
                RedisplayTasks();
            }
        }

        private void RedisplayTasks()
        {
            Jam activeJam = jamRepository.GetCurrentActiveJamInBand(managedBand.Id);

            if (activeJam != null)
            {
                DisplayedTaskFilter.FilterTasks(ServiceRegistry, taskRepository.GetTasksInJam(activeJam.Id), SelectedTaskFilter, taskCategoriesFiltered, DisplayedTasksInCurrentJam);

                UpdatePoints();

                Application.Current.Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested);
            }
        }

        private void UpdatePoints()
        {
            Jam activeJam = jamRepository.GetCurrentActiveJamInBand(managedBand.Id);

            if (activeJam != null)
            {
                List<Task> tasksInJam = taskRepository.GetTasksInJam(activeJam.Id).ToList();

                TotalPoints = tasksInJam.Sum(taskInJam => taskInJam.Points);
                CompletedPoints = tasksInJam.Where(taskInJam => taskInJam.IsCompleted).Sum(taskInJam => taskInJam.Points);
                RemainingPoints = tasksInJam.Where(taskInJam => !taskInJam.IsCompleted).Sum(taskInJam => taskInJam.Points);
            }
        }
    }
}