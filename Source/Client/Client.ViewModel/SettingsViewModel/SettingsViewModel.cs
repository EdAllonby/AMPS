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
        private readonly ToastNotificationManager notifier;
        private readonly IFtpManager ftpManager;
        private bool isNotificationsEnabled;
        private int toastsToDisplay;

        private string ftpUsername;
        private string ftpPassword;
        private string ftpAddress;

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
            get { return isNotificationsEnabled;}

            set
            {
                isNotificationsEnabled = value;
                OnPropertyChanged();
            }
        }

        public int ToastsToDisplay
        {
            get { return toastsToDisplay; }
            set
            {
                toastsToDisplay = value;
                OnPropertyChanged();
            }
        }

        public int MaxToastsToDisplay { get; private set; }

        public string FTPUsername
        {
            get { return ftpUsername; }
            set
            {
                ftpUsername = value;
                OnPropertyChanged();
            }
        }

        public string FTPPassword
        {
            get { return ftpPassword; }
            set
            {
                ftpPassword = value;
                OnPropertyChanged();
            }
        }

        public string FTPAddress
        {
            get { return ftpAddress; }
            set
            {
                ftpAddress = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler Close;

        public ICommand DiscardSettings
        {
            get { return new RelayCommand(()=> EventUtility.SafeFireEvent(Close, this));}
        }

        public ICommand ApplySettings
        {
            get { return new RelayCommand(ApplyNewSettings);}
        }

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
