using System;
using System.Collections.Generic;
using Shared.Domain;
using Shared.Repository;

namespace Server.Persistence
{
    /// <summary>
    /// Persist entities in a Database.
    /// </summary>
    /// <typeparam name="T">The <see cref="Entity" /> to persist in a Database.</typeparam>
    public sealed class DatabaseEntityPersister<T> : IEntityPersister<T> where T : Entity
    {
        private readonly EntityMapper<T> entityMapper;

        /// <summary>
        /// Create a new Database persister with an entity mapper implementation.
        /// </summary>
        /// <param name="entityMapper">The entity-to-table mapper implementation to use.</param>
        public DatabaseEntityPersister(EntityMapper<T> entityMapper)
        {
            this.entityMapper = entityMapper;
        }

        /// <summary>
        /// Gets an <see cref="Entity" /> from the Database by its Id.
        /// </summary>
        /// <param name="id">The Id of the <see cref="Entity" /> to get.</param>
        /// <returns>The <see cref="Entity" /> from the Database.</returns>
        public T GetEntityById(int id)
        {
            return entityMapper.GetEntityById(id);
        }

        /// <summary>
        /// Inserts an <see cref="Entity" />.
        /// </summary>
        /// <param name="entity">The <see cref="Entity" /> to insert.</param>
        /// <returns>If the insert was successful.</returns>
        public bool InsertEntity(T entity)
        {
            return entityMapper.InsertEntity(entity);
        }

        /// <summary>
        /// Updates an <see cref="Entity" />.
        /// </summary>
        /// <param name="entity">The updated <see cref="Entity" />.</param>
        /// <returns>If the update was successful.</returns>
        public bool UpdateEntity(T entity)
        {
            return entityMapper.UpdateEntity(entity);
        }

        /// <summary>
        /// Deletes an <see cref="Entity" />.
        /// </summary>
        /// <param name="entityId">The <see cref="Entity" /> to delete.</param>
        /// <returns>If the delete was successful.</returns>
        public bool DeleteEntity(int entityId)
        {
            return entityMapper.DeleteEntity(entityId);
        }

        /// <summary>
        /// Gets all <see cref="Entity" /> objects from the map.
        /// </summary>
        /// <returns>All <see cref="Entity" /> objects from the map.</returns>
        public IEnumerable<T> GetAllEntities()
        {
            return entityMapper.GetAllEntities();
        }

        /// <summary>
        /// Deletes all entities from the persister.
        /// </summary>
        /// <returns></returns>
        public bool DeleteAllEntities()
        {
            throw new NotImplementedException();
        }
    }
}