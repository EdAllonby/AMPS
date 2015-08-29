using System;
using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;
using Utility;

namespace Client.Service.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="EntitySnapshot{T}" /> the Client received.
    /// </summary>
    internal sealed class EntitySnapshotHandler<T> : MessageHandler<EntitySnapshot<T>>, IBootstrapper where T : Entity
    {
        public EntitySnapshotHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
        }

        public override void HandleMessage(EntitySnapshot<T> message)
        {
            var entityRepository = (IEntityRepository<T>) serviceRegistry.GetService<RepositoryManager>().GetRepository<T>();

            foreach (T entity in message.Entities)
            {
                entityRepository.AddEntity(entity);
            }

            OnEntityBootstrapCompleted();
        }

        /// <summary>
        /// Fires when the snapshot process has complete.
        /// </summary>
        public event EventHandler<Type> EntityBootstrapCompleted;

        private void OnEntityBootstrapCompleted()
        {
            EventUtility.SafeFireEvent(EntityBootstrapCompleted, this, typeof(T));
        }
    }
}