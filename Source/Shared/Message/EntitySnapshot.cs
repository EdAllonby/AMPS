using System;
using System.Collections.Generic;
using Shared.Domain;

namespace Shared.Message
{
    /// <summary>
    /// Packages a collection of <see cref="Entity" />s for the client to update their repository at startup.
    /// </summary>
    /// <typeparam name="TEntity">The snapshot <see cref="Entity" />. </typeparam>
    [Serializable]
    public sealed class EntitySnapshot<TEntity> : IMessage where TEntity : Entity
    {
        private readonly SnapshotMessageIdentifierFactory snapshotMessageIdentifierFactory = new();

        /// <summary>
        /// Create a snapshot containing <see cref="Entity" />s of type <see cref="TEntity" />.
        /// </summary>
        /// <param name="entities">the entities to packages as a snapshot.</param>
        public EntitySnapshot(IEnumerable<TEntity> entities)
        {
            Entities = entities;
        }

        /// <summary>
        /// The entities packages as a Snapshot.
        /// </summary>
        public IEnumerable<TEntity> Entities { get; }

        /// <summary>
        /// The type of Snapshot.
        /// </summary>
        public MessageIdentifier MessageIdentifier => snapshotMessageIdentifierFactory.GetIdentifierBySnapshotType(typeof(TEntity));
    }
}