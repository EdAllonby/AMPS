using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Client.Service.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="EntityNotification{T}" /> the Client received.
    /// </summary>
    internal sealed class TaskNotificationHandler : IMessageHandler
    {
        public void HandleMessage(IMessage message, IServiceRegistry serviceRegistry)
        {
            var taskNotification = (EntityNotification<Task>) message;
            var toastNotifier = serviceRegistry.GetService<ToastNotificationManager>();
            var taskRepository = (IEntityRepository<Task>) serviceRegistry.GetService<RepositoryManager>().GetRepository<Task>();

            Task task = taskNotification.Entity;

            switch (taskNotification.NotificationType)
            {
                case NotificationType.Create:
                    taskRepository.AddEntity(task);
                    toastNotifier.Notify("A task has been created.");
                    break;
                case NotificationType.Update:
                    taskRepository.UpdateEntity(task);
                    toastNotifier.Notify("A task has been updated.");
                    break;
            }
        }
    }
}