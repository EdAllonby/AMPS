using System;
using System.Collections.Generic;
using NUnit.Framework;
using Server;
using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace ServerTest.MessageHandlerTests
{
    public abstract class MessageHandlerTestFixture
    {
        protected int DefaultBandIdDefaultUserIsIn;
        protected int DefaultJamIdDefaultUserIsIn;
        protected User DefaultUser;
        protected MockClientHandler ConnectedUserClientHandler { get; private set; }
        protected IServiceRegistry ServiceRegistry { get; private set; }

        [SetUp]
        public virtual void BeforeEachTest()
        {
            ServiceRegistry = new ServiceRegistry();

            var entityIdAllocatorFactory = new EntityIdAllocatorFactory(new NewGenerator());

            int userId = entityIdAllocatorFactory.AllocateEntityId<User>();
            DefaultUser = new User(userId, "user", new ConnectionStatus(userId, ConnectionStatus.Status.Connected));

            ServiceRegistry.RegisterService<EntityIdAllocatorFactory>(entityIdAllocatorFactory);

            PopulateRepositoryManager(entityIdAllocatorFactory);
            PopulateClientManager();
        }

        private void PopulateClientManager()
        {
            IReadOnlyEntityRepository<User> userRepository = ServiceRegistry.GetService<RepositoryManager>().GetRepository<User>();

            var clientManager = new ClientManager();

            foreach (User user in userRepository.GetAllEntities())
            {
                if (user.Id == DefaultUser.Id)
                {
                    ConnectedUserClientHandler = new MockClientHandler();

                    clientManager.AddClientHandler(user.Id, ConnectedUserClientHandler);
                }
                else
                {
                    clientManager.AddClientHandler(user.Id, new MockClientHandler());
                }
            }

            ServiceRegistry.RegisterService<IClientManager>(clientManager);
        }

        private void PopulateRepositoryManager(EntityIdAllocatorFactory idAllocator)
        {
            var repositoryManager = new RepositoryManager();

            repositoryManager.AddRepository<User>(new UserRepository(new InMemoryEntityPersister<User>()));
            repositoryManager.AddRepository<Jam>(new JamRepository(new InMemoryEntityPersister<Jam>()));
            repositoryManager.AddRepository<Band>(new BandRepository(new InMemoryEntityPersister<Band>()));
            repositoryManager.AddRepository<Participation>(new ParticipationRepository(new InMemoryEntityPersister<Participation>()));

            ServiceRegistry.RegisterService<RepositoryManager>(repositoryManager);

            int userId2 = idAllocator.AllocateEntityId<User>();
            int userId3 = idAllocator.AllocateEntityId<User>();

            var userRepository = (UserRepository) repositoryManager.GetRepository<User>();

            var usersToAddToBand = new List<int> {DefaultUser.Id, userId2, userId3};

            foreach (int userId in usersToAddToBand)
            {
                var user = new User(userId, "user" + userId, new ConnectionStatus(userId, ConnectionStatus.Status.Connected));
                userRepository.AddEntity(user);
            }

            DefaultBandIdDefaultUserIsIn = SetUpMultiUserBand(usersToAddToBand, repositoryManager, idAllocator);
            DefaultJamIdDefaultUserIsIn = CreateJamForNewBand(DefaultBandIdDefaultUserIsIn, repositoryManager, idAllocator);
        }

        private static int SetUpMultiUserBand(IEnumerable<int> userIds, RepositoryManager repositoryManager, EntityIdAllocatorFactory idAllocator)
        {
            var bandRepository = (BandRepository) repositoryManager.GetRepository<Band>();
            var participationRepository = (ParticipationRepository) repositoryManager.GetRepository<Participation>();

            int bandId = idAllocator.AllocateEntityId<Band>();

            var band = new Band(bandId, "Band1");
            bandRepository.AddEntity(band);

            const int leaderId = 1;
            bool isLeader = false;

            foreach (int userId in userIds)
            {
                if (leaderId == userId)
                {
                    isLeader = true;
                }

                var participation = new Participation(idAllocator.AllocateEntityId<Participation>(), userId, band.Id, isLeader);
                participationRepository.AddEntity(participation);
            }

            return bandId;
        }

        private static int CreateJamForNewBand(int bandId, RepositoryManager repositoryManager, EntityIdAllocatorFactory idAllocator)
        {
            var jamId = idAllocator.AllocateEntityId<Jam>();
            var jam = new Jam(jamId, bandId, DateTime.UtcNow.AddDays(5));

            var jamRepository = (JamRepository) repositoryManager.GetRepository<Jam>();
            jamRepository.AddEntity(jam);

            return jamId;
        }

        public abstract void HandleMessage(IMessage message);
    }
}