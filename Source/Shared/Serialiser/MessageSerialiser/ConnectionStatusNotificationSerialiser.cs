using System.Net.Sockets;
using Shared.Message;
using Shared.Serialiser.EntitySerialiser;

namespace Shared.Serialiser.MessageSerialiser
{
    /// <summary>
    /// Serialises and deserialises <see cref="ConnectionStatusNotification" />.
    /// </summary>
    public sealed class ConnectionStatusNotificationSerialiser : MessageSerialiser<ConnectionStatusNotification>
    {
        private readonly ConnectionStatusSerialiser connectionStatusSerialiser = new ConnectionStatusSerialiser();
        private readonly NotificationTypeSerialiser notificationTypeSerialiser = new NotificationTypeSerialiser();

        /// <summary>
        /// Serialise a <see cref="ConnectionStatusNotification" /> down the wire.
        /// </summary>
        /// <param name="networkStream">The networkStream that connects the Client and Server.</param>
        /// <param name="message">The <see cref="ConnectionStatusNotification" /> to serialise.</param>
        protected override void Serialise(NetworkStream networkStream, ConnectionStatusNotification message)
        {
            notificationTypeSerialiser.Serialise(networkStream, message.NotificationType);
            connectionStatusSerialiser.Serialise(networkStream, message.ConnectionStatus);
        }

        /// <summary>
        /// Deserialises a <see cref="ConnectionStatusNotification" /> from the <see cref="NetworkStream" />.
        /// </summary>
        /// <param name="networkStream">The <see cref="NetworkStream" /> containing the serialised <see cref="ConnectionStatusNotification" />.</param>
        /// <returns>The deserialised <see cref="ConnectionStatusNotification" />.</returns>
        public override IMessage Deserialise(NetworkStream networkStream)
        {
            NotificationType notificationType = notificationTypeSerialiser.Deserialise(networkStream);
            var connectionStatusNotification = new ConnectionStatusNotification(connectionStatusSerialiser.Deserialise(networkStream), notificationType);

            Log.InfoFormat("{0} message deserialised.", connectionStatusNotification.MessageIdentifier);

            return connectionStatusNotification;
        }
    }
}