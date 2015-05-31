using NUnit.Framework;
using Shared;
using Shared.Domain;
using Shared.Repository;

namespace SharedTest
{
    [TestFixture]
    public sealed class RepositoryManagerTest
    {
        [Test]
        public void CanAddARepository()
        {
            RepositoryManager repositoryManager = new RepositoryManager();

            IEntityRepository entityRepository = new TaskRepository(new InMemoryEntityPersister<Task>());

            repositoryManager.AddRepository<Task>(entityRepository);

            Assert.AreEqual(repositoryManager.GetRepository<Task>(), entityRepository);
        }

        [Test]
        public void ReturnsNullWhenRepositoryCanNotBeFound()
        {
            RepositoryManager repositoryManager = new RepositoryManager();

            Assert.IsNull(repositoryManager.GetRepository<Task>());
        }
    }
}