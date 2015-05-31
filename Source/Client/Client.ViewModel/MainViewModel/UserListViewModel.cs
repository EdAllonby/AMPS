using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Shared;
using Shared.Domain;
using Shared.Repository;

namespace Client.ViewModel.MainViewModel
{
    /// <summary>
    /// Holds the logic for the view. Accesses the Service manager to receive and send messages.
    /// </summary>
    public class UserListViewModel : ViewModel
    {
        private readonly int bandId;
        private readonly ParticipationRepository participationRepository;
        private readonly IReadOnlyEntityRepository<User> userRepository;
        private IList<UserViewModel> connectedUsers = new List<UserViewModel>();
        private string userFilter = string.Empty;

        /// <summary>
        /// Creates a new view model with reference to the <see cref="Band" /> it is managing for.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="bandId">The <see cref="Band" /> Id to manage.</param>
        public UserListViewModel(IServiceRegistry serviceRegistry, int bandId)
            : base(serviceRegistry)
        {
            if (IsInDesignMode)
            {
                return;
            }

            this.bandId = bandId;

            userRepository = serviceRegistry.GetService<RepositoryManager>().GetRepository<User>();
            participationRepository = (ParticipationRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Participation>();

            userRepository.EntityAdded += OnUserChanged;
            userRepository.EntityUpdated += OnUserChanged;
            participationRepository.EntityAdded += OnParticipantChanged;
            participationRepository.EntityUpdated += OnParticipantChanged;

            UpdateConnectedParticipants();
        }

        /// <summary>
        /// The members of the <see cref="Band" />.
        /// </summary>
        public IList<UserViewModel> ConnectedUsers
        {
            get { return connectedUsers; }
            set
            {
                if (Equals(value, connectedUsers))
                {
                    return;
                }

                connectedUsers = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The filter for the members in the <see cref="Band" />.
        /// </summary>
        public string UserFilter
        {
            get { return userFilter; }
            set
            {
                userFilter = value;
                OnPropertyChanged();
                UpdateConnectedParticipants();
            }
        }

        private void OnParticipantChanged(object sender, EntityChangedEventArgs<Participation> e)
        {
            // For when a member has been added to the Band
            UpdateConnectedParticipants();
        }

        private void OnUserChanged(object sender, EntityChangedEventArgs<User> e)
        {
            // For when a member has changed connection status (Logged in or out).
            UpdateConnectedParticipants();
        }

        private void UpdateConnectedParticipants()
        {
            IEnumerable<int> membersOfBandIds = participationRepository.GetParticipationsByBandId(bandId).Select(participant => participant.UserId);

            IEnumerable<User> allUsersInBand = userRepository.GetAllEntities().Where(user => membersOfBandIds.Contains(user.Id));
            IEnumerable<User> filteredUsers = allUsersInBand.Where(CanPresentUser);
            IEnumerable<UserViewModel> connectedUserViewModels = filteredUsers.Select(user => new UserViewModel(ServiceRegistry, user, bandId));

            ConnectedUsers = connectedUserViewModels.ToList();
        }

        private bool CanPresentUser(User unfilteredUsers)
        {
            string[] individualWordFilters = Regex.Split(UserFilter, @"\s");
            string[] usernameWords = Regex.Split(unfilteredUsers.Username, @"\s");

            foreach (string individualWordFilter in individualWordFilters)
            {
                bool isWordFilterValid = usernameWords.Any(usernameWord => usernameWord.StartsWith(individualWordFilter, StringComparison.InvariantCultureIgnoreCase));

                if (!isWordFilterValid)
                {
                    return false;
                }
            }

            return true;
        }
    }
}