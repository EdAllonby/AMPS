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
        /// <summary>
        /// Handles the incoming <see cref="TaskRequest" />.
        /// </summary>
        /// <param name="message">The <see cref="TaskRequest" /> that has been received and needs to be handled.</param>
        /// <param name="serviceRegistry">The services needed to handle the message correctly.</param>
        public override void HandleMessage(TaskRequest message, IServiceRegistry serviceRegistry)
        {
            var taskRepository = (IEntityRepository<Task>) serviceRegistry.GetService<RepositoryManager>().GetRepository<Task>();

            EntityIdAllocatorFactory entityIdAllocatorFactory = serviceRegistry.GetService<EntityIdAllocatorFactory>();

            Task newTask = new Task(entityIdAllocatorFactory.AllocateEntityId<Task>(), message.Task);

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