using JetBrains.Annotations;
using Shared.Domain;

namespace Shared.Repository
{
    /// <summary>
    /// Models a <see cref="Repository" /> for a <see cref="Band" />.
    /// </summary>
    public sealed class BandRepository : EntityRepository<Band>
    {
        /// <summary>
        /// Intialises the repository with any <see cref="Band" /> entities it finds from the data layer.
        /// </summary>
        /// <param name="entityPersister">The <see cref="Band" /> persister to use.</param>
        public BandRepository([NotNull] IEntityPersister<Band> entityPersister) : base(entityPersister)
        {
        }
    }
}