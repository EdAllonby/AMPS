using System;
using System.Collections.Generic;
using System.Linq;
using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Server.EntityChangedHandler
{
    /// <summary>
    /// Handles <see cref="Entity" /> changes to the <see cref="BandRepository" />.
    /// </summary>
    internal sealed class BandChangedHandler : EntityChangedHandler
    {
        private readonly IReadOnlyEntityRepository<Band> bandRepository;

        /// <summary>
        /// Creates a new <see cref="TaskRepository" /> <see cref="Task" /> changed handler and wires up the change events.
        /// </summary>
        /// <param name="serviceRegistry">The server's service registry.</param>
        public BandChangedHandler(IServiceRegistry serviceRegistry)
            : base(serviceRegistry)
        {
            bandRepository = RepositoryManager.GetRepository<Band>();

            bandRepository.EntityAdded += OnBandAdded;
            bandRepository.EntityUpdated += OnBandUpdated;
        }

        /// <summary>
        /// Removes event subscriptions to <see cref="BandRepository" /> <see cref="Entity" /> changes.
        /// </summary>
        public override void StopOnMessageChangedHandling()
        {
            bandRepository.EntityAdded -= OnBandAdded;
            bandRepository.EntityUpdated -= OnBandUpdated;
        }

        private void OnBandAdded(object sender, EntityChangedEventArgs<Band> e)
        {
            var bandNotification = new EntityNotification<Band>(e.Entity, NotificationType.Create);

            IEnumerable<int> bandMembers = bandNotification.Entity.Members.Select(user => user.Id);

            ClientManager.SendMessageToClients(bandNotification, bandMembers);
        }

        private void OnBandUpdated(object sender, EntityChangedEventArgs<Band> e)
        {
            throw new NotImplementedException();
        }
    }
}