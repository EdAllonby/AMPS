using System.Collections.Concurrent;
using System.Collections.Generic;
using Shared.Domain;

namespace Shared.Repository
{
    /// <summary>
    /// Persist entities in memory.
    /// </summary>
    /// <typeparam name="T">The <see cref="Entity" /> to persist in memory/</typeparam>
    public sealed class InMemoryEntityPersister<T> : IEntityPersister<T> where T : Entity
    {
        private readonly IDictionary<int, T> entitiesIndexedById = new ConcurrentDictionary<int, T>();

        /// <summary>
        /// Gets an <see cref="Entity" /> from the Database by its Id.
        /// </summary>
        /// <param name="id">The Id of the <see cref="Entity" /> to get.</param>
        /// <returns>The <see cref="Entity" /> from the persister.</returns>
        public T GetEntityById(int id)
        {
            T entity;

            entitiesIndexedById.TryGetValue(id, out entity);

            return entity;
        }

        /// <summary>
        /// Inserts an <see cref="Entity" />.
        /// </summary>
        /// <param name="entity">The <see cref="Entity" /> to insert.</param>
        /// <returns>If the insert was successful.</returns>
        public bool InsertEntity(T entity)
        {
            entitiesIndexedById.Add(entity.Id, entity);
            return true;
        }

        /// <summary>
        /// Updates an <see cref="Entity" />.
        /// </summary>
        /// <param name="entity">The updated <see cref="Entity" />.</param>
        /// <returns>If the update was successful.</returns>
        public bool UpdateEntity(T entity)
        {
            entitiesIndexedById[entity.Id] = entity;

            return true;
        }

        /// <summary>
        /// Deletes an <see cref="Entity" />.
        /// </summary>
        /// <param name="entityId">The <see cref="Entity" /> to delete.</param>
        /// <returns>If the delete was successful.</returns>
        public bool DeleteEntity(int entityId)
        {
            entitiesIndexedById.Remove(entityId);

            return true;
        }

        /// <summary>
        /// Deletes all entities from the persister.
        /// </summary>
        /// <returns></returns>
        public bool DeleteAllEntities()
        {
            foreach (int entityId in entitiesIndexedById.Keys)
            {
                DeleteEntity(entityId);
            }

            return true;
        }

        /// <summary>
        /// Gets all <see cref="Entity" /> objects from the map.
        /// </summary>
        /// <returns>All <see cref="Entity" /> objects from the map.</returns>
        public IEnumerable<T> GetAllEntities()
        {
            return new List<T>(entitiesIndexedById.Values);
        }
    }
}