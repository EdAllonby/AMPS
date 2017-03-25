using System.Collections.Generic;
using System.Linq;
using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Server.EntityChangedHandler
{
    /// <summary>
    /// Handles <see cref="Task" /> changes to the <see cref="TaskRepository" />.
    /// </summary>
    internal sealed class TaskChangedHandler : EntityChangedHandler
    {
        private readonly TaskRepository taskRepository;

        public TaskChangedHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
            taskRepository = (TaskRepository) RepositoryManager.GetRepository<Task>();

            taskRepository.EntityAdded += OnTaskAdded;
            taskRepository.EntityUpdated += OnTaskUpdated;
        }

        /// <summary>
        /// Removes event subscriptions to <see cref="TaskRepository" /> <see cref="Entity" /> changes.
        /// </summary>
        public override void StopOnMessageChangedHandling()
        {
            taskRepository.EntityAdded -= OnTaskAdded;
            taskRepository.EntityUpdated -= OnTaskUpdated;
        }

        private void OnTaskUpdated(object sender, EntityChangedEventArgs<Task> e)
        {
            var taskNotification = new EntityNotification<Task>(e.Entity, NotificationType.Update);

            IEnumerable<User> band = e.Entity.Band.Members;
            ClientManager.SendMessageToClients(taskNotification, band.Select(user => user.Id));
        }

        private void OnTaskAdded(object sender, EntityChangedEventArgs<Task> e)
        {
            var addedBacklogTaskNotification = new EntityNotification<Task>(e.Entity, NotificationType.Create);

            IEnumerable<User> band = e.Entity.Band.Members;
            ClientManager.SendMessageToClients(addedBacklogTaskNotification, band.Select(user => user.Id));
        }
    }
}