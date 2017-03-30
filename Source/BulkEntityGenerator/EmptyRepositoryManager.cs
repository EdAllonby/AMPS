using System;
using System.Collections.Generic;
using Shared;
using Shared.Domain;
using Shared.Repository;

namespace BulkEntityGenerator
{
    public class EmptyRepositoryManager : IRepositoryManager
    {
        private readonly IDictionary<Type, IEntityRepository> repositoriesIndexedByEnclosedEntity = new Dictionary<Type, IEntityRepository>();
        public List<Type> RepositoryEntityTypes { get; set; }


        public void CreateRepositories()
        {
            foreach (Type repositoryEntityType in RepositoryEntityTypes)
            {
                var entityRepository = (IEntityRepository) Activator.CreateInstance(typeof(EmptyEntityRepository<>).MakeGenericType(repositoryEntityType));
                AddRepository(entityRepository);
            }
        }

        public void FlushAll()
        {
        }

        /// <summary>
        /// Get a repository from the <see cref="RepositoryManager" />.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Entity" /> that the repository holds.</typeparam>
        /// <returns>A readonly version of the repository requested. If no repository is found, return null.</returns>
        public IReadOnlyEntityRepository<T> GetRepository<T>() where T : Entity
        {
            IEntityRepository repository;
            repositoriesIndexedByEnclosedEntity.TryGetValue(typeof(T), out repository);
            return (IReadOnlyEntityRepository<T>) repository;
        }

        public void LoadRepositories()
        {
        }

        /// <summary>
        /// Add a repository to the <see cref="RepositoryManager" />.
        /// </summary>
        /// <param name="repository">The repository instance to add.</param>
        private void AddRepository(IEntityRepository repository)
        {
            repositoriesIndexedByEnclosedEntity.Add(repository.EnclosedEntityType, repository);
        }
    }
}