using System;
using Shared;
using Shared.Domain;
using Shared.Message.TaskMessage;
using Shared.Repository;

namespace Server.MessageHandler
{
    public sealed class TaskCommentRequestHandler : MessageHandler<TaskCommentRequest>
    {
        public TaskCommentRequestHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
        }

        public override void HandleMessage(TaskCommentRequest message)
        {
            TaskRepository taskRepository = (TaskRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Task>();

            EntityIdAllocatorFactory entityIdAllocatorFactory = serviceRegistry.GetService<EntityIdAllocatorFactory>();

            TaskComment taskComment = new TaskComment(entityIdAllocatorFactory.AllocateEntityId<TaskComment>(), message.TaskComment, DateTime.Now);

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