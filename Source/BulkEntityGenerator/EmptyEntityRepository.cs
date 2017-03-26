using System;
using System.Collections.Generic;
using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace BulkEntityGenerator
{
    public class EmptyEntityRepository<T> : IEntityRepository<T> where T : Entity
    {
        public event EventHandler<EntityChangedEventArgs<T>> EntityAdded;
        public event EventHandler<EntityChangedEventArgs<T>> EntityUpdated;
        public event EventHandler<EntityChangedEventArgs<T>> EntityRemoved;

        public T FindEntityById(int entityId)
        {
            return null;
        }

        public IEnumerable<T> GetAllEntities()
        {
            return new List<T>();
        }

        public IEntityPersister EntityPersister { get; set; }

        public IRepositoryManager RepositoryManager { get; set; }

        public Type EnclosedEntityType => typeof(T);

        public void DeleteEntity(int entityId)
        {
        }

        public void DeleteAll()
        {
        }

        public void Load()
        {
        }

        public void AddEntity(T entity)
        {
            EntityAdded?.Invoke(this, new EntityChangedEventArgs<T>(null, NotificationType.Create));
        }

        public void UpdateEntity(T entity)
        {
            EntityAdded?.Invoke(this, new EntityChangedEventArgs<T>(null, NotificationType.Update));
        }
    }
}