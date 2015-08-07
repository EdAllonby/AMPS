using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Client.Service.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="EntityNotification{T}" /> the Client received.
    /// </summary>
    internal sealed class UserNotificationHandler : MessageHandler<EntityNotification<User>>
    {
        public override void HandleMessage(EntityNotification<User> message, IServiceRegistry serviceRegistry)
        {
            var userRepository = (IEntityRepository<User>) serviceRegistry.GetService<RepositoryManager>().GetRepository<User>();

            switch (message.NotificationType)
            {
                case NotificationType.Create:
                    userRepository.AddEntity(message.Entity);
                    break;
            }
        }
    }
}