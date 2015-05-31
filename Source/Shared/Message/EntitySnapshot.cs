using System;
using System.Collections.Generic;
using Shared.Domain;

namespace Shared.Message
{
    /// <summary>
    /// Packages a collection of <see cref="Entity" />s for the client to update their repository at startup.
    /// </summary>
    /// <typeparam name="T">The snapshot <see cref="Entity" />. </typeparam>
    [Serializable]
    public sealed class EntitySnapshot<T> : IMessage where T : Entity
    {
        private readonly SnapshotMessageIdentifierFactory snapshotMessageIdentifierFactory = new SnapshotMessageIdentifierFactory();

        /// <summary>
        /// Create a snapshot containing <see cref="Entity" />s of type <see cref="T" />.
        /// </summary>
        /// <param name="entities">the entities to packages as a snapshot.</param>
        public EntitySnapshot(IEnumerable<T> entities)
        {
            Entities = entities;
        }

        /// <summary>
        /// The entities packages as a Snapshot.
        /// </summary>
        public IEnumerable<T> Entities { get; private set; }

        /// <summary>
        /// The type of Snapshot.
        /// </summary>
        public MessageIdentifier MessageIdentifier
        {
            get { return snapshotMessageIdentifierFactory.GetIdentifierBySnapshotType(typeof (T)); }
        }
    }
}