using Shared;
using Shared.Domain;
using Shared.Message.TaskMessage;
using Shared.Repository;

namespace Server.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="TaskRequest" /> the Server recieved.
    /// </summary>
    internal sealed class TaskRequestHandler : MessageHandler<TaskRequest>
    {
        public TaskRequestHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
        }

        /// <summary>
        /// Handles the incoming <see cref="TaskRequest" />.
        /// </summary>
        /// <param name="message">The <see cref="TaskRequest" /> that has been received and needs to be handled.</param>
        protected override void HandleMessage(TaskRequest message)
        {
            var taskRepository = (IEntityRepository<Task>) ServiceRegistry.GetService<IRepositoryManager>().GetRepository<Task>();

            IReadOnlyEntityRepository<Band> bandRepository = ServiceRegistry.GetService<IRepositoryManager>().GetRepository<Band>();

            int owningBandId = message.Task.BandId;

            if (bandRepository.FindEntityById(owningBandId) == null)
            {
                Log.WarnFormat("Not handling task request. No band found with Id {0}.", owningBandId);
                return;
            }

            var entityIdAllocatorFactory = ServiceRegistry.GetService<EntityIdAllocatorFactory>();


            var newTask = new Task(entityIdAllocatorFactory.AllocateEntityId<Task>(), message.Task);


            if (taskRepository.FindEntityById(newTask.Id) == null)
            {
                taskRepository.AddEntity(newTask);
            }
            else
            {
                taskRepository.UpdateEntity(newTask);
            }
        }
    }
}