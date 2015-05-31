using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Client.Service.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="EntityNotification{T}" /> the Client received.
    /// </summary>
    internal sealed class ParticipationNotificationHandler : IMessageHandler
    {
        public void HandleMessage(IMessage message, IServiceRegistry serviceRegistry)
        {
            var participationNotification = (EntityNotification<Participation>) message;

            var participationRepository = (IEntityRepository<Participation>) serviceRegistry.GetService<RepositoryManager>().GetRepository<Participation>();

            participationRepository.AddEntity(participationNotification.Entity);
        }
    }
}