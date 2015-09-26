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
    internal sealed class EntityBootstrapper<TEntity> : MessageHandler<EntitySnapshot<TEntity>>, IBootstrapper where TEntity : Entity
    {
        public EntityBootstrapper(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
        }

        protected override void HandleMessage(EntitySnapshot<TEntity> message)
        {
            var entityRepository = (IEntityRepository<TEntity>) ServiceRegistry.GetService<RepositoryManager>().GetRepository<TEntity>();

            foreach (TEntity entity in message.Entities)
            {
                entityRepository.AddEntity(entity);
            }

            OnEntityBootstrapCompleted();
        }

        /// <summary>
        /// Fires when the snapshot process has complete.
        /// </summary>
        public event EventHandler<EntityBootstrapEventArgs> EntityBootstrapCompleted;

        private void OnEntityBootstrapCompleted()
        {
            EventUtility.SafeFireEvent(EntityBootstrapCompleted, this, new EntityBootstrapEventArgs(typeof(TEntity)));
        }
    }
}