using System;
using System.Collections.Generic;
using log4net;
using Shared.Domain;
using Shared.Persistence;
using Shared.Repository;
using Utility;

namespace Shared
{
    /// <summary>
    /// Holds references to repositories.
    /// </summary>
    public sealed class RepositoryManager : IRepositoryManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RepositoryManager));

        private readonly PersistenceStrategy persistenceStrategy;
        private readonly IDictionary<Type, IEntityRepository> repositoriesIndexedByEnclosedEntity = new Dictionary<Type, IEntityRepository>();

        public RepositoryManager(PersistenceStrategy persistenceStrategy)
        {
            this.persistenceStrategy = persistenceStrategy;
        }

        /// <summary>
        /// The types of repositories this manager will contain.
        /// </summary>
        public List<Type> RepositoryEntityTypes { get; set; }

        public void CreateRepositories()
        {
            foreach (Type repositoryEntityType in RepositoryEntityTypes)
            {
                Type repositoryType = typeof(EntityRepository<>).FindFirstDerivedTypeWithGenericArgument(repositoryEntityType);
                var entityRepository = (IEntityRepository) Activator.CreateInstance(repositoryType);
                AddRepository(entityRepository);
            }
        }

        public void LoadRepositories()
        {
            foreach (IEntityRepository entityRepository in repositoriesIndexedByEnclosedEntity.Values)
            {
                Log.Info($"Loading {entityRepository.EnclosedEntityType} repository with existing entities.");
                entityRepository.Load();
            }

/*            Parallel.ForEach(repositoriesIndexedByEnclosedEntity.Values, entityRepository =>
            {
                Log.Info($"Loading {entityRepository.EnclosedEntityType} repository with existing entities.");
                entityRepository.Load();
            });*/
        }

        /// <summary>
        /// Get a repository from the <see cref="RepositoryManager" />.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Entity" /> that the repository holds.</typeparam>
        /// <returns>A readonly version of the repository requested. If no repository is found, return null.</returns>
        public IReadOnlyEntityRepository<T> GetRepository<T>() where T : Entity
        {
            repositoriesIndexedByEnclosedEntity.TryGetValue(typeof(T), out var repository);
            return (IReadOnlyEntityRepository<T>) repository;
        }

        public void FlushAll()
        {
            foreach (IEntityRepository entityRepository in repositoriesIndexedByEnclosedEntity.Values)
            {
                entityRepository.DeleteAll();
            }
        }

        /// <summary>
        /// Add a repository to the <see cref="RepositoryManager" />.
        /// </summary>
        /// <param name="repository">The repository instance to add.</param>
        private void AddRepository(IEntityRepository repository)
        {
            repository.RepositoryManager = this;
            Type entityType = repository.EnclosedEntityType;

            IEntityPersister persister;

            switch (persistenceStrategy)
            {
                case PersistenceStrategy.Database:
                    Type entityMapperType = typeof(EntityMapper<>).FindFirstDerivedTypeWithGenericArgument(entityType);
                    object entityMapper = Activator.CreateInstance(entityMapperType);
                    Type databasePersisterType = typeof(DatabaseEntityPersister<>).MakeGenericType(entityType);
                    persister = (IEntityPersister) Activator.CreateInstance(databasePersisterType, entityMapper);
                    break;
                case PersistenceStrategy.InMemory:
                    Type inMemoryEntityPersisterType = typeof(InMemoryEntityPersister<>).MakeGenericType(entityType);
                    persister = (IEntityPersister) Activator.CreateInstance(inMemoryEntityPersisterType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            repository.EntityPersister = persister;
            repositoriesIndexedByEnclosedEntity.Add(repository.EnclosedEntityType, repository);
        }
    }
}