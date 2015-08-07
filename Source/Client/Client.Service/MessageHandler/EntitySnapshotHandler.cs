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
    internal sealed class EntitySnapshotHandler<T> : MessageHandler<EntitySnapshot<T>> where T : Entity
    {
        public override void HandleMessage(EntitySnapshot<T> message, IServiceRegistry serviceRegistry)
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
        public event EventHandler EntityBootstrapCompleted;

        private void OnEntityBootstrapCompleted()
        {
            EventUtility.SafeFireEvent(EntityBootstrapCompleted, this);
        }
    }
}