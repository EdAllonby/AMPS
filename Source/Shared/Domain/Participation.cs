using System;

namespace Shared.Domain
{
    /// <summary>
    /// The relationship between a <see cref="User" /> and a <see cref="Band" />.
    /// </summary>
    [Serializable]
    public class Participation : Entity
    {
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
            UserId = userId;
            BandId = bandId;
            IsLeader = isLeader;
        }

        /// <summary>
        /// Create an incomplete Participation entity without an Id.
        /// </summary>
        /// <param name="userId">The identity of the user to link to a Band.</param>
        /// <param name="bandId">The identity of the Band that the user wants to link to.</param>
        /// <param name="isLeader">Defines if the participant is the leader of the <see cref="Band" />.</param>
        public Participation(int userId, int bandId, bool isLeader)
        {
            UserId = userId;
            BandId = bandId;
            IsLeader = isLeader;
        }

        /// <summary>
        /// Is this Participant a leader of the <see cref="Band" />?
        /// </summary>
        public bool IsLeader { get; }

        /// <summary>
        /// The participants <see cref="User" /> Id.
        /// </summary>
        private int UserId { get; }

        /// <summary>
        /// The <see cref="Band" /> Id this <see cref="User" /> is participating in.
        /// </summary>
        public int BandId { get; }

        public User User => RepositoryManager.GetRepository<User>().FindEntityById(UserId);

        public Band Band => RepositoryManager.GetRepository<Band>().FindEntityById(BandId);
    }
}