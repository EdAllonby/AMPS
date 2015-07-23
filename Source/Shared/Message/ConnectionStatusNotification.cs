using Shared.Domain;

namespace Shared.Message
{
    /// <summary>
    /// Signifies when a <see cref="User" />'s <see cref="ConnectionStatus" /> has changed.
    /// </summary>
    public sealed class ConnectionStatusNotification : IMessage
    {
        /// <summary>
        /// Create a new <see cref="ConnectionStatus" /> notification <see cref="IMessage" />.
        /// </summary>
        /// <param name="connectionStatus">The new <see cref="User" /> <see cref="ConnectionStatus" />.</param>
        /// <param name="notificationType">The entity modification to perform.</param>
        public ConnectionStatusNotification(ConnectionStatus connectionStatus, NotificationType notificationType)
        {
            this.ConnectionStatus = connectionStatus;
            this.NotificationType = notificationType;
        }

        /// <summary>
        /// The new <see cref="User" /> <see cref="ConnectionStatus" />.
        /// </summary>
        public ConnectionStatus ConnectionStatus { get; }

        /// <summary>
        /// The entity modification to perform.
        /// </summary>
        public NotificationType NotificationType { get; }

        /// <summary>
        /// The identifier for this message notification type..
        /// </summary>
        public MessageIdentifier MessageIdentifier
        {
            get { return MessageIdentifier.ConnectionStatusNotification; }
        }
    }
}