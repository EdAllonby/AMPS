using System;
using NUnit.Framework;
using Shared;

namespace SharedTest
{
    [TestFixture]
    public sealed class ServiceRegistryTest
    {
        [Test]
        public void CanRegisterService()
        {
            ServiceRegistry serviceRegistry = new ServiceRegistry();

            RepositoryManager repositoryManager = new RepositoryManager();

            serviceRegistry.RegisterService<RepositoryManager>(repositoryManager);

            Assert.AreEqual(serviceRegistry.GetService<RepositoryManager>(), repositoryManager);
        }

        [Test]
        public void ThrowsAnArgumentExceptionWhenServiceNotFound()
        {
            ServiceRegistry serviceRegistry = new ServiceRegistry();

            Assert.Throws<ArgumentException>(() => serviceRegistry.GetService<RepositoryManager>());
        }
    }
}