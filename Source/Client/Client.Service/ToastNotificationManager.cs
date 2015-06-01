using System;
using log4net;
using Shared;
using Utility;

namespace Client.Service
{
    public sealed class ToastNotificationManager : IService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (ToastNotificationManager));

        public ToastNotificationManager()
        {
            SendNotifications = true;
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

        /// <summary>
        /// Can send notifications to subscribed clients.
        /// </summary>
        public bool SendNotifications { get; set; }
    }
}