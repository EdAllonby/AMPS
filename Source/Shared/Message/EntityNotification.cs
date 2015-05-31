using System.Diagnostics.Contracts;
using Shared.Domain;

namespace Shared.Message
{
    /// <summary>
    /// Packages an <see cref="Entity" /> with a <see cref="NotificationType" />.
    /// Used to notify clients when an <see cref="Entity" /> of type <see cref="T" /> is changed Server side.
    /// </summary>
    /// <typeparam name="T">The <see cref="Entity" /> changed.</typeparam>
    public sealed class EntityNotification<T> : IMessage where T : Entity
    {
        private readonly NotificationMessageIdentifierFactory notificationMessageIdentifierFactory = new NotificationMessageIdentifierFactory();

        /// <summary>
        /// Create a new notification object for <see cref="T" />.
        /// </summary>
        /// <param name="entity">The <see cref="T" /> changed.</param>
        /// <param name="notificationType">What change the <see cref="T" /> <see cref="Entity" /> made.</param>
        public EntityNotification(T entity, NotificationType notificationType)
        {
            Contract.Requires(entity != null);
            Contract.Requires(!entity.IsNew);

            Entity = entity;
            NotificationType = notificationType;
        }

        /// <summary>
        /// What change the <see cref="T" /> <see cref="Entity" /> made.
        /// </summary>
        public NotificationType NotificationType { get; private set; }

        /// <summary>
        /// The <see cref="T" /> <see cref="Entity" /> changed.
        /// </summary>
        public T Entity { get; private set; }

        /// <summary>
        /// What the message identifier is.
        /// </summary>
        public MessageIdentifier MessageIdentifier
        {
            get { return notificationMessageIdentifierFactory.GetIdentifierBySnapshotType(typeof (T)); }
        }
    }
}