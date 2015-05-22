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
    internal sealed class EntitySnapshotHandler<T> : IMessageHandler where T : Entity
    {
        public void HandleMessage(IMessage message, IServiceRegistry serviceRegistry)
        {
            var entitySnapshot = (EntitySnapshot<T>) message;

            var entityRepository = (IEntityRepository<T>) serviceRegistry.GetService<RepositoryManager>().GetRepository<T>();

            foreach (T entity in entitySnapshot.Entities)
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