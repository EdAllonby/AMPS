using System.Collections.Generic;
using System.Linq;
using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Server.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="EntitySnapshotRequest{TEntity}" /> the Server received.
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
        protected override void HandleMessage(EntitySnapshotRequest<Task> message)
        {
            IReadOnlyEntityRepository<User> userRepository = ServiceRegistry.GetService<RepositoryManager>().GetRepository<User>();
            User user = userRepository.FindEntityById(message.UserId);

            IReadOnlyEntityRepository<Task> taskRepository = ServiceRegistry.GetService<RepositoryManager>().GetRepository<Task>();

            List<Task> tasksUserCanSee = taskRepository.GetAllEntities().Where(task => user.Bands.Select(band => band.Id).Contains(task.BandId)).ToList();

            var taskSnapshot = new EntitySnapshot<Task>(tasksUserCanSee);

            var clientManager = ServiceRegistry.GetService<IClientManager>();
            clientManager.SendMessageToClient(taskSnapshot, message.UserId);
        }
    }
}