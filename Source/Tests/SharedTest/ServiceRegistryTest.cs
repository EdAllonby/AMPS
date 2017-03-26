using System;
using NUnit.Framework;
using Shared;
using Shared.Repository;

namespace SharedTest
{
    [TestFixture]
    public sealed class ServiceRegistryTest
    {
        [Test]
        public void CanRegisterService()
        {
            var serviceRegistry = new ServiceRegistry();

            var repositoryManager = new RepositoryManager(PersistenceStrategy.InMemory);

            serviceRegistry.RegisterService<IRepositoryManager>(repositoryManager);

            Assert.AreEqual(serviceRegistry.GetService<IRepositoryManager>(), repositoryManager);
        }

        [Test]
        public void ThrowsAnArgumentExceptionWhenServiceNotFound()
        {
            var serviceRegistry = new ServiceRegistry();

            Assert.Throws<ArgumentException>(() => serviceRegistry.GetService<IRepositoryManager>());
        }
    }
}