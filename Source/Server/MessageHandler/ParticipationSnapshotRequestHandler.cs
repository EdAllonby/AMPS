using System.Collections.Generic;
using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Server.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="EntitySnapshotRequest{TEntity}" /> the Server received.
    /// </summary>
    internal sealed class ParticipationSnapshotRequestHandler : MessageHandler<EntitySnapshotRequest<Participation>>
    {
        public ParticipationSnapshotRequestHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
        }

        /// <summary>
        /// Handles the incoming <see cref="EntitySnapshotRequest{Participation}" />.
        /// </summary>
        /// <param name="message">
        /// The <see cref="EntitySnapshotRequest{Participation}" /> that has been received and needs to be handled.
        /// </param>
        protected override void HandleMessage(EntitySnapshotRequest<Participation> message)
        {
            IReadOnlyEntityRepository<User> userRepository = ServiceRegistry.GetService<RepositoryManager>().GetRepository<User>();
            var participationRepository = (ParticipationRepository) ServiceRegistry.GetService<RepositoryManager>().GetRepository<Participation>();

            User user = userRepository.FindEntityById(message.UserId);

            var clientManager = ServiceRegistry.GetService<IClientManager>();

            var userParticipations = new List<Participation>();

            foreach (Band band in user.Bands)
            {
                userParticipations.AddRange(participationRepository.GetParticipationsByBandId(band.Id));
            }

            var participationSnapshot = new EntitySnapshot<Participation>(userParticipations);

            clientManager.SendMessageToClient(participationSnapshot, message.UserId);
        }
    }
}