using System.Collections.Generic;
using System.Linq;
using log4net;
using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Message.BandMessage;
using Shared.Repository;

namespace Server.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="BandRequest" /> the Server received.
    /// </summary>
    internal sealed class BandRequestHandler : IMessageHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (BandRequestHandler));

        /// <summary>
        /// Handles the incoming <see cref="BandRequest" />.
        /// </summary>
        /// <param name="message">The <see cref="BandRequest" /> that has been received and needs to be handled.</param>
        /// <param name="serviceRegistry">The services needed to handle the message correctly.</param>
        public void HandleMessage(IMessage message, IServiceRegistry serviceRegistry)
        {
            var entityIdAllocatorFactory = serviceRegistry.GetService<EntityIdAllocatorFactory>();

            var participationRepository = (ParticipationRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Participation>();
            var bandRepository = (IEntityRepository<Band>) serviceRegistry.GetService<RepositoryManager>().GetRepository<Band>();

            var bandRequest = (BandRequest) message;

            if (IsBandValid(bandRequest))
            {
                CreateBandEntity(bandRequest, bandRepository, participationRepository, entityIdAllocatorFactory);
            }
        }

        private static void CreateBandEntity(BandRequest bandRequest, IEntityRepository<Band> bandRepository, IEntityRepository<Participation> participationRepository, EntityIdAllocatorFactory entityIdAllocatorFactory)
        {
            var band = new Band(entityIdAllocatorFactory.AllocateEntityId<Band>(), bandRequest.Name);

            var bandParticipants = new List<Participation>();

            foreach (int userId in bandRequest.UserIds)
            {
                int participationId = entityIdAllocatorFactory.AllocateEntityId<Participation>();

                bool isLeader = userId.Equals(bandRequest.LeaderId);

                bandParticipants.Add(new Participation(participationId, userId, band.Id, isLeader));
            }

            bandParticipants.ForEach(participationRepository.AddEntity);

            bandRepository.AddEntity(band);
        }

        private static bool IsBandValid(BandRequest bandRequest)
        {
            if (bandRequest.UserIds.Count != bandRequest.UserIds.Distinct().Count())
            {
                Log.Warn("Cannot make a Band between two users of same id.");
                return false;
            }

            // TODO: Do we need more checks here? Can a system have two bands with the same users?

            return true;
        }
    }
}