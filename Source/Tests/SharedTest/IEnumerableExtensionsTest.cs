using System.Collections.Generic;
using NUnit.Framework;
using Shared;
using Shared.Domain;

namespace SharedTest
{
    [TestFixture]
    public sealed class IEnumerableExtensionsTest
    {
        [Test]
        public void TwoCollectionsContainingTheSameElementsReturnsTrue()
        {
            User user1 = new User("Ed");
            User user2 = new User("Jim");
            User user3 = new User("Steve");
            User user4 = new User("Mike");
            User user5 = new User("Dave");

            List<User> userSetA = new List<User> {user1, user2, user3, user4, user5};
            List<User> userSetB = new List<User> {user1, user2, user3, user4, user5};

            Assert.IsTrue(userSetA.HasSameElementsAs(userSetB));
        }

        [Test]
        public void TwoCollectionsContainingTheSameElementsNotOrderedReturnsTrue()
        {
            User user1 = new User("Ed");
            User user2 = new User("Jim");
            User user3 = new User("Steve");
            User user4 = new User("Mike");
            User user5 = new User("Dave");

            List<User> userSetA = new List<User> { user3, user2, user1, user4, user5 };
            List<User> userSetB = new List<User> { user1, user2, user3, user5, user4 };

            Assert.IsTrue(userSetA.HasSameElementsAs(userSetB));
        }

        [Test]
        public void ACollectionMissingAnElementReturnsFalse()
        {
            User user1 = new User("Ed");
            User user2 = new User("Jim");
            User user3 = new User("Steve");
            User user4 = new User("Mike");
            User user5 = new User("Dave");

            List<User> userSetA = new List<User> { user1, user2, user3, user4, user5 };
            List<User> userSetB = new List<User> { user1, user2, user3, user4 };

            Assert.IsFalse(userSetA.HasSameElementsAs(userSetB));
        }

        [Test]
        public void AOneElementSetContainingTheSameElementReturnsTrue()
        {
            User user1 = new User("Ed");

            List<User> userSetA = new List<User> { user1 };
            List<User> userSetB = new List<User> { user1 };

            Assert.IsTrue(userSetA.HasSameElementsAs(userSetB));
        }
    }
}