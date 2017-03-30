using System.Windows.Media;
using Shared.Domain;

namespace Client.Model.SettingsModel
{
    /// <summary>
    /// Represents a <see cref="Task" /> for the UI with the full <see cref="User" />.
    /// </summary>
    public class TaskModel : NotifiableModel
    {
        private User assignedMember;
        private TaskCategory category;
        private bool isCompleted;
        private int points;
        private Brush taskCompletedColour;

        /// <summary>
        /// Create a new model for a <see cref="Task" />.
        /// </summary>
        /// <param name="task">The <see cref="Task" /> to model.</param>
        public TaskModel(Task task)
        {
            Task = task;
            TaskId = task.Id;
            Title = task.Title;
            Description = task.Description;
            AssignedMember = task.AssignedUser;
            Points = task.Points;
            IsCompleted = task.IsCompleted;
            category = task.Category;
            TaskCompletedColour = IsCompletedColour();
        }

        /// <summary>
        /// The modelled task.
        /// </summary>
        public Task Task { get; }

        /// <summary>
        /// The Id of the <see cref="Task" />
        /// </summary>
        public int TaskId { get; }

        /// <summary>
        /// The title of the <see cref="Task" />.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// A description of the <see cref="Task" />.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The <see cref="Band" />  member who this <see cref="Task" /> is assigned to.
        /// </summary>
        public User AssignedMember
        {
            get { return assignedMember; }
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
        /// The number of points this <see cref="Task" /> has.
        /// </summary>
        public int Points
        {
            get { return points; }
            set
            {
                if (Equals(value, points) || value < 0)
                {
                    return;
                }

                points = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Is the <see cref="Task" /> completed.
        /// </summary>
        public bool IsCompleted
        {
            get { return isCompleted; }
            set
            {
                if (Equals(value, isCompleted))
                {
                    return;
                }

                isCompleted = value;

                TaskCompletedColour = IsCompletedColour();

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The category of the task.
        /// </summary>
        public TaskCategory Category
        {
            get { return category; }
            set
            {
                if (Equals(value, category))
                {
                    return;
                }

                category = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Colour to use when the <see cref="Task" /> is complete.
        /// </summary>
        public Brush TaskCompletedColour
        {
            get { return taskCompletedColour; }
            set
            {
                if (Equals(value, taskCompletedColour))
                {
                    return;
                }

                taskCompletedColour = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The task's background colour.
        /// </summary>
        public Brush TaskBackgroundColour => CategoryColour();

        private Brush CategoryColour()
        {
            if (Category.Equals(TaskCategory.Guitar))
            {
                return ColourPalette.GuitarColour;
            }
            if (Category.Equals(TaskCategory.Bass))
            {
                return ColourPalette.BassColour;
            }
            if (Category.Equals(TaskCategory.Drums))
            {
                return ColourPalette.DrumColour;
            }
            if (Category.Equals(TaskCategory.Vocals))
            {
                return ColourPalette.VocalColour;
            }
            if (Category.Equals(TaskCategory.Synth))
            {
                return ColourPalette.SynthColour;
            }
            if (Category.Equals(TaskCategory.Mixing))
            {
                return ColourPalette.MixingColour;
            }
            if (Category.Equals(TaskCategory.Other))
            {
                return ColourPalette.OtherColour;
            }
            if (Category.Equals(TaskCategory.Project))
            {
                return ColourPalette.ProjectColour;
            }

            return ColourPalette.UnknownColour;
        }

        private Brush IsCompletedColour()
        {
            return IsCompleted ? TaskColours.CompletedTaskColour : TaskColours.UncompletedTaskColour;
        }
    }
}