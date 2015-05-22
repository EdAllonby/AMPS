using System.Net.Sockets;
using Shared.Domain;
using Shared.Message;
using Shared.Message.ParticipationMessage;
using Shared.Serialiser.EntitySerialiser;

namespace Shared.Serialiser.MessageSerialiser.ParticipationMessageSerialiser
{
    /// <summary>
    /// Used to serialise and deserialise the <see cref="ParticipationRequest" /> <see cref="IMessage"/>.
    /// </summary>
    internal sealed class ParticipationRequestSerialiser : MessageSerialiser<ParticipationRequest>
    {
        private readonly EntitySerialiser<Participation> participationSerialiser = new EntitySerialiser<Participation>();

        /// <summary>
        /// Serialise a <see cref="ParticipationRequest" /> down the wire.
        /// </summary>
        /// <param name="networkStream">The networkStream that connects the Client and Server.</param>
        /// <param name="message">The <see cref="ParticipationRequest"/> to serialise.</param>
        protected override void Serialise(NetworkStream networkStream, ParticipationRequest message)
        {
            participationSerialiser.Serialise(networkStream, message.Participation);
        }

        /// <summary>
        /// Deserialises a <see cref="ParticipationRequest" /> from the <see cref="NetworkStream" />.
        /// </summary>
        /// <param name="networkStream">The <see cref="NetworkStream" /> containing the serialised <see cref="ParticipationRequest" />.</param>
        /// <returns>The deserialised <see cref="ParticipationRequest" />.</returns>
        public override IMessage Deserialise(NetworkStream networkStream)
        {
            var participationRequest = new ParticipationRequest(participationSerialiser.Deserialise(networkStream));
            Log.InfoFormat("{0} message deserialised", participationRequest.MessageIdentifier);
            return participationRequest;
        }
    }
}