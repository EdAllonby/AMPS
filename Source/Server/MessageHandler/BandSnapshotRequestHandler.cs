using System.Collections.Generic;
using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Server.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="EntitySnapshotRequest{T}" /> the Server received.
    /// </summary>
    internal class BandSnapshotRequestHandler : IMessageHandler
    {
        /// <summary>
        /// Handles the incoming <see cref="EntitySnapshotRequest{Band}" />.
        /// </summary>
        /// <param name="message">The <see cref="EntitySnapshotRequest{Band}" /> that has been received and needs to be handled.</param>
        /// <param name="serviceRegistry">The services needed to handle the message correctly.</param>
        public void HandleMessage(IMessage message, IServiceRegistry serviceRegistry)
        {
            var bandSnapshotRequest = (EntitySnapshotRequest<Band>) message;

            IReadOnlyEntityRepository<Band> bandRepository = serviceRegistry.GetService<RepositoryManager>().GetRepository<Band>();
            var clientManager = serviceRegistry.GetService<IClientManager>();

            IEnumerable<Band> currentUsers = bandRepository.GetAllEntities();
            var bandSnapshot = new EntitySnapshot<Band>(currentUsers);

            clientManager.SendMessageToClient(bandSnapshot, bandSnapshotRequest.UserId);
        }
    }
}