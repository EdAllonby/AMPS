using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Message.TaskMessage;
using Shared.Repository;

namespace Server.MessageHandler
{
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