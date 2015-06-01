using System.Diagnostics.Contracts;
using System.Net.Sockets;
using log4net;
using Shared.Domain;

namespace Shared.Serialiser.EntitySerialiser
{
    /// <summary>
    /// Used to serialise and deserialise the <see cref="ConnectionStatus" />.
    /// </summary>
    public sealed class ConnectionStatusSerialiser
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (ConnectionStatusSerialiser));
        private readonly ISerialiser serialiser = new BinarySerialiser();

        /// <summary>
        /// Serialises a <see cref="ConnectionStatus" /> through the <see cref="NetworkStream" />.
        /// </summary>
        /// <param name="networkStream">The <see cref="NetworkStream" /> to serialise the <see cref="ConnectionStatus" /> across.</param>
        /// <param name="connectionStatus">The <see cref="ConnectionStatus" /> to serialise.</param>
        public void Serialise(NetworkStream networkStream, ConnectionStatus connectionStatus)
        {
            Contract.Requires(networkStream != null);
            Contract.Requires(connectionStatus != null);

            serialiser.Serialise(networkStream, connectionStatus);
        }

        /// <summary>
        /// Deserialises a <see cref="ConnectionStatus" /> from the <see cref="NetworkStream" />.
        /// </summary>
        /// <param name="networkStream">The <see cref="NetworkStream" /> containing the serialised <see cref="ConnectionStatus" />.</param>
        /// <returns>The deserialised <see cref="ConnectionStatus" />.</returns>
        public ConnectionStatus Deserialise(NetworkStream networkStream)
        {
            Contract.Requires(networkStream != null);

            var connectionStatus = (ConnectionStatus) serialiser.Deserialise(networkStream);

            Log.Debug("Network stream has received data and deserialised to an ConnectionStatus object.");
            return connectionStatus;
        }
    }
}