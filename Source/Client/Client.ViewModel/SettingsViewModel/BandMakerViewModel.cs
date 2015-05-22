using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Client.Model.SettingsModel;
using Client.Service;
using Client.ViewModel.Commands;
using Shared;
using Shared.Domain;
using Shared.Repository;
using Utility;

namespace Client.ViewModel.SettingsViewModel
{
    /// <summary>
    /// Holds the logic for the view. Accesses the Service manager to receive and send messages. 
    /// </summary>
    public class BandMakerViewModel : ViewModel
    {
        private readonly BandRepository bandRepository;
        private readonly int clientId;
        private readonly IClientService clientService;
        private readonly ParticipationRepository participationRepository;
        private readonly UserRepository userRepository;
        private BandMakerModel bandMakerModel;

        /// <summary>
        /// Creates a new view model for band maker.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry"/>.</param>
        public BandMakerViewModel(IServiceRegistry serviceRegistry)
            : base(serviceRegistry)
        {
            bandRepository = (BandRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Band>();

            bandRepository.EntityAdded += OnBandsChanged;
            bandRepository.EntityUpdated += OnBandsChanged;

            userRepository = (UserRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<User>();
            participationRepository = (ParticipationRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Participation>();
            clientService = serviceRegistry.GetService<IClientService>();

            userRepository.EntityAdded += OnUserAdded;

            clientId = serviceRegistry.GetService<IClientService>().ClientUserId;

            IEnumerable<User> usersToDisplay = userRepository.GetAllEntities();

            bandMakerModel = new BandMakerModel(bandRepository.GetAllEntities(), usersToDisplay, clientId);
        }

        /// <summary>
        /// The underlying model.
        /// </summary>
        public BandMakerModel BandMakerModel
        {
            get { return bandMakerModel; }
            set
            {
                if (Equals(value, bandMakerModel)) return;
                bandMakerModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// try and create a new <see cref="Band" /> on the server.
        /// </summary>
        public ICommand CreateBand
        {
            get { return new RelayCommand(CreateNewBand, CanCreateBand); }
        }

        /// <summary>
        /// Try to join membership with a <see cref="Band" />.
        /// </summary>
        public ICommand JoinBand
        {
            get { return new RelayCommand(JoinNewBand, CanJoinBand); }
        }

        /// <summary>
        /// Fires when requesting the main band management view.
        /// </summary>
        public event EventHandler<WindowRequestedEventArgs> OpenMainViewRequested;

        private void OnBandsChanged(object sender, EntityChangedEventArgs<Band> e)
        {
            BandMakerModel.Bands = bandRepository.GetAllEntities().ToList();

            IEnumerable<int> bandIds = participationRepository.GetAllBandIdsByUserId(clientId);
            if (bandIds.Any())
            {
                Application.Current.Dispatcher.Invoke(() => EventUtility.SafeFireEvent(OpenMainViewRequested, this, new WindowRequestedEventArgs(e.Entity)));
            }
        }

        private void OnUserAdded(object sender, EntityChangedEventArgs<User> e)
        {
            UpdateUsers();
        }

        private void UpdateUsers()
        {
            BandMakerModel.BandMakerUsers = userRepository
                .GetAllEntities()
                .Select(user => new BandMakerUserModel(user, clientId)).ToList();
        }

        private bool CanJoinBand()
        {
            return BandMakerModel.SelectedBand != null;
        }

        private void JoinNewBand()
        {
            clientService.AddUserToBand(clientId, BandMakerModel.SelectedBand.Id, false);
        }

        private bool CanCreateBand()
        {
            return !string.IsNullOrWhiteSpace(BandMakerModel.BandName) && IsOneOrMoreUserSelected() && IsALeaderSelected();
        }

        private bool IsOneOrMoreUserSelected()
        {
            return BandMakerModel.BandMakerUsers.Any(bandMakerUserModel => bandMakerUserModel.IsSelected);
        }

        private bool IsALeaderSelected()
        {
            return BandMakerModel.BandMakerUsers.Any(bandMakerUserModel => bandMakerUserModel.IsLeader);
        }

        private void CreateNewBand()
        {
            IEnumerable<int> bandUserIds = from bandMakerUserModel in BandMakerModel.BandMakerUsers
                where bandMakerUserModel.IsSelected
                select bandMakerUserModel.User.Id;

            int bandLeaderId = GetLeaderId();

            if (bandLeaderId == 0)
            {
                Log.Error("No leader has been selected.");
            }
            else
            {
                clientService.CreateBand(bandUserIds.ToList(), BandMakerModel.BandName, bandLeaderId);
            }
        }

        private int GetLeaderId()
        {
            return (from bandMakerUserModel in BandMakerModel.BandMakerUsers
                where bandMakerUserModel.IsLeader
                select bandMakerUserModel.User.Id).FirstOrDefault();
        }
    }
}