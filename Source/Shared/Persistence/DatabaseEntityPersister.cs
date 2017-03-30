using System;
using System.Collections.Generic;
using log4net;
using Shared.Domain;
using Shared.Repository;

namespace Shared.Persistence
{
    /// <summary>
    /// Persist entities in a Database.
    /// </summary>
    /// <typeparam name="T">The <see cref="Entity" /> to persist in a Database.</typeparam>
    public sealed class DatabaseEntityPersister<T> : IEntityPersister<T> where T : Entity
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DatabaseEntityPersister<T>));

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
            try
            {
                return entityMapper.InsertEntity(entity);
            }
            catch (Exception e)
            {
                Log.Error($"Could not insert {typeof(T)}.", e);
                return false;
            }
        }

        /// <summary>
        /// Updates an <see cref="Entity" />.
        /// </summary>
        /// <param name="entity">The updated <see cref="Entity" />.</param>
        /// <returns>If the update was successful.</returns>
        public bool UpdateEntity(T entity)
        {
            try
            {
                return entityMapper.UpdateEntity(entity);
            }
            catch (Exception e)
            {
                Log.Error($"Could not update {typeof(T)}.", e);
                return false;
            }
        }

        /// <summary>
        /// Deletes an <see cref="Entity" />.
        /// </summary>
        /// <param name="entityId">The <see cref="Entity" /> to delete.</param>
        /// <returns>If the delete was successful.</returns>
        public bool DeleteEntity(int entityId)
        {
            try
            {
                return entityMapper.DeleteEntity(entityId);
            }
            catch (Exception e)
            {
                Log.Error($"Could not delete {typeof(T)} with Id {entityId}.", e);
                return false;
            }
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