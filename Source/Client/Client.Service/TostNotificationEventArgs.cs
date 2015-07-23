using System;

namespace Client.Service
{
    public sealed class TostNotificationEventArgs : EventArgs
    {
        public TostNotificationEventArgs(Notification notification)
        {
            this.Notification = notification;
        }

        public Notification Notification { get; }
    }
}