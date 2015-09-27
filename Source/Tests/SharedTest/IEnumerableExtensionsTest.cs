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
            string user1 = "Ed";
            string user2 = "Jim";
            string user3 = "Steve";
            string user4 = "Mike";
            string user5 = "Dave";

            List<string> userSetA = new List<string> {user1, user2, user3, user4, user5};
            List<string> userSetB = new List<string> {user1, user2, user3, user4};

            Assert.IsFalse(userSetA.AreSetsEqual(userSetB));
        }

        [Test]
        public void ACollectionMissingAnElementReversedReturnsFalse()
        {
            string user1 = "Ed";
            string user2 = "Jim";
            string user3 = "Steve";
            string user4 = "Mike";
            string user5 = "Dave";

            List<string> userSetA = new List<string> { user1, user2, user3, user4 };
            List<string> userSetB = new List<string> { user1, user2, user3, user4, user5 };

            Assert.IsFalse(userSetA.AreSetsEqual(userSetB));
        }

        [Test]
        public void AOneElementSetContainingTheSameElementReturnsTrue()
        {
            string user1 = "Ed";

            List<string> userSetA = new List<string> {user1};
            List<string> userSetB = new List<string> {user1};

            Assert.IsTrue(userSetA.AreSetsEqual(userSetB));
        }

        [Test]
        public void TwoCollectionsContainingTheSameElementsNotOrderedReturnsTrue()
        {
            string user1 = "Ed";
            string user2 = "Jim";
            string user3 = "Steve";
            string user4 = "Mike";
            string user5 = "Dave";

            List<string> userSetA = new List<string> {user3, user2, user1, user4, user5};
            List<string> userSetB = new List<string> {user1, user2, user3, user5, user4};

            Assert.IsTrue(userSetA.AreSetsEqual(userSetB));
        }

        [Test]
        public void TwoCollectionsContainingTheSameElementsReturnsTrue()
        {
            string band1 = "Test";
            string band2 = "Jim";
            string band3 = "Steve";
            string band4 = "Mike";
            string band5 = "Dave";

            var bandSetA = new List<string> {band1, band2, band3, band4, band5};
            var bandSetB = new List<string> {band1, band2, band3, band4, band5};

            Assert.IsTrue(bandSetA.AreSetsEqual(bandSetB));
        }

        [Test]
        public void TwoCollectionsContainingTheSameElementsReversedReturnsTrue()
        {
            string user1 = "Ed";
            string user2 = "Jim";
            string user3 = "Steve";
            string user4 = "Mike";
            string user5 = "Dave";

            var userSetA = new List<string> { user1, user2, user3, user4, user5 };
            var userSetB = new List<string> { user5, user4, user3, user2, user1 };

            Assert.IsTrue(userSetA.AreSetsEqual(userSetB));
        }

        [Test]
        public void TwoCollectionsContainingSameCountReturnsFalse()
        {
            string user1 = "Ed";
            string user2 = "Dave";

            List<string> userSetA = new List<string> { user2, user2, user2, user2, user2 };
            List<string> userSetB = new List<string> { user1, user1, user1, user1, user1 };

            Assert.IsFalse(userSetA.AreSetsEqual(userSetB));
        }


        [Test]
        public void TwoCollectionsContainingSameCountOppositeReturnsFalse()
        {
            string user1 = "Ed";
            string user2 = "Dave";

            List<string> userSetA = new List<string> { user2, user2, user2, user2, user2 };
            List<string> userSetB = new List<string> { user1, user1, user1, user1, user1 };

            Assert.IsFalse(userSetB.AreSetsEqual(userSetA));
        }


        [Test]
        public void TwoCollectionsWithSimilarDuplicateButDifferentElementsReturnsFalse()
        {
            string user1 = "Ed";
            string user2 = "Dave";
            string user3 = "Steve";

            List<string> userSetA = new List<string> { user2, user2, user3, user1 };
            List<string> userSetB = new List<string> { user3, user2, user1, user3 };

            Assert.IsFalse(userSetA.AreSetsEqual(userSetB));
        }
    }
}