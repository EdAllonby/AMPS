using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Client.Service.MessageHandler
{
    internal sealed class BandNotificationHandler : MessageHandler<EntityNotification<Band>>
    {
        public BandNotificationHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
        }

        protected override void HandleMessage(EntityNotification<Band> message)
        {
            var bandRepository = (IEntityRepository<Band>) ServiceRegistry.GetService<RepositoryManager>().GetRepository<Band>();

            switch (message.NotificationType)
            {
                case NotificationType.Create:
                    bandRepository.AddEntity(message.Entity);
                    break;

                case NotificationType.Update:
                    bandRepository.UpdateEntity(message.Entity);
                    break;
            }
        }
    }
}