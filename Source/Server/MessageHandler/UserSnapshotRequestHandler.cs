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
    internal sealed class UserSnapshotRequestHandler : MessageHandler<EntitySnapshotRequest<User>>
    {
        public UserSnapshotRequestHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
        }

        /// <summary>
        /// Handles the incoming <see cref="EntitySnapshotRequest{User}" />.
        /// </summary>
        /// <param name="message">The <see cref="EntitySnapshotRequest{User}" /> that has been received and needs to be handled.</param>
        public override void HandleMessage(EntitySnapshotRequest<User> message)
        {
            IReadOnlyEntityRepository<User> userRepository = serviceRegistry.GetService<RepositoryManager>().GetRepository<User>();
            var clientManager = serviceRegistry.GetService<IClientManager>();

            IEnumerable<User> currentUsers = userRepository.GetAllEntities();
            var userSnapshot = new EntitySnapshot<User>(currentUsers);

            clientManager.SendMessageToClient(userSnapshot, message.UserId);
        }
    }
}