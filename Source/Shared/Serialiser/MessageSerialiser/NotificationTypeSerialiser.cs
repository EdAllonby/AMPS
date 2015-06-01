using System.Diagnostics.Contracts;
using System.Net.Sockets;
using log4net;
using Shared.Message;

namespace Shared.Serialiser.MessageSerialiser
{
    /// <summary>
    /// Used to serialise and deserialise the <see cref="NotificationType" />.
    /// </summary>
    internal sealed class NotificationTypeSerialiser
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (NotificationType));
        private readonly ISerialiser serialiser = new BinarySerialiser();

        /// <summary>
        /// Serialise a <see cref="NotificationType" /> down the wire.
        /// </summary>
        /// <param name="networkStream">The networkStream that connects the Client and Server.</param>
        /// <param name="notificationType">The <see cref="NotificationType" /> to serialise.</param>
        public void Serialise(NetworkStream networkStream, NotificationType notificationType)
        {
            Contract.Requires(networkStream != null);

            serialiser.Serialise(networkStream, notificationType);

            Log.DebugFormat("Sent Message NotificationType: {0} to stream", notificationType);
        }

        /// <summary>
        /// Deserialises a <see cref="NotificationType" /> from the <see cref="NetworkStream" />.
        /// </summary>
        /// <param name="networkStream">The <see cref="NetworkStream" /> containing the serialised <see cref="NotificationType" />.</param>
        /// <returns>The deserialised <see cref="NotificationType" />.</returns>
        public NotificationType Deserialise(NetworkStream networkStream)
        {
            Contract.Requires(networkStream != null);

            var notificationType = (NotificationType) serialiser.Deserialise(networkStream);
            return notificationType;
        }
    }
}