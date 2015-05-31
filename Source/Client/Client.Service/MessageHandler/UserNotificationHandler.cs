using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Client.Service.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="EntityNotification{T}" /> the Client received.
    /// </summary>
    internal sealed class UserNotificationHandler : IMessageHandler
    {
        public void HandleMessage(IMessage message, IServiceRegistry serviceRegistry)
        {
            var userNotification = (EntityNotification<User>) message;

            var userRepository = (IEntityRepository<User>) serviceRegistry.GetService<RepositoryManager>().GetRepository<User>();

            switch (userNotification.NotificationType)
            {
                case NotificationType.Create:
                    userRepository.AddEntity(userNotification.Entity);
                    break;
            }
        }
    }
}