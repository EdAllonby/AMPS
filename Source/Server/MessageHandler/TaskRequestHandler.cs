using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Message.TaskMessage;
using Shared.Repository;

namespace Server.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="TaskRequest" /> the Server recieved.
    /// </summary>
    internal sealed class TaskRequestHandler : IMessageHandler
    {
        /// <summary>
        /// Handles the incoming <see cref="TaskRequest" />.
        /// </summary>
        /// <param name="message">The <see cref="TaskRequest" /> that has been received and needs to be handled.</param>
        /// <param name="serviceRegistry">The services needed to handle the message correctly.</param>
        public void HandleMessage(IMessage message, IServiceRegistry serviceRegistry)
        {
            var taskRequest = (TaskRequest) message;

            var taskRepository = (IEntityRepository<Task>) serviceRegistry.GetService<RepositoryManager>().GetRepository<Task>();

            EntityIdAllocatorFactory entityIdAllocatorFactory = serviceRegistry.GetService<EntityIdAllocatorFactory>();

            Task newTask = new Task(entityIdAllocatorFactory.AllocateEntityId<Task>(), taskRequest.Task);

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