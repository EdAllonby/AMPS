using System.Collections.Generic;
using System.Linq;
using Shared.Domain;

namespace Client.Model.SettingsModel
{
    /// <summary>
    /// Models data for b<see cref="Band" /> creation.
    /// </summary>
    public sealed class BandMakerModel : NotifiableModel
    {
        private List<BandMakerUserModel> bandMakerUsers = new List<BandMakerUserModel>();
        private string bandName = string.Empty;
        private List<Band> bands;
        private Band selectedBand;

        /// <summary>
        /// Creates a new model for <see cref="Band" /> creation.
        /// </summary>
        /// <param name="bands"><see cref="Band" /> available to join.</param>
        /// <param name="users"><see cref="User" />s who can join a <see cref="Band" />.</param>
        /// <param name="clientUserId">The Id of the client wanting to create a <see cref="Band" />/</param>
        public BandMakerModel(IEnumerable<Band> bands, IEnumerable<User> users, int clientUserId)
        {
            Bands = bands.ToList();

            foreach (User user in users)
            {
                bandMakerUsers.Add(new BandMakerUserModel(user, clientUserId));
            }
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
        /// A model of <see cref="User" />s who can join a <see cref="Band" />.
        /// </summary>
        public List<BandMakerUserModel> BandMakerUsers
        {
            get { return bandMakerUsers; }
            set
            {
                if (Equals(value, bandMakerUsers)) return;
                bandMakerUsers = value;
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

        /// <summary>
        /// The name of the <see cref="Band" />.
        /// </summary>
        public string BandName
        {
            get { return bandName; }
            set
            {
                if (Equals(value, bandName)) return;
                bandName = value;
                OnPropertyChanged();
            }
        }
    }
}