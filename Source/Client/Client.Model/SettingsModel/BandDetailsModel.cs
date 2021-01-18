using System.Collections.Generic;
using System.Linq;
using Shared.Domain;

namespace Client.Model.SettingsModel
{
    /// <summary>
    /// Models data for <see cref="Band" /> details.
    /// </summary>
    public sealed class BandDetailsModel : NotifiableModel
    {
        private Band band;
        private User bandLeader;
        private List<User> bandMembers;

        /// <summary>
        /// Create a new model for holding <see cref="Band" /> details data.
        /// </summary>
        /// <param name="bandMembers">The members of the <see cref="Band" />.</param>
        /// <param name="leader">The leader of the <see cref="Band" />.</param>
        /// <param name="band">The <see cref="Band" /> <see cref="Entity" /> to model this details page for.</param>
        public BandDetailsModel(Band band)
        {
            BandMembers = band.Members.ToList();
            BandLeader = band.Leader;
            Band = band;
        }

        /// <summary>
        /// The leader of the <see cref="Band" />.
        /// </summary>
        public User BandLeader
        {
            get => bandLeader;
            set
            {
                if (Equals(value, bandLeader)) return;
                bandLeader = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The <see cref="Band" /> <see cref="Entity" /> to model this details page for.
        /// </summary>
        public Band Band
        {
            get => band;
            set
            {
                if (Equals(value, band)) return;
                band = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The members of the <see cref="Band" />.
        /// </summary>
        public List<User> BandMembers
        {
            get => bandMembers;
            set
            {
                if (Equals(value, bandMembers)) return;
                bandMembers = value;
                OnPropertyChanged();
            }
        }
    }
}