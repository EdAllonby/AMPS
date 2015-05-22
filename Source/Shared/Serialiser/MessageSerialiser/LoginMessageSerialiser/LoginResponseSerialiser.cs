using System.Net.Sockets;
using Shared.Message;
using Shared.Message.LoginMessage;

namespace Shared.Serialiser.MessageSerialiser.LoginMessageSerialiser
{
    /// <summary>
    /// Used to serialise and deserialise the <see cref="LoginResponse" /> <see cref="IMessage"/>.
    /// </summary>
    internal sealed class LoginResponseSerialiser : MessageSerialiser<LoginResponse>
    {
        private readonly ISerialisationType serialiser = new BinarySerialiser();

        /// <summary>
        /// Serialise a <see cref="LoginResponse" /> down the wire.
        /// </summary>
        /// <param name="networkStream">The networkStream that connects the Client and Server.</param>
        /// <param name="message">The <see cref="LoginResponse"/> to serialise.</param>
        protected override void Serialise(NetworkStream networkStream, LoginResponse message)
        {
            serialiser.Serialise(networkStream, message);
        }

        /// <summary>
        /// Deserialises a <see cref="LoginResponse" /> from the <see cref="NetworkStream" />.
        /// </summary>
        /// <param name="networkStream">The <see cref="NetworkStream" /> containing the serialised <see cref="LoginResponse" />.</param>
        /// <returns>The deserialised <see cref="LoginResponse" />.</returns>
        public override IMessage Deserialise(NetworkStream networkStream)
        {
            var loginResponse = (LoginResponse) serialiser.Deserialise(networkStream);
            Log.InfoFormat("Network stream has received data and deserialised to a {0} object", loginResponse.MessageIdentifier);
            return loginResponse;
        }
    }
}