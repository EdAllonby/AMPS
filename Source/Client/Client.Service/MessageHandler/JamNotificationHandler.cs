using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Client.Service.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="EntityNotification{T}" /> the Client received.
    /// </summary>
    internal sealed class JamNotificationHandler : IMessageHandler
    {
        public void HandleMessage(IMessage message, IServiceRegistry serviceRegistry)
        {
            var jamNotification = (EntityNotification<Jam>) message;

            var jamRepository = (JamRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Jam>();

            switch (jamNotification.NotificationType)
            {
                case NotificationType.Create:
                    jamRepository.AddEntity(jamNotification.Entity);
                    break;

                case NotificationType.Update:
                    jamRepository.UpdateEntity(jamNotification.Entity);
                    break;
            }
        }
    }
}