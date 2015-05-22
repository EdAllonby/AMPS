using System.Net.Sockets;
using Shared.Message;
using Shared.Message.LoginMessage;

namespace Shared.Serialiser.MessageSerialiser.LoginMessageSerialiser
{
    /// <summary>
    /// Used to serialise and deserialise the <see cref="LoginRequest" /> <see cref="IMessage"/>.
    /// </summary>
    internal sealed class LoginRequestSerialiser : MessageSerialiser<LoginRequest>
    {
        private readonly ISerialisationType serialiser = new BinarySerialiser();

        /// <summary>
        /// Serialise a <see cref="LoginRequest" /> down the wire.
        /// </summary>
        /// <param name="networkStream">The networkStream that connects the Client and Server.</param>
        /// <param name="message">The <see cref="LoginRequest"/> to serialise.</param>
        protected override void Serialise(NetworkStream networkStream, LoginRequest message)
        {
            serialiser.Serialise(networkStream, message);
        }

        /// <summary>
        /// Deserialises a <see cref="LoginRequest" /> from the <see cref="NetworkStream" />.
        /// </summary>
        /// <param name="networkStream">The <see cref="NetworkStream" /> containing the serialised <see cref="LoginRequest" />.</param>
        /// <returns>The deserialised <see cref="LoginRequest" />.</returns>
        public override IMessage Deserialise(NetworkStream networkStream)
        {
            var loginRequest = (LoginRequest) serialiser.Deserialise(networkStream);
            Log.InfoFormat("Network stream has received data and deserialised to a {0} object", loginRequest.MessageIdentifier);
            return loginRequest;
        }
    }
}