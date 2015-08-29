﻿using System.Collections.Generic;
using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Server.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="EntitySnapshotRequest{T}" /> the Server received.
    /// </summary>
    internal sealed class ParticipationSnapshotRequestHandler : MessageHandler<EntitySnapshotRequest<Participation>>
    {
        public ParticipationSnapshotRequestHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
        }

        /// <summary>
        /// Handles the incoming <see cref="EntitySnapshotRequest{Participation}" />.
        /// </summary>
        /// <param name="message">
        /// The <see cref="EntitySnapshotRequest{Participation}" /> that has been received and needs to be handled.
        /// </param>
        public override void HandleMessage(EntitySnapshotRequest<Participation> message)
        {
            var participationRepository = (ParticipationRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Participation>();
            var clientManager = serviceRegistry.GetService<IClientManager>();

            var userParticipations = new List<Participation>();

            foreach (int bandId in participationRepository.GetAllBandIdsByUserId(message.UserId))
            {
                userParticipations.AddRange(participationRepository.GetParticipationsByBandId(bandId));
            }

            var participationSnapshot = new EntitySnapshot<Participation>(userParticipations);

            clientManager.SendMessageToClient(participationSnapshot, message.UserId);
        }
    }
}