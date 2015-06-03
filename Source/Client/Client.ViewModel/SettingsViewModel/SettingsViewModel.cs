using System;
using System.Windows.Input;
using Client.Service;
using Client.ViewModel.Commands;
using Shared;
using Utility;

namespace Client.ViewModel.SettingsViewModel
{
    public sealed class SettingsViewModel : ViewModel
    {
        private readonly ToastNotificationManager notifier;
        private bool isNotificationsEnabled;
        private int toastsToDisplay;

        public SettingsViewModel(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
            notifier = serviceRegistry.GetService<ToastNotificationManager>();

            isNotificationsEnabled = notifier.SendNotifications;
            ToastsToDisplay = notifier.TotalToastsToDisplay;
            MaxToastsToDisplay = ToastNotificationManager.MaxNotificationsToDisplay;
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
            EventUtility.SafeFireEvent(Close, this);
        }
    }
}
