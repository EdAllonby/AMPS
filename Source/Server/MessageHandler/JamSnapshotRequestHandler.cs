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
    internal sealed class JamSnapshotRequestHandler : MessageHandler<EntitySnapshotRequest<Jam>>
    {
        public JamSnapshotRequestHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
        }

        /// <summary>
        /// Handles the incoming <see cref="EntitySnapshotRequest{Jam}" />.
        /// </summary>
        /// <param name="message">The <see cref="EntitySnapshotRequest{Jam}" /> that has been received and needs to be handled.</param>
        protected override void HandleMessage(EntitySnapshotRequest<Jam> message)
        {
            var userRepository = (UserRepository) ServiceRegistry.GetService<RepositoryManager>().GetRepository<User>();
            IReadOnlyEntityRepository<Jam> jamRepository = ServiceRegistry.GetService<RepositoryManager>().GetRepository<Jam>();
            var clientManager = ServiceRegistry.GetService<IClientManager>();

            User user = userRepository.FindEntityById(message.UserId);

            IEnumerable<int> bandIds = user.Bands.Select(band=>band.Id);

            List<Jam> jams = jamRepository.GetAllEntities().Where(jam => bandIds.Contains(jam.Band.Id)).ToList();

            var jamSnapshot = new EntitySnapshot<Jam>(jams);

            clientManager.SendMessageToClient(jamSnapshot, message.UserId);
        }
    }
}