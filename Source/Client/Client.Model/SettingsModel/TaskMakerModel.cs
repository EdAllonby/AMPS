using System.Collections.Generic;
using System.Linq;
using Shared.Domain;
using Utility;

namespace Client.Model.SettingsModel
{
    /// <summary>
    /// Models data for a new <see cref="Task" /> to be created.
    /// </summary>
    public sealed class TaskMakerModel : NotifiableModel
    {
        private User assignedMember;
        private List<User> bandMembers;
        private List<int> pointsList;
        private TaskCategory taskCategory = TaskCategory.Guitar;
        private string taskDescription = string.Empty;
        private int taskPoints;
        private string taskTitle = string.Empty;

        /// <summary>
        /// Create a new model for a <see cref="Task" /> to be created.
        /// </summary>
        public TaskMakerModel(List<User> bandMembers)
        {
            BandMembers = bandMembers;
            PointsList = NumbersGenerator.CreateFibonacciSequence(8, true).ToList();
        }

        /// <summary>
        /// The title of the <see cref="Task" />.
        /// </summary>
        public string TaskTitle
        {
            get => taskTitle;
            set
            {
                if (Equals(value, taskTitle))
                {
                    return;
                }
                taskTitle = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// A description of the <see cref="Task" />.
        /// </summary>
        public string TaskDescription
        {
            get => taskDescription;
            set
            {
                if (Equals(value, taskDescription))
                {
                    return;
                }
                taskDescription = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The number of points this <see cref="Task" /> has.
        /// </summary>
        public int TaskPoints
        {
            get => taskPoints;
            set
            {
                if (Equals(value, taskPoints) || value < 0)
                {
                    return;
                }

                taskPoints = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The points to choose from.
        /// </summary>
        public List<int> PointsList
        {
            get => pointsList;
            set
            {
                if (Equals(value, pointsList))
                {
                    return;
                }

                pointsList = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The members of the <see cref="Task" />'s <see cref="Band" />.
        /// </summary>
        public List<User> BandMembers
        {
            get => bandMembers;
            set
            {
                if (Equals(value, bandMembers))
                {
                    return;
                }
                bandMembers = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The <see cref="Band" />  member who this <see cref="Task" /> is assigned to.
        /// </summary>
        public User AssignedMember
        {
            get => assignedMember;
            set
            {
                if (Equals(value, assignedMember))
                {
                    return;
                }
                assignedMember = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Catagory types to choose from.
        /// </summary>
        public static IEnumerable<TaskCategory> TaskCategories => EnumUtility.EnumToEnumerable<TaskCategory>();

        /// <summary>
        /// The chosen <see cref="TaskCategory" />.
        /// </summary>
        public TaskCategory TaskCategory
        {
            get => taskCategory;
            set
            {
                if (Equals(value, taskCategory))
                {
                    return;
                }
                taskCategory = value;
                OnPropertyChanged();
            }
        }
    }
}