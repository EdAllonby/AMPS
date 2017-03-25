using System.Windows.Media;
using Client.Model;
using Client.Service;
using Shared;
using Shared.Domain;

namespace Client.ViewModel.MainViewModel
{
    /// <summary>
    /// Translates the <see cref="User" /> domain object for the View Model to use.
    /// </summary>
    public sealed class UserViewModel : ViewModel
    {
        private readonly Band bandToManage;
        private readonly User user;

        /// <summary>
        /// Creates a new view model with reference to the <see cref="Band" /> it is managing for.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="user">The <see cref="User" /> to display on the view.</param>
        /// <param name="bandToManage">The <see cref="Band" /> to manage.</param>
        public UserViewModel(IServiceRegistry serviceRegistry, User user, Band bandToManage)
            : base(serviceRegistry)
        {
            if (!IsInDesignMode)
            {
                this.user = user;
                this.bandToManage = bandToManage;
            }
        }

        /// <summary>
        /// The displayed <see cref="User" />'s Id.
        /// </summary>
        public int UserId => user.Id;

        /// <summary>
        /// The colour of the button.
        /// </summary>
        public Brush BorderColour => ConnectionStatusToColour();

        /// <summary>
        /// The user's name.
        /// </summary>
        public string Username => user.Username;

        /// <summary>
        /// The extra information to be displayed next to the <see cref="Username" />.
        /// </summary>
        public string ExtraUserInformation => GetExtraUsernameInformation();

        private string GetExtraUsernameInformation()
        {
            string extraInformation = string.Empty;

            int clientUserId = ServiceRegistry.GetService<IClientService>().ClientUserId;
            if (clientUserId.Equals(UserId))
            {
                extraInformation += " (You)";
            }

            if (bandToManage.Leader.Equals(user))
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