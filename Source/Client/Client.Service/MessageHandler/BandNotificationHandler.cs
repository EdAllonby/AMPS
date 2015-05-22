using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Client.Service.MessageHandler
{
    internal sealed class BandNotificationHandler : IMessageHandler
    {
        public void HandleMessage(IMessage message, IServiceRegistry serviceRegistry)
        {
            var bandNotification = (EntityNotification<Band>) message;

            var bandRepository = (IEntityRepository<Band>) serviceRegistry.GetService<RepositoryManager>().GetRepository<Band>();

            switch (bandNotification.NotificationType)
            {
                case NotificationType.Create:
                    bandRepository.AddEntity(bandNotification.Entity);
                    break;

                case NotificationType.Update:
                    bandRepository.UpdateEntity(bandNotification.Entity);
                    break;
            }
        }
    }
}