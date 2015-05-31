using NUnit.Framework;
using Shared.Domain;

namespace SharedTest.DomainTest
{
    [TestFixture]
    public class UserTest
    {
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(8)]
        public void AssignCustomIdToUserTest(int userId)
        {
            var user = new User(userId, "user", new ConnectionStatus(userId, ConnectionStatus.Status.Connected));
            Assert.AreEqual(user.Id, userId);
        }

        [TestCase("Tim", "Eric", 3)]
        [TestCase("Tim", "Tim", 3)]
        public void UsersWithSameIdEqualityTest(string firstUserUsername, string secondUserUsername, int userId)
        {
            var user1 = new User(userId, firstUserUsername, new ConnectionStatus(userId, ConnectionStatus.Status.Connected));
            var user2 = new User(userId, secondUserUsername, new ConnectionStatus(userId, ConnectionStatus.Status.Connected));

            Assert.AreEqual(user1, user2);
        }

        [Test]
        public void UserEqualsTest()
        {
            const int User1Id = 1;
            const int User2Id = 2;

            var user1 = new User(User1Id, "User1", new ConnectionStatus(User1Id, ConnectionStatus.Status.Connected));

            Assert.AreEqual(user1.Id, User1Id);

            var user2 = new User(User2Id, "User2", new ConnectionStatus(User2Id, ConnectionStatus.Status.Connected));
            Assert.AreNotSame(user1.Id, user2.Id);

            Assert.IsFalse(user1.Equals(user2));
        }

        [Test]
        public void UserHashCodeTest()
        {
            var user1 = new User(1, "User", new ConnectionStatus(1, ConnectionStatus.Status.Connected));
            var user2 = new User(1, "User", new ConnectionStatus(1, ConnectionStatus.Status.Connected));

            Assert.AreEqual(user1.GetHashCode(), user2.GetHashCode());
        }

        [Test]
        public void UserHasUserNameTest()
        {
            const string Username = "user's name";
            var user = new User(1, Username, new ConnectionStatus(1, ConnectionStatus.Status.Connected));
            Assert.AreEqual(user.Username, Username);
        }

        [Test]
        public void UserReferenceEqualsTest()
        {
            var user1 = new User(1, "User", new ConnectionStatus(1, ConnectionStatus.Status.Connected));
            User user2 = user1;

            Assert.IsTrue(user1.Equals(user2));
            Assert.IsTrue(user1.Equals(user2));
            Assert.IsFalse(user1.Equals(null));

            object userObject = user1;

            Assert.IsFalse(userObject.Equals(2));
            Assert.IsFalse(userObject.Equals(null));
        }

        [Test]
        public void UsersWithSameNameHaveDifferentIdsEqualityTest()
        {
            var user1 = new User(1, "User", new ConnectionStatus(1, ConnectionStatus.Status.Connected));
            var user2 = new User(2, "User", new ConnectionStatus(2, ConnectionStatus.Status.Connected));
            Assert.AreNotEqual(user1, user2);
        }
    }
}