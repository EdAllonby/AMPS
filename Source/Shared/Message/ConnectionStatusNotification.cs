using Shared.Domain;

namespace Shared.Message
{
    /// <summary>
    /// Signifies when a <see cref="User" />'s <see cref="ConnectionStatus" /> has changed.
    /// </summary>
    public sealed class ConnectionStatusNotification : IMessage
    {
        private readonly ConnectionStatus connectionStatus;
        private readonly NotificationType notificationType;

        /// <summary>
        /// Create a new <see cref="ConnectionStatus" /> notification <see cref="IMessage" />.
        /// </summary>
        /// <param name="connectionStatus">The new <see cref="User" /> <see cref="ConnectionStatus" />.</param>
        /// <param name="notificationType">The entity modification to perform.</param>
        public ConnectionStatusNotification(ConnectionStatus connectionStatus, NotificationType notificationType)
        {
            this.connectionStatus = connectionStatus;
            this.notificationType = notificationType;
        }

        /// <summary>
        /// The new <see cref="User" /> <see cref="ConnectionStatus" />.
        /// </summary>
        public ConnectionStatus ConnectionStatus
        {
            get { return connectionStatus; }
        }

        /// <summary>
        /// The entity modification to perform.
        /// </summary>
        public NotificationType NotificationType
        {
            get { return notificationType; }
        }

        /// <summary>
        /// The identifier for this message notification type..
        /// </summary>
        public MessageIdentifier MessageIdentifier
        {
            get { return MessageIdentifier.ConnectionStatusNotification; }
        }
    }
}