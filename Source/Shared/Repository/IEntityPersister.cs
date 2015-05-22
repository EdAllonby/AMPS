using System.Collections.Generic;
using Shared.Domain;

namespace Shared.Repository
{
    /// <summary>
    /// Persists an <see cref="Entity" />.
    /// </summary>
    /// <typeparam name="T">The <see cref="Entity" />  to persist.</typeparam>
    public interface IEntityPersister<T> where T : Entity
    {
        /// <summary>
        /// Gets an <see cref="Entity" /> from the Database by its Id.
        /// </summary>
        /// <param name="id">The Id of the <see cref="Entity" /> to get.</param>
        /// <returns>The <see cref="Entity" /> from the persister.</returns>
        T GetEntityById(int id);

        /// <summary>
        /// Inserts an <see cref="Entity" />.
        /// </summary>
        /// <param name="entity">The <see cref="Entity" /> to insert.</param>
        /// <returns>If the insert was successful.</returns>
        bool InsertEntity(T entity);

        /// <summary>
        /// Updates an <see cref="Entity" />.
        /// </summary>
        /// <param name="entity">The updated <see cref="Entity" />.</param>
        /// <returns>If the update was successful.</returns>
        bool UpdateEntity(T entity);

        /// <summary>
        /// Deletes an <see cref="Entity" />.
        /// </summary>
        /// <param name="entityId">The <see cref="Entity" /> to delete.</param>
        /// <returns>If the delete was successful.</returns>
        bool DeleteEntity(int entityId);

        /// <summary>
        /// Gets all <see cref="Entity" /> objects from the map.
        /// </summary>
        /// <returns>All <see cref="Entity" /> objects from the map.</returns>
        IEnumerable<T> GetAllEntities();

        /// <summary>
        /// Deletes all entities from the persister.
        /// </summary>
        /// <returns>If the delete was successful.</returns>
        bool DeleteAllEntities();
    }
}