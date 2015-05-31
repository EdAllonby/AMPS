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
    internal sealed class OnTaskChangedHandler : OnEntityChangedHandler
    {
        private readonly ParticipationRepository participationRepository;
        private readonly TaskRepository taskRepository;
        private readonly IReadOnlyEntityRepository<User> userRepository;

        public OnTaskChangedHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
            taskRepository = (TaskRepository) RepositoryManager.GetRepository<Task>();

            participationRepository = (ParticipationRepository) RepositoryManager.GetRepository<Participation>();
            userRepository = RepositoryManager.GetRepository<User>();

            taskRepository.EntityAdded += OnTaskAdded;
            taskRepository.EntityUpdated += OnTaskUpdated;
        }

        private void OnTaskUpdated(object sender, EntityChangedEventArgs<Task> e)
        {
            var bandParticipantIds = participationRepository.GetParticipationsByBandId(e.Entity.BandId)
                .Select(participant => participant.Id);

            var taskNotification = new EntityNotification<Task>(e.Entity, NotificationType.Update);

            ClientManager.SendMessageToClients(taskNotification, bandParticipantIds);
        }

        private void OnTaskAdded(object sender, EntityChangedEventArgs<Task> e)
        {
            var addedBacklogTaskNotification = new EntityNotification<Task>(e.Entity, NotificationType.Create);

            List<Participation> participantsInBand = participationRepository.GetParticipationsByBandId(e.Entity.BandId);

            IEnumerable<User> participantUsers = participantsInBand.Select(participant => userRepository.FindEntityById(participant.UserId));

            IEnumerable<int> connectedUserIds = participantUsers.Where(user => user.ConnectionStatus.UserConnectionStatus == ConnectionStatus.Status.Connected).Select(user => user.Id);

            ClientManager.SendMessageToClients(addedBacklogTaskNotification, connectedUserIds);
        }

        /// <summary>
        /// Removes event subscriptions to <see cref="TaskRepository" /> <see cref="Entity" /> changes.
        /// </summary>
        public override void StopOnMessageChangedHandling()
        {
            taskRepository.EntityAdded -= OnTaskAdded;
            taskRepository.EntityUpdated -= OnTaskUpdated;
        }
    }
}