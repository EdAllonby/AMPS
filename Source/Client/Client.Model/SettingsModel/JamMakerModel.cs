using System.Collections.Generic;
using System.Linq;
using Shared.Domain;
using Utility;

namespace Client.Model.SettingsModel
{
    /// <summary>
    /// Model for <see cref="Jam" /> Maker Window.
    /// </summary>
    public sealed class JamMakerModel : NotifiableModel
    {
        private List<AddableTaskModel> addableTask;
        private int jamDayLength;

        /// <summary>
        /// Creates a new <see cref="Jam" /> Maker Model with addable <see cref="Task" />s.
        /// </summary>
        /// <param name="addableTasks">The tasks that can be added to the <see cref="Jam" />.</param>
        public JamMakerModel(List<AddableTaskModel> addableTasks)
        {
            AddableTasks = addableTasks;
            DaysList = NumbersGenerator.CreateNumberSequence(1, 7, 1).ToList();
        }

        /// <summary>
        /// A collection of addable <see cref="Task" />s for the <see cref="Jam" />.
        /// </summary>
        public List<AddableTaskModel> AddableTasks
        {
            get { return addableTask; }
            set
            {
                if (Equals(value, addableTask)) return;
                addableTask = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The number of days the <see cref="Jam" /> will last.
        /// </summary>
        public int JamDayLength
        {
            get { return jamDayLength; }
            set
            {
                if (Equals(value, jamDayLength)) return;
                jamDayLength = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The available days allowed.
        /// </summary>
        public List<int> DaysList { get; private set; }
    }
}