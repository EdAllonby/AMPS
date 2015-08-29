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
    internal sealed class TaskSnapshotRequestHandler : MessageHandler<EntitySnapshotRequest<Task>>
    {
        public TaskSnapshotRequestHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
        }

        /// <summary>
        /// Handles the incoming <see cref="EntitySnapshotRequest{Task}" />.
        /// </summary>
        /// <param name="message">The <see cref="EntitySnapshotRequest{Task}" /> that has been received and needs to be handled.</param>
        public override void HandleMessage(EntitySnapshotRequest<Task> message)
        {
            IReadOnlyEntityRepository<Task> taskRepository = serviceRegistry.GetService<RepositoryManager>().GetRepository<Task>();
            var participationRepository = (ParticipationRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Participation>();

            var clientManager = serviceRegistry.GetService<IClientManager>();

            List<int> bandIdsUserIsIn = participationRepository.GetAllBandIdsByUserId(message.UserId).ToList();

            List<Task> tasksUserCanSee = taskRepository.GetAllEntities().Where(task => bandIdsUserIsIn.Contains(task.BandId)).ToList();

            var taskSnapshot = new EntitySnapshot<Task>(tasksUserCanSee);

            clientManager.SendMessageToClient(taskSnapshot, message.UserId);
        }
    }
}