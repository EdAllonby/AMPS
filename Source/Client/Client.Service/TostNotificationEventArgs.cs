using System;

namespace Client.Service
{
    public sealed class TostNotificationEventArgs : EventArgs
    {
        private readonly string message;

        public TostNotificationEventArgs(string message)
        {
            this.message = message;
        }

        public string Message
        {
            get { return message; }
        }
    }
}