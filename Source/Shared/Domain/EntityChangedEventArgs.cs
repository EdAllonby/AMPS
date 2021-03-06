﻿using System;
using Shared.Message;
using Shared.Repository;

namespace Shared.Domain
{
    /// <summary>
    /// Signifies a entity change in a <see cref="EntityRepository{T}" />.
    /// </summary>
    /// <typeparam name="T">The entity that has been changed in the repository.</typeparam>
    public sealed class EntityChangedEventArgs<T> : EventArgs where T : Entity
    {
        /// <summary>
        /// Signifies either an <see cref="Entity" /> creation or deletion. When an <see cref="Entity" /> is deleted,
        /// <see cref="PreviousEntity" /> is set to the current entity.
        /// </summary>
        /// <remarks>Must not have a <see cref="NotificationType" />.Update object passed.</remarks>
        /// <param name="entity">The entity that is created or deleted.</param>
        /// <param name="notificationType">What is happening to the entity in the repository.</param>
        public EntityChangedEventArgs(T entity, NotificationType notificationType)
        {
            Entity = entity;

            if (notificationType == NotificationType.Delete)
            {
                PreviousEntity = entity;
            }

            NotificationType = notificationType;
        }

        /// <summary>
        /// Signifies an <see cref="Entity" /> being updated in the <see cref="EntityRepository{T}" />.
        /// </summary>
        /// <param name="entity">The entity being updated.</param>
        /// <param name="previousEntity">The entity state before being updated.</param>
        public EntityChangedEventArgs(T entity, T previousEntity)
        {
            Entity = entity;
            PreviousEntity = previousEntity;

            NotificationType = NotificationType.Update;
        }

        /// <summary>
        /// The action being applied to the entity.
        /// </summary>
        public NotificationType NotificationType { get; }

        /// <summary>
        /// The new entity.
        /// </summary>
        public T Entity { get; }

        /// <summary>
        /// The previous state of the entity.
        /// </summary>
        public T PreviousEntity { get; }
    }
}