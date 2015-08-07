using System.Collections.Generic;
using System.Linq;
using log4net;
using Shared;
using Shared.Domain;
using Shared.Message.ParticipationMessage;
using Shared.Repository;

namespace Server.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="ParticipationRequest" /> the Server received.
    /// </summary>
    internal sealed class ParticipationRequestHandler : MessageHandler<ParticipationRequest>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Server));

        /// <summary>
        /// Handles the incoming <see cref="ParticipationRequest" />.
        /// </summary>
        /// <param name="message">The <see cref="ParticipationRequest" /> that has been received and needs to be handled.</param>
        /// <param name="serviceRegistry">The services needed to handle the message correctly.</param>
        public override void HandleMessage(ParticipationRequest message, IServiceRegistry serviceRegistry)
        {
            var participationRepository = (ParticipationRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Participation>();

            if (CheckUserCanEnterBand(message, participationRepository))
            {
                var entityIdAllocatorFactory = serviceRegistry.GetService<EntityIdAllocatorFactory>();
                AddUserToBand(message, entityIdAllocatorFactory, participationRepository);
            }
        }

        private static bool CheckUserCanEnterBand(ParticipationRequest participationRequest, ParticipationRepository participationRepository)
        {
            Participation newparticipation = participationRequest.Participation;

            List<Participation> currentParticipantsInBand = participationRepository.GetParticipationsByBandId(newparticipation.BandId);

            if (currentParticipantsInBand.Any(participation => participation.UserId == newparticipation.UserId))
            {
                Log.WarnFormat(
                    "User with id {0} cannot be added to band with Id {1}, user already exists in this Band.",
                    participationRequest.Participation.UserId, participationRequest.Participation.BandId);

                return false;
            }

            return true;
        }

        private static void AddUserToBand(ParticipationRequest participationRequest, EntityIdAllocatorFactory entityIdAllocatorFactory, IEntityRepository<Participation> participationRepository)
        {
            int participationId = entityIdAllocatorFactory.AllocateEntityId<Participation>();

            var participation = new Participation(participationId, participationRequest.Participation.UserId, participationRequest.Participation.BandId, participationRequest.Participation.IsLeader);

            participationRepository.AddEntity(participation);
        }
    }
}