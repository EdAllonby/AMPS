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

        protected override void HandleMessage(TaskCommentRequest message)
        {
            TaskRepository taskRepository = (TaskRepository) ServiceRegistry.GetService<RepositoryManager>().GetRepository<Task>();

            EntityIdAllocatorFactory entityIdAllocatorFactory = ServiceRegistry.GetService<EntityIdAllocatorFactory>();

            TaskComment taskComment = new TaskComment(entityIdAllocatorFactory.AllocateEntityId<TaskComment>(), message.TaskComment, DateTime.Now);

            Task task = taskRepository.FindEntityById(taskComment.TaskId);

            task.AddCommentToRelevantParent(taskComment);

            taskRepository.UpdateEntity(task);
        }
    }
}