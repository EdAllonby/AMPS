using System;
using System.Collections.Generic;
using Server.EntityIdGenerator;
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

            serviceRegistry.RegisterService<IRepositoryManager>(repositoryManager);
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
            PersistenceStrategy persistenceStrategy = useDatabasePersistence ? PersistenceStrategy.Database : PersistenceStrategy.InMemory;

            var repositoryManager = new RepositoryManager(persistenceStrategy)
            {
                RepositoryEntityTypes = new List<Type> { typeof(User), typeof(Participation), typeof(Band), typeof(Jam), typeof(Task), typeof(TaskComment) }
            };

            repositoryManager.CreateRepositories();
            repositoryManager.LoadRepositories();
            return repositoryManager;
        }
    }
}