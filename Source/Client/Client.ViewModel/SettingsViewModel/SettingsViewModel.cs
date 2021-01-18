using System;
using System.Windows.Input;
using Client.Service;
using Client.Service.FTPService;
using Client.ViewModel.Commands;
using Shared;
using Utility;

namespace Client.ViewModel.SettingsViewModel
{
    public sealed class SettingsViewModel : ViewModel
    {
        private readonly IFtpManager ftpManager;
        private readonly ToastNotificationManager notifier;
        private string ftpAddress;
        private string ftpPassword;
        private string ftpUsername;
        private bool isNotificationsEnabled;
        private int toastsToDisplay;

        public SettingsViewModel(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
            notifier = serviceRegistry.GetService<ToastNotificationManager>();

            isNotificationsEnabled = notifier.SendNotifications;
            ToastsToDisplay = notifier.TotalToastsToDisplay;
            MaxToastsToDisplay = ToastNotificationManager.MaxNotificationsToDisplay;

            ftpManager = serviceRegistry.GetService<IFtpManager>();
            FTPAddress = ftpManager.Address;
            FTPUsername = ftpManager.Username;
            FTPPassword = ftpManager.Password;
        }

        public bool IsNotificationsEnabled
        {
            get => isNotificationsEnabled;

            set
            {
                isNotificationsEnabled = value;
                OnPropertyChanged();
            }
        }

        public int ToastsToDisplay
        {
            get => toastsToDisplay;
            set
            {
                toastsToDisplay = value;
                OnPropertyChanged();
            }
        }

        public int MaxToastsToDisplay { get; }

        public string FTPUsername
        {
            get => ftpUsername;
            set
            {
                ftpUsername = value;
                OnPropertyChanged();
            }
        }

        public string FTPPassword
        {
            get => ftpPassword;
            set
            {
                ftpPassword = value;
                OnPropertyChanged();
            }
        }

        public string FTPAddress
        {
            get => ftpAddress;
            set
            {
                ftpAddress = value;
                OnPropertyChanged();
            }
        }

        public ICommand DiscardSettings => new RelayCommand(() => EventUtility.SafeFireEvent(Close, this));
        public ICommand ApplySettings => new RelayCommand(ApplyNewSettings);
        public event EventHandler Close;

        private void ApplyNewSettings()
        {
            notifier.SendNotifications = IsNotificationsEnabled;
            notifier.TotalToastsToDisplay = ToastsToDisplay;

            ftpManager.Address = FTPAddress;
            ftpManager.Username = FTPUsername;
            ftpManager.Password = FTPPassword;

            EventUtility.SafeFireEvent(Close, this);
        }
    }
}