using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
        /// Intialises the repository with any <see cref="Jam" /> entities it finds from the data layer.
        /// </summary>
        /// <param name="entityPersister">The <see cref="Jam" /> entity-to-table mapper to use.</param>
        public JamRepository(IEntityPersister<Jam> entityPersister) : base(entityPersister)
        {
            Contract.Requires(entityPersister != null);
        }

        /// <summary>
        /// Gets all of the <see cref="Jam" />s that a <see cref="Band" /> has had.
        /// </summary>
        /// <param name="bandId">The <see cref="Band" /> Id to get all <see cref="Jam" />s.</param>
        /// <returns>The <see cref="Jam" />s a <see cref="Band" /> has had.</returns>
        public IEnumerable<Jam> GetAllJamsInBand(int bandId)
        {
            return new List<Jam>(GetAllEntities().Where(jam => jam.BandId == bandId));
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

            return jamsInBand.Where(jam => jam.BandId == bandId).FirstOrDefault(jam => jam.IsActive);
        }
    }
}