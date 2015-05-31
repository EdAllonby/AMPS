using NUnit.Framework;
using Shared.Domain;

namespace SharedTest.DomainTest
{
    [TestFixture]
    public class EntityTest
    {
        [Test]
        public void EntityWithIdHasId()
        {
            const int IdToSet = 3;
            Entity user = new User(IdToSet, "TestNewUser", new ConnectionStatus(IdToSet, ConnectionStatus.Status.Connected));

            Assert.AreEqual(IdToSet, user.Id);
        }

        [Test]
        public void EntityWithIdHasIsNewFlagFalse()
        {
            const int IdToSet = 3;
            Entity user = new User(IdToSet, "TestNewUser", new ConnectionStatus(IdToSet, ConnectionStatus.Status.Connected));

            Assert.IsFalse(user.IsNew);
        }

        [Test]
        public void NewEntitySetsIsNewFlag()
        {
            Entity user = new User("TestNewUser");

            Assert.IsTrue(user.IsNew);
        }
    }
}