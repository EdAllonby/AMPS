using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Shared.Domain;

namespace Shared.Repository
{
    /// <summary>
    /// Models a <see cref="Repository" /> for a <see cref="Participation" />.
    /// </summary>
    public sealed class ParticipationRepository : EntityRepository<Participation>
    {
        /// <summary>
        /// Intialises the repository with any <see cref="Participation" /> entities it finds from the data layer.
        /// </summary>
        /// <param name="entityPersister">The <see cref="Participation" /> entity-to-table mapper to use.</param>
        public ParticipationRepository(IEntityPersister<Participation> entityPersister) : base(entityPersister)
        {
            Contract.Requires(entityPersister != null);
        }

        /// <summary>
        /// Gets all <see cref="Participation" /> objects that match the Band Id.
        /// </summary>
        /// <param name="bandId">The Id of the Band all returning <see cref="Participation" /> objects should have.</param>
        /// <returns>The <see cref="Participation" /> objects that match the Band Id.</returns>
        public List<Participation> GetParticipationsByBandId(int bandId)
        {
            return GetAllEntities().Where(participation => participation.BandId == bandId).ToList();
        }

        /// <summary>
        /// Gets a collection of <see cref="Band" /> Ids which a <see cref="User" /> participates in.
        /// </summary>
        /// <param name="userId">The <see cref="User" /> Id to find <see cref="Band" />s for.</param>
        /// <returns></returns>
        public IEnumerable<int> GetAllBandIdsByUserId(int userId)
        {
            return from participation in GetAllEntities()
                where participation.UserId.Equals(userId)
                select participation.BandId;
        }

        /// <summary>
        /// Checks whether a <see cref="Band" /> exists with a group of participants.
        /// </summary>
        /// <param name="userIds">The group of participants to check if a <see cref="Band" /> exists for.</param>
        /// <returns>Whether or not a <see cref="Band" /> exists with the group of participants.</returns>
        [Pure]
        public bool DoesBandWithUsersExist(IEnumerable<int> userIds)
        {
            var userIdsIndexedByBandId = GetUserIdsIndexedByBandId();

            return userIdsIndexedByBandId.Select(ids => ids.Value.HasSameElementsAs(userIds)).Any(isBand => isBand);
        }

        /// <summary>
        /// Finds whether the the <see cref="User" /> in the <see cref="Band" /> is the leader.
        /// </summary>
        /// <param name="userId">The <see cref="User" /> Id in the <see cref="Band" />.</param>
        /// <param name="bandId">The <see cref="Band" /> to check.</param>
        /// <returns>Whether the user is a leader in a <see cref="Band" />.</returns>
        public bool IsParticipantLeaderOfBand(int userId, int bandId)
        {
            Participation participation = GetParticipationByUserIdAndBandId(userId, bandId);

            return participation != null && participation.IsLeader;
        }

        /// <summary>
        /// Finds the specific Participation object that matches the userId and bandId.
        /// </summary>
        /// <param name="userId">The Id of the User to match.</param>
        /// <param name="bandId">The Id of the Band to match.</param>
        /// <returns>The <see cref="Participation" /> that matches the user Id and Jam Id.</returns>
        private Participation GetParticipationByUserIdAndBandId(int userId, int bandId)
        {
            foreach (var possibleParticipation in GetAllEntities())
            {
                if (possibleParticipation.UserId.Equals(userId) && possibleParticipation.BandId.Equals(bandId))
                {
                    return possibleParticipation;
                }
            }

            return null;
        }

        private Dictionary<int, List<int>> GetUserIdsIndexedByBandId()
        {
            var idsIndexedByBandId = new Dictionary<int, List<int>>();

            foreach (var participation in GetAllEntities())
            {
                if (!idsIndexedByBandId.ContainsKey(participation.BandId))
                {
                    idsIndexedByBandId[participation.BandId] = new List<int>();
                }

                idsIndexedByBandId[participation.BandId].Add(participation.UserId);
            }

            return idsIndexedByBandId;
        }
    }
}