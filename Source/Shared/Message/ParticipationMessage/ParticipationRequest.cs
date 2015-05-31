using System;
using System.Diagnostics.Contracts;
using Shared.Domain;

namespace Shared.Message.ParticipationMessage
{
    /// <summary>
    /// Packages <see cref="Participation" />s related by Jam Id for the Server to send to the Client
    /// </summary>
    [Serializable]
    public sealed class ParticipationRequest : IMessage
    {
        /// <summary>
        /// A new <see cref="Participation" /> request.
        /// </summary>
        /// <param name="participation">The <see cref="Participation" /> to request to create.</param>
        public ParticipationRequest(Participation participation)
        {
            Contract.Requires(participation != null);

            Participation = participation;
        }

        /// <summary>
        /// The <see cref="Participation" /> to request to create.
        /// </summary>
        public Participation Participation { get; private set; }

        public MessageIdentifier MessageIdentifier
        {
            get { return MessageIdentifier.ParticipationRequest; }
        }
    }
}