using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Client.Service.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="EntityNotification{T}" /> the Client received.
    /// </summary>
    internal sealed class ParticipationNotificationHandler : MessageHandler<EntityNotification<Participation>>
    {
        public ParticipationNotificationHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
        }

        public override void HandleMessage(EntityNotification<Participation> message)
        {
            var participationRepository = (IEntityRepository<Participation>) serviceRegistry.GetService<RepositoryManager>().GetRepository<Participation>();

            participationRepository.AddEntity(message.Entity);
        }
    }
}