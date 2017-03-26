using Shared;
using Shared.Domain;
using Shared.Message.ParticipationMessage;
using Shared.Repository;

namespace Server.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="ParticipationRequest" /> the Server received.
    /// </summary>
    internal sealed class ParticipationRequestHandler : MessageHandler<ParticipationRequest>
    {
        public ParticipationRequestHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
        }

        /// <summary>
        /// Handles the incoming <see cref="ParticipationRequest" />.
        /// </summary>
        /// <param name="message">The <see cref="ParticipationRequest" /> that has been received and needs to be handled.</param>
        protected override void HandleMessage(ParticipationRequest message)
        {
            var participationRepository = (ParticipationRepository) ServiceRegistry.GetService<IRepositoryManager>().GetRepository<Participation>();

            var entityIdAllocatorFactory = ServiceRegistry.GetService<EntityIdAllocatorFactory>();
            AddUserToBand(message, entityIdAllocatorFactory, participationRepository);
        }

        private void AddUserToBand(ParticipationRequest participationRequest, EntityIdAllocatorFactory entityIdAllocatorFactory, IEntityRepository<Participation> participationRepository)
        {
            int participationId = entityIdAllocatorFactory.AllocateEntityId<Participation>();

            participationRequest.Participation.RepositoryManager = ServiceRegistry.GetService<IRepositoryManager>();

            var participation = new Participation(participationId, participationRequest.Participation.User.Id, participationRequest.Participation.Band.Id, participationRequest.Participation.IsLeader);

            participationRepository.AddEntity(participation);
        }
    }
}