using System;

namespace Client.Service
{
    public sealed class TostNotificationEventArgs : EventArgs
    {
        private readonly Notification notification;

        public TostNotificationEventArgs(Notification notification)
        {
            this.notification = notification;
        }

        public Notification Notification
        {
            get { return notification; }
        }
    }
}