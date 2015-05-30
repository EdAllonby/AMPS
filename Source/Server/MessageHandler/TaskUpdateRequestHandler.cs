using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Message.TaskMessage;
using Shared.Repository;

namespace Server.MessageHandler
{
    /// <summary>
    /// Handles an update to a <see cref="Task" />.
    /// </summary>
    internal sealed class TaskUpdateRequestHandler : IMessageHandler
    {
        public void HandleMessage(IMessage message, IServiceRegistry serviceRegistry)
        {
            TaskRepository taskRepository = (TaskRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Task>();

            var taskUpdateRequest = (TaskUpdateRequest) message;

            taskRepository.UpdateEntity(taskUpdateRequest.UpdatedTask);
        }
    }
}