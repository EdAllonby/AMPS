using System;
using System.Collections.Generic;
using Shared.Domain;
using Shared.Repository;

namespace Shared
{
    public interface IRepositoryManager : IService
    {
        List<Type> RepositoryEntityTypes { get; set; }

        void CreateRepositories();
        void FlushAll();
        IReadOnlyEntityRepository<T> GetRepository<T>() where T : Entity;
        void LoadRepositories();
    }
}