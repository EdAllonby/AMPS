using System.Linq;
using System.Windows.Media;
using Client.Model;
using Client.Service;
using Shared;
using Shared.Domain;
using Shared.Repository;

namespace Client.ViewModel.MainViewModel
{
    /// <summary>
    /// Translates the <see cref="User" /> domain object for the View Model to use.
    /// </summary>
    public sealed class UserViewModel : ViewModel
    {
        private readonly int bandId;
        private readonly User user;

        /// <summary>
        /// Creates a new view model with reference to the <see cref="Band" /> it is managing for.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="user">The <see cref="User" /> to display on the view.</param>
        /// <param name="bandId">The <see cref="Band" /> Id to manage.</param>
        public UserViewModel(IServiceRegistry serviceRegistry, User user, int bandId)
            : base(serviceRegistry)
        {
            if (!IsInDesignMode)
            {
                this.user = user;
                this.bandId = bandId;
            }
        }

        /// <summary>
        /// The displayed <see cref="User" />'s Id.
        /// </summary>
        public int UserId
        {
            get { return user.Id; }
        }

        /// <summary>
        /// The colour of the button.
        /// </summary>
        public Brush BorderColour
        {
            get { return ConnectionStatusToColour(); }
        }

        /// <summary>
        /// The user's name.
        /// </summary>
        public string Username
        {
            get { return user.Username; }
        }

        /// <summary>
        /// The extra information to be displayed next to the <see cref="Username" />.
        /// </summary>
        public string ExtraUserInformation
        {
            get { return GetExtraUsernameInformation(); }
        }

        private string GetExtraUsernameInformation()
        {
            string extraInformation = string.Empty;

            int clientUserId = ServiceRegistry.GetService<IClientService>().ClientUserId;
            if (clientUserId.Equals(UserId))
            {
                extraInformation += " (You)";
            }

            ParticipationRepository participationRepository =
                (ParticipationRepository) ServiceRegistry.GetService<RepositoryManager>().GetRepository<Participation>();

            Participation leaderParticipant =
                participationRepository.GetParticipationsByBandId(bandId).First(participant => participant.IsLeader);

            if (leaderParticipant.UserId.Equals(UserId))
            {
                extraInformation += " (Leader)";
            }

            return extraInformation;
        }

        private Brush ConnectionStatusToColour()
        {
            ConnectionStatus connectionStatus = user.ConnectionStatus;
            switch (connectionStatus.UserConnectionStatus)
            {
                case ConnectionStatus.Status.Connected:
                    return UserColours.ConnectedColour;

                case ConnectionStatus.Status.Disconnected:
                    return UserColours.DisconnectedColour;

                default:
                    return UserColours.UnknownColour;
            }
        }
    }
}