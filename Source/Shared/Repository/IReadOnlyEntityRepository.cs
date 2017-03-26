using System;
using System.Collections.Generic;
using Shared.Domain;

namespace Shared.Repository
{
    /// <summary>
    /// Exposes only the read only functionality of an <see cref="Entity" /> Repository.
    /// </summary>
    /// <typeparam name="T">The entity type that this readonly repository holds.</typeparam>
    public interface IReadOnlyEntityRepository<T> where T : Entity
    {
        /// <summary>
        /// Gets raised when an entity in the repository has been added.
        /// </summary>
        event EventHandler<EntityChangedEventArgs<T>> EntityAdded;

        /// <summary>
        /// Gets raised when an entity in the repository has been updated.
        /// </summary>
        event EventHandler<EntityChangedEventArgs<T>> EntityUpdated;

        /// <summary>
        /// Gets raised when an entity in the repository has been removed.
        /// </summary>
        event EventHandler<EntityChangedEventArgs<T>> EntityRemoved;

        /// <summary>
        /// Retrieves an <see cref="Entity" /> entity from the repository.
        /// </summary>
        /// <param name="entityId">The <see cref="Entity" /> entity Id to find.</param>
        /// <returns>The <see cref="Entity" /> which matches the ID. If no <see cref="Entity" /> is found, return null.</returns>
        T FindEntityById(int entityId);

        IEnumerable<T> GetAllEntities();
    }
}