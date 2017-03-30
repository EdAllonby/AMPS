using System.Collections.Generic;
using NUnit.Framework;
using Shared;

namespace SharedTest
{
    [TestFixture]
    public sealed class EnumerableExtensionsTest
    {
        [Test]
        public void ACollectionMissingAnElementReturnsFalse()
        {
            var user1 = "Ed";
            var user2 = "Jim";
            var user3 = "Steve";
            var user4 = "Mike";
            var user5 = "Dave";

            var userSetA = new List<string> { user1, user2, user3, user4, user5 };
            var userSetB = new List<string> { user1, user2, user3, user4 };

            Assert.IsFalse(userSetA.AreSetsEqual(userSetB));
        }

        [Test]
        public void ACollectionMissingAnElementReversedReturnsFalse()
        {
            var user1 = "Ed";
            var user2 = "Jim";
            var user3 = "Steve";
            var user4 = "Mike";
            var user5 = "Dave";

            var userSetA = new List<string> { user1, user2, user3, user4 };
            var userSetB = new List<string> { user1, user2, user3, user4, user5 };

            Assert.IsFalse(userSetA.AreSetsEqual(userSetB));
        }

        [Test]
        public void AOneElementSetContainingTheSameElementReturnsTrue()
        {
            var user1 = "Ed";

            var userSetA = new List<string> { user1 };
            var userSetB = new List<string> { user1 };

            Assert.IsTrue(userSetA.AreSetsEqual(userSetB));
        }


        [Test]
        public void TwoCollectionsContainingSameCountOppositeReturnsFalse()
        {
            var user1 = "Ed";
            var user2 = "Dave";

            var userSetA = new List<string> { user2, user2, user2, user2, user2 };
            var userSetB = new List<string> { user1, user1, user1, user1, user1 };

            Assert.IsFalse(userSetB.AreSetsEqual(userSetA));
        }

        [Test]
        public void TwoCollectionsContainingSameCountReturnsFalse()
        {
            var user1 = "Ed";
            var user2 = "Dave";

            var userSetA = new List<string> { user2, user2, user2, user2, user2 };
            var userSetB = new List<string> { user1, user1, user1, user1, user1 };

            Assert.IsFalse(userSetA.AreSetsEqual(userSetB));
        }

        [Test]
        public void TwoCollectionsContainingTheSameElementsNotOrderedReturnsTrue()
        {
            var user1 = "Ed";
            var user2 = "Jim";
            var user3 = "Steve";
            var user4 = "Mike";
            var user5 = "Dave";

            var userSetA = new List<string> { user3, user2, user1, user4, user5 };
            var userSetB = new List<string> { user1, user2, user3, user5, user4 };

            Assert.IsTrue(userSetA.AreSetsEqual(userSetB));
        }

        [Test]
        public void TwoCollectionsContainingTheSameElementsReturnsTrue()
        {
            var band1 = "Test";
            var band2 = "Jim";
            var band3 = "Steve";
            var band4 = "Mike";
            var band5 = "Dave";

            var bandSetA = new List<string> { band1, band2, band3, band4, band5 };
            var bandSetB = new List<string> { band1, band2, band3, band4, band5 };

            Assert.IsTrue(bandSetA.AreSetsEqual(bandSetB));
        }

        [Test]
        public void TwoCollectionsContainingTheSameElementsReversedReturnsTrue()
        {
            var user1 = "Ed";
            var user2 = "Jim";
            var user3 = "Steve";
            var user4 = "Mike";
            var user5 = "Dave";

            var userSetA = new List<string> { user1, user2, user3, user4, user5 };
            var userSetB = new List<string> { user5, user4, user3, user2, user1 };

            Assert.IsTrue(userSetA.AreSetsEqual(userSetB));
        }


        [Test]
        public void TwoCollectionsWithSimilarDuplicateButDifferentElementsReturnsFalse()
        {
            var user1 = "Ed";
            var user2 = "Dave";
            var user3 = "Steve";

            var userSetA = new List<string> { user2, user2, user3, user1 };
            var userSetB = new List<string> { user3, user2, user1, user3 };

            Assert.IsFalse(userSetA.AreSetsEqual(userSetB));
        }
    }
}