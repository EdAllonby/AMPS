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
            var entityIdAllocatorFactory = ServiceRegistry.GetService<EntityIdAllocatorFactory>();
            var taskComment = new TaskComment(entityIdAllocatorFactory.AllocateEntityId<TaskComment>(), message.TaskComment, DateTime.Now);
            taskComment.RepositoryManager = ServiceRegistry.GetService<IRepositoryManager>();
            Task task = taskComment.Task;
            task.AddCommentToRelevantParent(taskComment);
            var taskRepository = (TaskRepository) ServiceRegistry.GetService<IRepositoryManager>().GetRepository<Task>();
            taskRepository.UpdateEntity(task);
        }
    }
}