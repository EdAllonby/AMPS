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
        public void ReturnsNullWhenRepositoryCanNotBeFound()
        {
            var repositoryManager = new RepositoryManager(PersistenceStrategy.InMemory);

            Assert.IsNull(repositoryManager.GetRepository<Task>());
        }
    }
}