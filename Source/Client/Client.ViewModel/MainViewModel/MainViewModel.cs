using System;
using System.Windows;
using System.Windows.Input;
using Client.Service;
using Client.ViewModel.Commands;
using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;
using Utility;

namespace Client.ViewModel.MainViewModel
{
    /// <summary>
    /// View Model for Main window.
    /// </summary>
    public sealed class MainViewModel : ViewModel
    {
        private readonly Band band;
        private readonly BandRepository bandRepository;
        private readonly IClientService clientService;
        private readonly JamRepository jamRepository;
        private readonly ParticipationRepository participationRepository;
        private readonly int userId;
        private readonly IReadOnlyEntityRepository<User> userRepository;

        private string memberSearchTerm = string.Empty;

        /// <summary>
        /// Creates a new view model with reference to the <see cref="Band" /> it is managing for.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="band">The <see cref="Band" /> to manage.</param>
        public MainViewModel(IServiceRegistry serviceRegistry, Band band)
            : base(serviceRegistry)
        {
            if (!IsInDesignMode)
            {
                this.band = band;

                clientService = serviceRegistry.GetService<IClientService>();
                clientService.ClientDisconnected += OnClientDisconnected;
                userRepository = ServiceRegistry.GetService<RepositoryManager>().GetRepository<User>();
                participationRepository = (ParticipationRepository) ServiceRegistry.GetService<RepositoryManager>().GetRepository<Participation>();
                bandRepository = (BandRepository) ServiceRegistry.GetService<RepositoryManager>().GetRepository<Band>();
                jamRepository = (JamRepository) ServiceRegistry.GetService<RepositoryManager>().GetRepository<Jam>();

                jamRepository.EntityAdded += JamChanged;
                jamRepository.EntityUpdated += JamChanged;

                userId = ServiceRegistry.GetService<IClientService>().ClientUserId;
                bool isLeader = participationRepository.IsParticipantLeaderOfBand(userId, band.Id);

                if (isLeader)
                {
                    IsCreateJamVisible = Visibility.Visible;
                }
                if (!isLeader)
                {
                    IsCreateJamVisible = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// The title string.
        /// </summary>
        public string Title
        {
            get
            {
                string extraInformation = string.Empty;

                if (participationRepository.IsParticipantLeaderOfBand(userId, band.Id))
                {
                    extraInformation = "You are the leader of this Band.";
                }
                return string.Format("AMPS - Agile Music Project Suite: Welcome, {0}. {1}", userRepository.FindEntityById(userId).Username, extraInformation);
            }
        }

        /// <summary>
        /// The name of the managed <see cref="Band" />.
        /// </summary>
        public string BandName
        {
            get { return string.Format("Welcome to the Agile Music Project Suite for: {0}", bandRepository.FindEntityById(band.Id).Name); }
        }

        /// <summary>
        /// The details of the connected server.
        /// </summary>
        public string ServerConnectionStatus
        {
            get { return string.Format("Connected to Server at {0}:{1}", clientService.ServerEndPoint.Address, clientService.ServerEndPoint.Port); }
        }

        public string MemberSearchTerm
        {
            get { return memberSearchTerm; }
            set
            {
                memberSearchTerm = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Open a new <see cref="Band" /> details window.
        /// </summary>
        public ICommand OpenBandDetails
        {
            get { return new RelayCommand(OpenBandDetailsView); }
        }

        /// <summary>
        /// Open a new <see cref="Task" /> creator window.
        /// </summary>
        public ICommand CreateTask
        {
            get { return new RelayCommand(OpenCreateTaskView); }
        }

        /// <summary>
        /// Open a new <see cref="Band" /> <see cref="Task" /> backlog information window.
        /// </summary>
        public ICommand OpenTaskBacklog
        {
            get { return new RelayCommand(OpenTaskBacklogView); }
        }

        /// <summary>
        /// Log off the User and go to the login screen.
        /// </summary>
        public ICommand LogOffUser
        {
            get { return new RelayCommand(LogOffUserFromServer); }
        }

        /// <summary>
        /// Open a new application about box window.
        /// </summary>
        public ICommand OpenAboutBox
        {
            get { return new RelayCommand(OpenAboutBoxView); }
        }

        /// <summary>
        /// Opens a new <see cref="Jam" /> maker window.
        /// </summary>
        public ICommand OpenJamMaker
        {
            get { return new RelayCommand(OpenJamMakerView, CanOpenJamMakerView); }
        }

        /// <summary>
        /// Returns whether the user is a leader and can see the Create <see cref="Jam" /> button.
        /// </summary>
        public Visibility IsCreateJamVisible { get; private set; }

        private void OnClientDisconnected(object sender, EventArgs e)
        {
            EventUtility.SafeFireEvent(CloseMainAndOpenLoginViewRequested, this);
        }

        private void LogOffUserFromServer()
        {
            clientService.LogOff();
        }

        /// <summary>
        /// Fires when a <see cref="Band" /> details view is requested.
        /// </summary>
        public event EventHandler<WindowRequestedEventArgs> OpenBandDetailsViewRequested;

        /// <summary>
        /// Fires when a Create <see cref="Task" /> view is requested.
        /// </summary>
        public event EventHandler<WindowRequestedEventArgs> OpenCreateTaskViewRequested;

        /// <summary>
        /// Fires when a <see cref="Jam" /> maker view is requested.
        /// </summary>
        public event EventHandler<WindowRequestedEventArgs> OpenJamMakerViewRequested;

        /// <summary>
        /// Fires when a <see cref="Task" /> backlog view is requested.
        /// </summary>
        public event EventHandler<WindowRequestedEventArgs> OpenTaskBacklogViewRequested;

        /// <summary>
        /// Fires when the application About Box view is requested.
        /// </summary>
        public event EventHandler OpenAboutBoxViewRequested;

        /// <summary>
        /// Fires when a <see cref="Jam" /> has ended.
        /// </summary>
        public event EventHandler<JamEndedEventArgs> OnJamEnded;

        /// <summary>
        /// When the Login view should be shown again, the default state.
        /// </summary>
        public event EventHandler CloseMainAndOpenLoginViewRequested;

        private bool CanOpenJamMakerView()
        {
            Jam currentJam = jamRepository.GetCurrentActiveJamInBand(band.Id);
            return currentJam == null || !currentJam.IsActive;
        }

        private void OpenBandDetailsView()
        {
            Application.Current.Dispatcher.Invoke(() => EventUtility.SafeFireEvent(OpenBandDetailsViewRequested, this, new WindowRequestedEventArgs(band)));
        }

        private void OpenCreateTaskView()
        {
            Application.Current.Dispatcher.Invoke(() => EventUtility.SafeFireEvent(OpenCreateTaskViewRequested, this, new WindowRequestedEventArgs(band)));
        }

        private void OpenTaskBacklogView()
        {
            Application.Current.Dispatcher.Invoke(() => EventUtility.SafeFireEvent(OpenTaskBacklogViewRequested, this, new WindowRequestedEventArgs(band)));
        }

        private void OpenJamMakerView()
        {
            Application.Current.Dispatcher.Invoke(() => EventUtility.SafeFireEvent(OpenJamMakerViewRequested, this, new WindowRequestedEventArgs(band)));
        }

        private void OpenAboutBoxView()
        {
            Application.Current.Dispatcher.Invoke(() => EventUtility.SafeFireEvent(OpenAboutBoxViewRequested, this));
        }

        private void JamChanged(object sender, EntityChangedEventArgs<Jam> e)
        {
            if (e.NotificationType.Equals(NotificationType.Update) && !e.PreviousEntity.IsActive.Equals(e.Entity.IsActive) && !e.Entity.IsActive)
            {
                EventUtility.SafeFireEvent(OnJamEnded, this, new JamEndedEventArgs(e.Entity));
            }

            Application.Current.Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested);
        }
    }
}