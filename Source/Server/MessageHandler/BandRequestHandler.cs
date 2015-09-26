using System.Collections.Generic;
using System.Linq;
using log4net;
using Shared;
using Shared.Domain;
using Shared.Message.BandMessage;
using Shared.Repository;

namespace Server.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="BandRequest" /> the Server received.
    /// </summary>
    internal sealed class BandRequestHandler : MessageHandler<BandRequest>
    {
        public BandRequestHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
        }

        /// <summary>
        /// Handles the incoming <see cref="BandRequest" />.
        /// </summary>
        /// <param name="message">The <see cref="BandRequest" /> that has been received and needs to be handled.</param>
        protected override void HandleMessage(BandRequest message)
        {
            var entityIdAllocatorFactory = ServiceRegistry.GetService<EntityIdAllocatorFactory>();

            var participationRepository = (ParticipationRepository) ServiceRegistry.GetService<RepositoryManager>().GetRepository<Participation>();
            var bandRepository = (IEntityRepository<Band>) ServiceRegistry.GetService<RepositoryManager>().GetRepository<Band>();

            if (IsBandValid(message))
            {
                CreateBandEntity(message, bandRepository, participationRepository, entityIdAllocatorFactory);
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