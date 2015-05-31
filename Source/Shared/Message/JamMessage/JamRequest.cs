using System;
using System.Collections.Generic;
using Shared.Domain;

namespace Shared.Message.JamMessage
{
    /// <summary>
    /// Packages a List of <see cref="Task" /> Ids that should be part of a new Jam and links it to a Band.
    /// </summary>
    [Serializable]
    public sealed class JamRequest : IMessage
    {
        private readonly DateTime jamEndDate;

        /// <summary>
        /// Creates a new <see cref="Jam" /> Request.
        /// </summary>
        /// <param name="bandId">The <see cref="Band" /> to hold a new <see cref="Jam" />.</param>
        /// <param name="taskIds">The <see cref="Task" />s wanted in the <see cref="Jam" />.</param>
        /// <param name="jamEndDate">The requested end date of the <see cref="Jam" />.</param>
        public JamRequest(int bandId, List<int> taskIds, DateTime jamEndDate)
        {
            this.jamEndDate = jamEndDate;
            BandId = bandId;
            TaskIds = taskIds;
        }

        /// <summary>
        /// The <see cref="Task" />s to include in the new <see cref="Jam" />.
        /// </summary>
        public List<int> TaskIds { get; private set; }

        /// <summary>
        /// The <see cref="Band" /> who holds the <see cref="Jam" /> session.
        /// </summary>
        public int BandId { get; private set; }

        /// <summary>
        /// The requested end date of the <see cref="Jam" />.
        /// </summary>
        public DateTime JamEndDate
        {
            get { return jamEndDate; }
        }

        /// <summary>
        /// The message identifier for this <see cref="IMessage" /> request.
        /// </summary>
        public MessageIdentifier MessageIdentifier
        {
            get { return MessageIdentifier.JamRequest; }
        }
    }
}