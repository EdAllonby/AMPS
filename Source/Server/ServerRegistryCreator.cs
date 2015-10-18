using Server.EntityIdGenerator;
using Server.Persistence;
using Shared;
using Shared.Configuration;
using Shared.Domain;
using Shared.Repository;

namespace Server
{
    /// <summary>
    /// Creates a <see cref="IServiceRegistry" /> for the <see cref="Server" />.
    /// </summary>
    public static class ServerRegistryCreator
    {
        /// <summary>
        /// Creates a new <see cref="IServiceRegistry" /> instance.
        /// </summary>
        /// <returns>A <see cref="IServiceRegistry" /> instance.</returns>
        public static IServiceRegistry RegisterServices(bool useDatabasePersistence)
        {
            IServiceRegistry serviceRegistry = new ServiceRegistry();

            RepositoryManager repositoryManager = CreateRepositoryManager(useDatabasePersistence);

            serviceRegistry.RegisterService<RepositoryManager>(repositoryManager);
            serviceRegistry.RegisterService<IClientManager>(new ClientManager());

            if (useDatabasePersistence)
            {
                serviceRegistry.RegisterService<EntityIdAllocatorFactory>(new EntityIdAllocatorFactory(new GeneratorFromDatabase()));
                serviceRegistry.RegisterService<PasswordHandler>(new PasswordHandler(new DatabasePasswordPersister()));
            }
            else
            {
                serviceRegistry.RegisterService<EntityIdAllocatorFactory>(new EntityIdAllocatorFactory(new NewGenerator()));
                serviceRegistry.RegisterService<PasswordHandler>(new PasswordHandler(new InMemoryPasswordPersister()));
            }

            serviceRegistry.RegisterService<JamManager>(new JamManager(repositoryManager.GetRepository<Jam>()));

            serviceRegistry.RegisterService<AppConfigManager>(new AppConfigManager(new AppConfiguration()));

            return serviceRegistry;
        }

        private static RepositoryManager CreateRepositoryManager(bool useDatabasePersistence)
        {
            var repositoryManager = new RepositoryManager();

            if (useDatabasePersistence)
            {
                CreateDatabaseRepositories(repositoryManager);
            }
            else
            {
                CreateInMemoryRepositories(repositoryManager);
            }

            return repositoryManager;
        }

        private static void CreateInMemoryRepositories(RepositoryManager repositoryManager)
        {
            repositoryManager.AddRepository<User>(new UserRepository(new InMemoryEntityPersister<User>()));
            repositoryManager.AddRepository<Participation>(new ParticipationRepository(new InMemoryEntityPersister<Participation>()));
            repositoryManager.AddRepository<Band>(new BandRepository(new InMemoryEntityPersister<Band>()));
            repositoryManager.AddRepository<Jam>(new JamRepository(new InMemoryEntityPersister<Jam>()));
            repositoryManager.AddRepository<Task>(new TaskRepository(new InMemoryEntityPersister<Task>()));
        }

        private static void CreateDatabaseRepositories(RepositoryManager repositoryManager)
        {
            repositoryManager.AddRepository<User>(new UserRepository(new DatabaseEntityPersister<User>(new UserMapper())));
            repositoryManager.AddRepository<Participation>(new ParticipationRepository(new DatabaseEntityPersister<Participation>(new ParticipationMapper())));
            repositoryManager.AddRepository<Band>(new BandRepository(new DatabaseEntityPersister<Band>(new BandMapper())));
            repositoryManager.AddRepository<Jam>(new JamRepository(new DatabaseEntityPersister<Jam>(new JamMapper())));
            repositoryManager.AddRepository<Task>(new TaskRepository(new DatabaseEntityPersister<Task>(new TaskMapper())));
        }
    }
}