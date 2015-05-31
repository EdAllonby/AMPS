using System.Collections.Generic;
using System.Linq;
using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Server.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="EntitySnapshotRequest{T}" /> the Server received.
    /// </summary>
    internal class JamSnapshotRequestHandler : IMessageHandler
    {
        /// <summary>
        /// Handles the incoming <see cref="EntitySnapshotRequest{Jam}" />.
        /// </summary>
        /// <param name="message">The <see cref="EntitySnapshotRequest{Jam}" /> that has been received and needs to be handled.</param>
        /// <param name="serviceRegistry">The services needed to handle the message correctly.</param>
        public void HandleMessage(IMessage message, IServiceRegistry serviceRegistry)
        {
            var jamSnapshotRequest = (EntitySnapshotRequest<Jam>) message;
            var participationRepository = (ParticipationRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Participation>();
            IReadOnlyEntityRepository<Jam> jamRepository = serviceRegistry.GetService<RepositoryManager>().GetRepository<Jam>();
            var clientManager = serviceRegistry.GetService<IClientManager>();

            IEnumerable<int> bandIds = participationRepository.GetAllBandIdsByUserId(jamSnapshotRequest.UserId);

            List<Jam> jams = jamRepository.GetAllEntities().Where(jam => bandIds.Contains(jam.BandId)).ToList();

            var jamSnapshot = new EntitySnapshot<Jam>(jams);

            clientManager.SendMessageToClient(jamSnapshot, jamSnapshotRequest.UserId);
        }
    }
}