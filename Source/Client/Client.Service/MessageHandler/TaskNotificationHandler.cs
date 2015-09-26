﻿using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Client.Service.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="EntityNotification{T}" /> the Client received.
    /// </summary>
    internal sealed class TaskNotificationHandler : MessageHandler<EntityNotification<Task>>
    {
        public TaskNotificationHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
        }

        protected override void HandleMessage(EntityNotification<Task> message)
        {
            var toastNotifier = ServiceRegistry.GetService<ToastNotificationManager>();
            var taskRepository = (IEntityRepository<Task>) ServiceRegistry.GetService<RepositoryManager>().GetRepository<Task>();

            Task task = message.Entity;

            switch (message.NotificationType)
            {
                case NotificationType.Create:
                    taskRepository.AddEntity(task);
                    toastNotifier.Notify(new Notification("A Task has been created", "A task has been created."));
                    break;
                case NotificationType.Update:
                    taskRepository.UpdateEntity(task);
                    toastNotifier.Notify(new Notification("A Task has been updated", "A task has been updated."));
                    break;
            }
        }
    }
}