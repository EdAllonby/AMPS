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
    internal sealed class TaskCommentChangedHandler : EntityChangedHandler
    {
        private readonly TaskCommentRepository taskCommentRepository;

        public TaskCommentChangedHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
            taskCommentRepository = (TaskCommentRepository) RepositoryManager.GetRepository<TaskComment>();
            taskCommentRepository.EntityAdded += OnTaskCommentAdded;
        }

        /// <summary>
        /// Removes event subscriptions to <see cref="TaskRepository" /> <see cref="Entity" /> changes.
        /// </summary>
        public override void StopOnMessageChangedHandling()
        {
            taskCommentRepository.EntityAdded -= OnTaskCommentAdded;
        }

        private void OnTaskCommentAdded(object sender, EntityChangedEventArgs<TaskComment> e)
        {
            TaskComment taskComment = e.Entity;
            var entityNotification = new EntityNotification<TaskComment>(taskComment, NotificationType.Create);
            IEnumerable<User> members = taskComment.Task.Band.Members;
            ClientManager.SendMessageToClients(entityNotification, members.Select(user => user.Id));
        }
    }
}