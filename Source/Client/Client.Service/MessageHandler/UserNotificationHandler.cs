using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Client.Service.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="EntityNotification{TEntity}" /> the Client received.
    /// </summary>
    internal sealed class UserNotificationHandler : MessageHandler<EntityNotification<User>>
    {
        public UserNotificationHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
        }

        protected override void HandleMessage(EntityNotification<User> message)
        {
            var userRepository = (IEntityRepository<User>) ServiceRegistry.GetService<IRepositoryManager>().GetRepository<User>();

            switch (message.NotificationType)
            {
                case NotificationType.Create:
                    userRepository.AddEntity(message.Entity);
                    break;
            }
        }
    }
}