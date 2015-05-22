using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using Shared.Message;

namespace Shared.Serialiser.MessageSerialiser
{
    /// <summary>
    /// Used to serialise and deserialise the <see cref="ClientDisconnection" /> <see cref="IMessage"/>.
    /// </summary>
    public sealed class ClientDisconnectionSerialiser : MessageSerialiser<ClientDisconnection>
    {
        private readonly BinaryFormatter binaryFormatter = new BinaryFormatter();

        /// <summary>
        /// Serialise a <see cref="ClientDisconnection" /> down the wire.
        /// </summary>
        /// <param name="networkStream">The NetworkStream that connects the Client and Server.</param>
        /// <param name="message">The <see cref="ClientDisconnection"/> to serialise.</param>
        protected override void Serialise(NetworkStream networkStream, ClientDisconnection message)
        {
            binaryFormatter.Serialize(networkStream, message);
        }

        /// <summary>
        /// Deserialises a <see cref="ClientDisconnection" /> from the <see cref="NetworkStream" />.
        /// </summary>
        /// <param name="networkStream">The <see cref="NetworkStream" /> containing the serialised <see cref="ClientDisconnection" />.</param>
        /// <returns>The deserialised <see cref="ClientDisconnection" />.</returns>
        public override IMessage Deserialise(NetworkStream networkStream)
        {
            var clientDisconnection = (ClientDisconnection) binaryFormatter.Deserialize(networkStream);
            Log.InfoFormat("{0} message deserialised", clientDisconnection.MessageIdentifier);
            return clientDisconnection;
        }
    }
}