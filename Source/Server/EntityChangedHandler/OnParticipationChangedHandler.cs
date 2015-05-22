using System.Collections.Generic;
using System.Linq;
using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Server.EntityChangedHandler
{
    /// <summary>
    /// Handles <see cref="Entity" /> changes to the <see cref="ParticipationRepository" />.
    /// </summary>
    internal sealed class OnParticipationChangedHandler : OnEntityChangedHandler
    {
        private readonly IReadOnlyEntityRepository<Band> bandRepository;
        private readonly ParticipationRepository participationRepository;
        private readonly TaskRepository taskRepository;

        /// <summary>
        /// Creates a new <see cref="ParticipationRepository" /> <see cref="Entity" /> changed handler and wires up the change events.
        /// </summary>
        /// <param name="serviceRegistry">The server's service registry.</param>
        public OnParticipationChangedHandler(IServiceRegistry serviceRegistry)
            : base(serviceRegistry)
        {
            participationRepository = (ParticipationRepository) RepositoryManager.GetRepository<Participation>();
            taskRepository = (TaskRepository) RepositoryManager.GetRepository<Task>();
            bandRepository = RepositoryManager.GetRepository<Band>();

            participationRepository.EntityAdded += OnParticipationAdded;
        }

        private void OnParticipationAdded(object sender, EntityChangedEventArgs<Participation> e)
        {
            Participation participation = e.Entity;

            List<Participation> bandParticipants = participationRepository.GetParticipationsByBandId(participation.BandId);

            var participationNotification = new EntityNotification<Participation>(participation, NotificationType.Create);

            IEnumerable<int> bandParticipantUserIds = bandParticipants.Select(bandParticipant => bandParticipant.UserId);

            ClientManager.SendMessageToClients(participationNotification, bandParticipantUserIds);

            List<Participation> otherParticipants = bandParticipants.Where(bandParticipant => !bandParticipant.Equals(participation)).ToList();

            otherParticipants.ForEach(otherParticipant => ClientManager.SendMessageToClient(new EntityNotification<Participation>(otherParticipant, NotificationType.Create), participation.UserId));

            Band band = bandRepository.FindEntityById(participation.BandId);

            SendBandNotificationToParticipants(band, participation.UserId, otherParticipants);

            if (band != null)
            {
                SendTasksToNewBandParticipant(band.Id, participation.UserId);
                SendJamsToNewBandParticipant(band.Id, participation.UserId);
            }
        }

        private void SendBandNotificationToParticipants(Band band, int newParticipantUserId, IEnumerable<Participation> otherParticipants)
        {
            if (band != null)
            {
                ClientManager.SendMessageToClient(new EntityNotification<Band>(band, NotificationType.Update), newParticipantUserId);

                IEnumerable<int> currentBandParticipantUserIds = otherParticipants.Select(participant => participant.UserId);

                ClientManager.SendMessageToClients(new EntityNotification<Band>(band, NotificationType.Update), currentBandParticipantUserIds);
            }
        }

        private void SendTasksToNewBandParticipant(int bandId, int newParticipantUserId)
        {
            var tasks = taskRepository.GetTasksAssociatedWithBand(bandId);

            foreach (Task task in tasks)
            {
                var taskNotification = new EntityNotification<Task>(task, NotificationType.Create);
                ClientManager.SendMessageToClient(taskNotification, newParticipantUserId);
            }
        }

        private void SendJamsToNewBandParticipant(int bandId, int newParticipantUserId)
        {
            var jamRepository = (JamRepository) RepositoryManager.GetRepository<Jam>();

            foreach (Jam jam in jamRepository.GetAllJamsInBand(bandId))
            {
                var jamNotification = new EntityNotification<Jam>(jam, NotificationType.Create);
                ClientManager.SendMessageToClient(jamNotification, newParticipantUserId);
            }
        }

        /// <summary>
        /// Removes event subscriptions to <see cref="JamRepository" /> <see cref="Entity"/> changes.
        /// </summary>
        public override void StopOnMessageChangedHandling()
        {
            participationRepository.EntityAdded -= OnParticipationAdded;
        }
    }
}