using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Shared.Domain;

namespace Shared.Message.BandMessage
{
    /// <summary>
    /// Packages a List of user Ids that should be part of a new Band. A <see cref="Band" /> must have a leader.
    /// </summary>
    [Serializable]
    public sealed class BandRequest : IMessage
    {
        private readonly int leaderId;
        private readonly string name;

        /// <summary>
        /// Defines a band request with a set of members.
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="name"></param>
        /// <param name="leaderId"></param>
        public BandRequest(List<int> userIds, string name, int leaderId)
        {
            Contract.Requires(leaderId > 0);

            UserIds = userIds;
            this.name = name;
            this.leaderId = leaderId;
        }

        /// <summary>
        /// Requested name for new Band.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// The users to add to the Band.
        /// </summary>
        public List<int> UserIds { get; private set; }

        /// <summary>
        /// The leader's Id.
        /// </summary>
        public int LeaderId
        {
            get { return leaderId; }
        }

        /// <summary>
        /// The message identifier for this <see cref="IMessage" /> request.
        /// </summary>
        public MessageIdentifier MessageIdentifier
        {
            get { return MessageIdentifier.BandRequest; }
        }
    }
}