using System.Collections.Generic;
using System.Linq;
using Shared.Domain;

namespace Client.Model.SettingsModel
{
    /// <summary>
    /// Models data for choosing an available <see cref="Band" /> the <see cref="User" /> is a member of.
    /// </summary>
    public sealed class BandChooserModel : NotifiableModel
    {
        private List<Band> bands;
        private Band selectedBand;

        /// <summary>
        /// Creates a new model for participating <see cref="Band" />s to enter main screen.
        /// </summary>
        /// <param name="bands"></param>
        public BandChooserModel(IEnumerable<Band> bands)
        {
            Bands = bands.ToList();
        }

        /// <summary>
        /// <see cref="Band" /> available to join.
        /// </summary>
        public List<Band> Bands
        {
            get { return bands; }
            set
            {
                if (Equals(value, bands)) return;
                bands = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The selected <see cref="Band" /> to join.
        /// </summary>
        public Band SelectedBand
        {
            get { return selectedBand; }
            set
            {
                if (Equals(value, selectedBand)) return;
                selectedBand = value;
                OnPropertyChanged();
            }
        }
    }
}