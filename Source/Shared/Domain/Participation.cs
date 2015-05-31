using System;
using System.Diagnostics.Contracts;

namespace Shared.Domain
{
    /// <summary>
    /// The relationship between a <see cref="User" /> and a <see cref="Band" />.
    /// </summary>
    [Serializable]
    public class Participation : Entity
    {
        private readonly int bandId;
        private readonly bool isLeader;
        private readonly int userId;

        /// <summary>
        /// Creates a new participation entity.
        /// </summary>
        /// <param name="id">The identity of this participation <see cref="Entity" />.</param>
        /// <param name="userId">The identity of the user to link to a <see cref="Band" />.</param>
        /// <param name="bandId">The identity of the <see cref="Band" /> that the user wants to link to.</param>
        /// <param name="isLeader">Defines if the participant is the leader of the <see cref="Band" />.</param>
        public Participation(int id, int userId, int bandId, bool isLeader)
            : base(id)
        {
            Contract.Requires(id > 0);
            Contract.Requires(userId > 0);
            Contract.Requires(bandId > 0);

            this.userId = userId;
            this.bandId = bandId;
            this.isLeader = isLeader;
        }

        /// <summary>
        /// Create an incomplete Participation entity without an Id.
        /// </summary>
        /// <param name="userId">The identity of the user to link to a Band.</param>
        /// <param name="bandId">The identity of the Band that the user wants to link to.</param>
        /// <param name="isLeader">Defines if the participant is the leader of the <see cref="Band" />.</param>
        public Participation(int userId, int bandId, bool isLeader)
        {
            Contract.Requires(userId > 0);
            Contract.Requires(bandId > 0);

            this.userId = userId;
            this.bandId = bandId;
            this.isLeader = isLeader;
        }

        /// <summary>
        /// Is this Participant a leader of the <see cref="Band" />?
        /// </summary>
        public bool IsLeader
        {
            get { return isLeader; }
        }

        /// <summary>
        /// The participants <see cref="User" /> Id.
        /// </summary>
        public int UserId
        {
            get { return userId; }
        }

        /// <summary>
        /// The <see cref="Band" /> Id this <see cref="User" /> is participating in.
        /// </summary>
        public int BandId
        {
            get { return bandId; }
        }
    }
}