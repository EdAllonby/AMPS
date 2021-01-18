using System.Collections.Generic;
using System.Linq;
using Shared.Domain;
using Utility;

namespace Client.Model.SettingsModel
{
    /// <summary>
    /// A <see cref="Task" /> model which can be set to add.
    /// </summary>
    public sealed class AddableTaskModel : TaskModel
    {
        private bool add;
        private List<User> bandMembers;
        private List<int> pointsList;

        /// <summary>
        /// Create a new Addable <see cref="TaskModel" />, which can be edited.
        /// </summary>
        /// <param name="task">The <see cref="Task" /> to model.</param>
        /// <param name="bandMembers">The members of the band to assign the task.</param>
        public AddableTaskModel(Task task, List<User> bandMembers)
            : base(task)
        {
            BandMembers = bandMembers;
            PointsList = NumbersGenerator.CreateFibonacciSequence(8, true).ToList();
        }

        /// <summary>
        /// Whether the <see cref="Task" /> should be added in the given context.
        /// </summary>
        public bool Add
        {
            get => add;
            set
            {
                if (Equals(value, add)) return;
                add = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Task points available.
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
        /// Members of the <see cref="Band" />.
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
    }
}