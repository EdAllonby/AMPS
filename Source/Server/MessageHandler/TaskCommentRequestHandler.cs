using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Message.TaskMessage;
using Shared.Repository;

namespace Server.MessageHandler
{
    public sealed class TaskCommentRequestHandler : IMessageHandler
    {
        public void HandleMessage(IMessage message, IServiceRegistry serviceRegistry)
        {
            TaskComment incompleteTaskComment = ((TaskCommentRequest) message).TaskComment;

            TaskRepository taskRepository = (TaskRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Task>();

            EntityIdAllocatorFactory entityIdAllocatorFactory = serviceRegistry.GetService<EntityIdAllocatorFactory>();

            TaskComment taskComment = new TaskComment(entityIdAllocatorFactory.AllocateEntityId<TaskComment>(), incompleteTaskComment);

            Task task = taskRepository.FindEntityById(taskComment.TaskId);

            if (taskComment.ParentComment != null)
            {
                task.AddCommentToRelevantParent(taskComment);
            }
            else
            {
                task.Comments.Add(taskComment);
            }

            taskRepository.UpdateEntity(task);
        }
    }
}