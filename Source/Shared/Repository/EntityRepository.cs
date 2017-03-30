using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Shared.Domain;
using Shared.Message;
using Utility;

namespace Shared.Repository
{
    /// <summary>
    /// Models a <see cref="Repository" /> for an <see cref="Entity" />.
    /// </summary>
    /// <typeparam name="T">An <see cref="Entity" />.</typeparam>
    public abstract class EntityRepository<T> : IEntityRepository<T> where T : Entity
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(EntityRepository<T>));
        private IRepositoryManager repositoryManager;

        /// <summary>
        /// The persistence strategy to use.
        /// </summary>
        private IEntityPersister<T> TypedEntityPersister { get; set; }

        public IEntityPersister EntityPersister
        {
            set { TypedEntityPersister = (IEntityPersister<T>) value; }
        }

        /// <summary>
        /// The owning repository manager.
        /// </summary>
        public IRepositoryManager RepositoryManager
        {
            get { return repositoryManager; }
            set
            {
                if (repositoryManager != null)
                {
                    throw new ArgumentException("RepositoryManager already set.");
                }

                repositoryManager = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="Entity" /> type that is held in the repository.
        /// </summary>
        public Type EnclosedEntityType => typeof(T);

        /// <summary>
        /// Gets raised when an entity in the repository has been added.
        /// </summary>
        public event EventHandler<EntityChangedEventArgs<T>> EntityAdded;

        /// <summary>
        /// Gets raised when an entity in the repository has been updated.
        /// </summary>
        public event EventHandler<EntityChangedEventArgs<T>> EntityUpdated;

        /// <summary>
        /// Gets raised when an entity in the repository has been removed.
        /// </summary>
        public event EventHandler<EntityChangedEventArgs<T>> EntityRemoved;

        /// <summary>
        /// Adds an <see cref="Entity" /> to the repository.
        /// </summary>
        /// <param name="entity">The <see cref="Entity" /> to add.</param>
        public void AddEntity(T entity)
        {
            entity.RepositoryManager = repositoryManager;

            if (!entity.CreatedDate.HasValue)
            {
                entity.CreatedDate = DateTime.UtcNow;
            }

            bool didInsert = TypedEntityPersister.InsertEntity(entity);

            if (didInsert)
            {
                Log.DebugFormat("Entity with Id {0} added to {1} repository.", entity.Id, EnclosedEntityType.Name);

                OnEntityAdded(entity);
            }
            else
            {
                Log.ErrorFormat("Could not add entity of type {0} with Id {1} to Database.", typeof(T), entity.Id);
            }
        }

        /// <summary>
        /// Updates an <see cref="Entity" /> in the <see cref="EntityRepository{T}" />.
        /// </summary>
        /// <typeparam name="T">The <see cref="Entity" /> that the <see cref="EntityRepository{T}" /> holds.</typeparam>
        public void UpdateEntity(T entity)
        {
            entity.RepositoryManager = repositoryManager;

            entity.UpdatedDate = DateTime.UtcNow;

            bool didUpdate = TypedEntityPersister.UpdateEntity(entity);

            if (didUpdate)
            {
                T previousEntity = FindEntityById(entity.Id);

                TypedEntityPersister.UpdateEntity(entity);

                Log.DebugFormat("Entity with Id {0} has been updated in {1} repository.", entity.Id, EnclosedEntityType.Name);

                OnEntityUpdated(entity, previousEntity);
            }
            else
            {
                Log.ErrorFormat("Could not update entity of type {0} with Id {1} to {2} repository.", typeof(T).Name, entity.Id, EnclosedEntityType.Name);
            }
        }

        /// <summary>
        /// Retrieves an <see cref="Entity" /> entity from the repository.
        /// </summary>
        /// <param name="entityId">The <see cref="Entity" /> entity Id to find.</param>
        /// <returns>The <see cref="Entity" /> which matches the ID. If no <see cref="Entity" /> is found, return null.</returns>
        public T FindEntityById(int entityId)
        {
            T entity = TypedEntityPersister.GetEntityById(entityId);

            if (entity != null)
            {
                entity.RepositoryManager = RepositoryManager;
            }

            return entity;
        }

        /// <summary>
        /// Retrieves all <see cref="Entity" />s from the repository.
        /// </summary>
        /// <returns>A collection of all <see cref="Entity" />s in the repository.</returns>
        public IEnumerable<T> GetAllEntities()
        {
            List<T> entities = TypedEntityPersister.GetAllEntities().ToList();
            foreach (T entity in entities)
            {
                entity.RepositoryManager = RepositoryManager;
            }
            return entities;
        }

        /// <summary>
        /// Delete an entity from the repository.
        /// </summary>
        /// <param name="entityId">The <see cref="Entity" /> Id to delete.</param>
        /// <returns>If the delete was successful.</returns>
        public void DeleteEntity(int entityId)
        {
            bool didDelete = TypedEntityPersister.DeleteEntity(entityId);

            if (didDelete)
            {
                Log.DebugFormat("Entity with Id {0} removed from {1} repository", entityId, EnclosedEntityType.Name);
            }
        }

        /// <summary>
        /// Remove all entities.
        /// </summary>
        public void DeleteAll()
        {
            bool didDelete = TypedEntityPersister.DeleteAllEntities();

            if (didDelete)
            {
                Log.DebugFormat("All entities removed from {0} repository", EnclosedEntityType.Name);
            }
        }

        public void Load()
        {
            TypedEntityPersister.GetAllEntities();
        }

        private void OnEntityAdded(T entity)
        {
            var entityChangedEventArgs = new EntityChangedEventArgs<T>(entity, NotificationType.Create);

            EventUtility.SafeFireEvent(EntityAdded, this, entityChangedEventArgs);
        }

        /// <summary>
        /// Fires an <see cref="EntityUpdated" /> event.
        /// </summary>
        /// <param name="entity">The current state of the Entity.</param>
        /// <param name="previousEntity">The previous state of the Entity, before being updated.</param>
        protected void OnEntityUpdated(T entity, T previousEntity)
        {
            var entityChangedEventArgs = new EntityChangedEventArgs<T>(entity, previousEntity);

            EventUtility.SafeFireEvent(EntityUpdated, this, entityChangedEventArgs);
        }
    }
}