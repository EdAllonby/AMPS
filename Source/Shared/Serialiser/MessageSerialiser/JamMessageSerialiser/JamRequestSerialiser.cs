using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using Shared.Message;
using Shared.Message.JamMessage;

namespace Shared.Serialiser.MessageSerialiser.JamMessageSerialiser
{
    /// <summary>
    /// Used to serialise and deserialise the <see cref="JamRequest" /> <see cref="IMessage"/>.
    /// </summary>
    internal sealed class JamRequestSerialiser : MessageSerialiser<JamRequest>
    {
        private readonly BinaryFormatter binaryFormatter = new BinaryFormatter();

        /// <summary>
        /// Serialise a <see cref="JamRequest" /> down the wire.
        /// </summary>
        /// <param name="networkStream">The NetworkStream that connects the Client and Server.</param>
        /// <param name="message">The <see cref="JamRequest"/> to serialise.</param>
        protected override void Serialise(NetworkStream networkStream, JamRequest message)
        {
            binaryFormatter.Serialize(networkStream, message);
        }

        /// <summary>
        /// Deserialises a <see cref="JamRequest" /> from the <see cref="NetworkStream" />.
        /// </summary>
        /// <param name="networkStream">The <see cref="NetworkStream" /> containing the serialised <see cref="JamRequest" />.</param>
        /// <returns>The deserialised <see cref="JamRequest" />.</returns>
        public override IMessage Deserialise(NetworkStream networkStream)
        {
            var jamRequest = (JamRequest) binaryFormatter.Deserialize(networkStream);
            Log.InfoFormat("{0} message deserialised", jamRequest.MessageIdentifier);
            return jamRequest;
        }
    }
}