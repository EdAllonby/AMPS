using System;
using Shared.Domain;

namespace Shared.Repository
{
    /// <summary>
    /// Non-generic entity repository <see langword="interface" />.
    /// </summary>
    public interface IEntityRepository
    {
        IEntityPersister EntityPersister { get; set; }

        /// <summary>
        /// The owning manager.
        /// </summary>
        RepositoryManager RepositoryManager { set; }

        /// <summary>
        /// Gets the <see cref="Entity" /> type that is held in the repository.
        /// </summary>
        Type EnclosedEntityType { get; }


        /// <summary>
        /// Delete an entity from the repository.
        /// </summary>
        /// <param name="entityId">The <see cref="Entity" /> Id to delete.</param>
        /// <returns>If the delete was successful.</returns>
        void DeleteEntity(int entityId);

        /// <summary>
        /// Remove all entities from the repository.
        /// </summary>
        void DeleteAll();
    }

    /// <summary>
    /// Generic <see cref="Entity" /> <see cref="Repository" /> <see langword="iinterface" />.
    /// </summary>
    /// <typeparam name="T">The <see cref="Entity" /> type.</typeparam>
    public interface IEntityRepository<T> : IReadOnlyEntityRepository<T>, IEntityRepository where T : Entity
    {
        /// <summary>
        /// Adds an <see cref="Entity" /> to the repository.
        /// </summary>
        /// <param name="entity">The <see cref="Entity" /> to add.</param>
        void AddEntity(T entity);

        /// <summary>
        /// Updates an <see cref="Entity" /> in the repository.
        /// </summary>
        /// <param name="entity">The <see cref="Entity" /> to update. Uses its Id as the comparer.</param>
        void UpdateEntity(T entity);
    }
}