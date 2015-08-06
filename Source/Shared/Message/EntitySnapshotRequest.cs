using System;
using Shared.Domain;

namespace Shared.Message
{
    /// <summary>
    /// Request for an entity snapshot where <see cref="T" /> is the <see cref="Entity" /> requested.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public sealed class EntitySnapshotRequest<T> : IMessage where T : Entity
    {
        private readonly SnapshotRequestMessageIdentifierFactory snapshotRequestMessageIdentifierFactory = new SnapshotRequestMessageIdentifierFactory();

        /// <summary>
        /// Create a new request for an <see cref="Entity" /> snapshot.
        /// </summary>
        /// <param name="userId">The <see cref="User" /> requesting the snapshot.</param>
        public EntitySnapshotRequest(int userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// The <see cref="User" /> requesting the snapshot.
        /// </summary>
        public int UserId { get; private set; }

        /// <summary>
        /// The identifier associated with the particular <see cref="IMessage" />.
        /// </summary>
        public MessageIdentifier MessageIdentifier => snapshotRequestMessageIdentifierFactory.GetIdentifierBySnapshotType(typeof (T));
    }
}