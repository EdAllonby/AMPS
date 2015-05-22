using Shared.Domain;

namespace Client.Model.SettingsModel
{
    /// <summary>
    /// Models a <see cref="User" /> for a <see cref="Band" /> maker.
    /// </summary>
    public sealed class BandMakerUserModel : NotifiableModel
    {
        private readonly int clientId;
        private readonly User user;
        private bool isLeader;
        private bool isSelected;

        /// <summary>
        /// Create a new <see cref="User" /> model.
        /// </summary>
        /// <param name="user">A <see cref="User" /> who can enter the band.</param>
        /// <param name="clientId">used to assertain if the current <see cref="User" /> is the client.</param>
        public BandMakerUserModel(User user, int clientId)
        {
            this.user = user;
            this.clientId = clientId;

            if (clientId == user.Id)
            {
                IsSelected = true;
            }
        }

        /// <summary>
        /// A <see cref="User" /> who can enter the band.
        /// </summary>
        public User User
        {
            get { return user; }
        }

        /// <summary>
        /// If the <see cref="User" /> will enter the <see cref="Band" />.
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (Equals(value, isSelected)) return;
                isSelected = value;

                if (!isSelected)
                {
                    IsLeader = false;
                }

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// If it is possible to change selection.
        /// </summary>
        // ReSharper disable UnusedMember.Global
        public bool IsSelectedEnabled
            // ReSharper restore UnusedMember.Global
        {
            get { return !User.Id.Equals(clientId); }
        }

        /// <summary>
        /// Will the <see cref="User" /> be the leader of the <see cref="Band" />.
        /// </summary>
        public bool IsLeader
        {
            get { return isLeader; }
            set
            {
                if (Equals(value, isLeader)) return;
                isLeader = value;
                OnPropertyChanged();
            }
        }
    }
}