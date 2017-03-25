using System.Collections.Generic;
using System.Linq;
using Shared.Domain;

namespace Shared.Repository
{
    /// <summary>
    /// Models a <see cref="Repository" /> for a <see cref="Jam" />.
    /// </summary>
    public sealed class JamRepository : EntityRepository<Jam>
    {
        /// <summary>
        /// Gets all of the <see cref="Jam" />s that a <see cref="Band" /> has had.
        /// </summary>
        /// <param name="bandId">The <see cref="Band" /> Id to get all <see cref="Jam" />s.</param>
        /// <returns>The <see cref="Jam" />s a <see cref="Band" /> has had.</returns>
        public IEnumerable<Jam> GetAllJamsInBand(int bandId)
        {
            return new List<Jam>(GetAllEntities().Where(jam => jam.Band.Id == bandId));
        }

        /// <summary>
        /// Gets the active <see cref="Jam" /> in a <see cref="Band" />.
        /// </summary>
        /// <param name="bandId">The <see cref="Band" />'s Id to check for current active Jam. </param>
        /// <returns>
        /// Gets the current <see cref="Jam" /> for the <see cref="Band" />. If there is no current active
        /// <see cref="Jam" /> then return null.
        /// </returns>
        public Jam GetCurrentActiveJamInBand(int bandId)
        {
            IEnumerable<Jam> jamsInBand = GetAllEntities();

            return jamsInBand.Where(jam => jam.Band.Id == bandId).FirstOrDefault(jam => jam.IsActive);
        }
    }
}