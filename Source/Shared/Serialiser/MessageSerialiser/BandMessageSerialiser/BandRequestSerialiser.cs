using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using Shared.Message;
using Shared.Message.BandMessage;

namespace Shared.Serialiser.MessageSerialiser.BandMessageSerialiser
{
    /// <summary>
    /// Used to serialise and deserialise the <see cref="BandRequest" /> <see cref="IMessage"/>.
    /// </summary>
    internal sealed class BandRequestSerialiser : MessageSerialiser<BandRequest>
    {
        private readonly BinaryFormatter binaryFormatter = new BinaryFormatter();

        /// <summary>
        /// Serialise a <see cref="BandRequest" /> down the wire.
        /// </summary>
        /// <param name="networkStream">The networkStream that connects the Client and Server.</param>
        /// <param name="message">The <see cref="BandRequest"/> to serialise.</param>
        protected override void Serialise(NetworkStream networkStream, BandRequest message)
        {
            binaryFormatter.Serialize(networkStream, message);
        }

        /// <summary>
        /// Deserialises a <see cref="BandRequest" /> from the <see cref="NetworkStream" />.
        /// </summary>
        /// <param name="networkStream">The <see cref="NetworkStream" /> containing the serialised <see cref="BandRequest" />.</param>
        /// <returns>The deserialised <see cref="BandRequest" />.</returns>
        public override IMessage Deserialise(NetworkStream networkStream)
        {
            var bandRequest = (BandRequest) binaryFormatter.Deserialize(networkStream);
            Log.InfoFormat("{0} message deserialised", bandRequest.MessageIdentifier);
            return bandRequest;
        }
    }
}