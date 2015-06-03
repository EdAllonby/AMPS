using System;
using log4net;
using Shared;
using Utility;

namespace Client.Service
{
    public sealed class ToastNotificationManager : IService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (ToastNotificationManager));
        private int totalToastsToDisplay = 4;

        public ToastNotificationManager()
        {
            SendNotifications = true;
        }

        /// <summary>
        /// Can send notifications to subscribed clients.
        /// </summary>
        public bool SendNotifications { get; set; }

        public int TotalToastsToDisplay
        {
            get { return totalToastsToDisplay; }
            set
            {
                if (value > 0 || value < MaxNotificationsToDisplay)
                {
                    totalToastsToDisplay = value;
                }
            }
        }

        public int MaxNotificationsToDisplay
        {
            get { return 15; }
        }

        public event EventHandler<TostNotificationEventArgs> ToastNotificationRequested;

        /// <summary>
        /// Notify observers of a new Toast Notification.
        /// </summary>
        /// <param name="message">The message to add to the toast.</param>
        public void Notify(string message)
        {
            if (SendNotifications)
            {
                Log.Debug("Sent a toast message notification.");
                EventUtility.SafeFireEvent(ToastNotificationRequested, this, new TostNotificationEventArgs(message));
            }
        }
    }
}