using System.Collections.Generic;
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
        /// Gets all <see cref="Participation" /> objects that match the Band Id.
        /// </summary>
        /// <param name="bandId">The Id of the Band all returning <see cref="Participation" /> objects should have.</param>
        /// <returns>The <see cref="Participation" /> objects that match the Band Id.</returns>
        public List<Participation> GetParticipationsByBandId(int bandId)
        {
            return GetAllEntities().Where(participation => participation.BandId == bandId).ToList();
        }

        /// <summary>
        /// Checks whether a <see cref="Band" /> exists with a group of participants.
        /// </summary>
        /// <param name="userIds">The group of participants to check if a <see cref="Band" /> exists for.</param>
        /// <returns>Whether or not a <see cref="Band" /> exists with the group of participants.</returns>
        public bool DoesBandWithUsersExist(IEnumerable<int> userIds)
        {
            Dictionary<int, List<int>> userIdsIndexedByBandId = GetUserIdsIndexedByBandId();

            return userIdsIndexedByBandId.Select(ids => ids.Value.AreSetsEqual(userIds)).Any(isBand => isBand);
        }

        private Dictionary<int, List<int>> GetUserIdsIndexedByBandId()
        {
            var idsIndexedByBandId = new Dictionary<int, List<int>>();

            foreach (Participation participation in GetAllEntities())
            {
                if (!idsIndexedByBandId.ContainsKey(participation.Band.Id))
                {
                    idsIndexedByBandId[participation.Band.Id] = new List<int>();
                }

                idsIndexedByBandId[participation.Band.Id].Add(participation.User.Id);
            }

            return idsIndexedByBandId;
        }
    }
}