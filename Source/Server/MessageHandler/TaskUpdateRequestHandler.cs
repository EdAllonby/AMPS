using System;
using Shared;
using Shared.Domain;
using Shared.Message.TaskMessage;
using Shared.Repository;

namespace Server.MessageHandler
{
    /// <summary>
    /// Handles an update to a <see cref="Task" />.
    /// </summary>
    internal sealed class TaskUpdateRequestHandler : MessageHandler<TaskUpdateRequest>
    {
        public TaskUpdateRequestHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
        }

        protected override void HandleMessage(TaskUpdateRequest message)
        {
            var taskRepository = (TaskRepository) ServiceRegistry.GetService<IRepositoryManager>().GetRepository<Task>();

            Task updatedTask = message.UpdatedTask;

            if (updatedTask.IsCompleted && updatedTask.CompletedDate == DateTime.MinValue)
            {
                updatedTask.CompletedDate = DateTime.Now;
            }

            taskRepository.UpdateEntity(updatedTask);
        }
    }
}