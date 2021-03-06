﻿using System.Collections.Generic;
using System.Linq;
using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Server.EntityChangedHandler
{
    /// <summary>
    /// Handles <see cref="Entity" /> changes to the <see cref="JamRepository" />.
    /// </summary>
    internal sealed class JamChangedHandler : EntityChangedHandler
    {
        private readonly IReadOnlyEntityRepository<Jam> jamRepository;
        private readonly ParticipationRepository participationRepository;

        /// <summary>
        /// Creates a new <see cref="JamRepository" /> <see cref="Entity" /> changed handler and wires up the change events.
        /// </summary>
        /// <param name="serviceRegistry">The server's service registry.</param>
        public JamChangedHandler(IServiceRegistry serviceRegistry)
            : base(serviceRegistry)
        {
            jamRepository = RepositoryManager.GetRepository<Jam>();
            participationRepository = (ParticipationRepository) RepositoryManager.GetRepository<Participation>();

            jamRepository.EntityAdded += OnJamAdded;
            jamRepository.EntityUpdated += OnJamUpdated;
        }

        /// <summary>
        /// Removes event subscriptions to <see cref="JamRepository" /> <see cref="Entity" /> changes.
        /// </summary>
        public override void StopOnMessageChangedHandling()
        {
            jamRepository.EntityAdded -= OnJamAdded;
        }

        private void OnJamAdded(object sender, EntityChangedEventArgs<Jam> e)
        {
            var jamNotification = new EntityNotification<Jam>(e.Entity, NotificationType.Create);

            IEnumerable<int> userIds = UsersParticipatingInJam(e.Entity);

            ClientManager.SendMessageToClients(jamNotification, userIds);
        }

        private static IEnumerable<int> UsersParticipatingInJam(Jam jam)
        {
            return jam.Band.Members.Select(user => user.Id);
        }

        private void OnJamUpdated(object sender, EntityChangedEventArgs<Jam> e)
        {
            var jamNotification = new EntityNotification<Jam>(e.Entity, NotificationType.Update);

            IEnumerable<int> userIds = UsersParticipatingInJam(e.Entity);

            ClientManager.SendMessageToClients(jamNotification, userIds);
        }
    }
}