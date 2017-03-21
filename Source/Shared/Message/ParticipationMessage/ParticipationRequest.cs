using System;
using JetBrains.Annotations;
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
        public ParticipationRequest([NotNull] Participation participation)
        {
            Participation = participation;
        }

        /// <summary>
        /// The <see cref="Participation" /> to request to create.
        /// </summary>
        public Participation Participation { get; }

        public MessageIdentifier MessageIdentifier => MessageIdentifier.ParticipationRequest;
    }
}