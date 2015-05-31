using NUnit.Framework;
using Shared.Domain;

namespace SharedTest.DomainTest
{
    [TestFixture]
    public class ParticipationTest
    {
        [Test]
        public void CreateIncompleteParticipationTest()
        {
            const int UserId = 1;
            const int BandId = 2;
            const bool IsLeader = true;

            var incompleteParticipation = new Participation(UserId, BandId, IsLeader);
            Assert.IsTrue(incompleteParticipation.IsNew);
            Assert.AreEqual(incompleteParticipation.UserId, UserId);
            Assert.AreEqual(incompleteParticipation.BandId, BandId);
            Assert.AreEqual(incompleteParticipation.IsLeader, IsLeader);
        }

        [Test]
        public void ParticipationEqualsTest()
        {
            const int ParticipationId = 1;
            const int UserId = 1;
            const int BandId = 2;
            const bool IsLeader = true;

            var participation1 = new Participation(ParticipationId, UserId, BandId, IsLeader);
            var participation2 = new Participation(ParticipationId, UserId, BandId, IsLeader);

            Assert.AreEqual(participation1, participation2);
            Assert.IsTrue(participation1.Equals((object) participation2));

            participation1 = new Participation(2, 1, 4, false);
            Assert.AreNotEqual(participation1, participation2);
        }

        [Test]
        public void ParticipationHashCodeTest()
        {
            const int ParticipationId = 1;
            const int UserId = 1;
            const int BandId = 2;
            const bool IsLeader = true;

            var participation = new Participation(ParticipationId, UserId, BandId, IsLeader);
            var participation2 = new Participation(ParticipationId, UserId, BandId, IsLeader);
            Assert.AreEqual(participation.GetHashCode(), participation2.GetHashCode());
        }

        [Test]
        public void ParticipationHoldsInformationCorrectly()
        {
            const int ParticipationId = 1;
            const int UserId = 1;
            const int BandId = 2;
            const bool IsLeader = false;

            var participation = new Participation(ParticipationId, UserId, BandId, IsLeader);
            Assert.AreEqual(participation.UserId, UserId);
            Assert.AreEqual(participation.BandId, BandId);
            Assert.AreEqual(participation.IsLeader, IsLeader);
        }

        [Test]
        public void ParticipationReferenceEqualsTest()
        {
            const int ParticipationId = 1;
            const int UserId = 1;
            const int BandId = 2;
            const bool IsLeader = false;

            var participation1 = new Participation(ParticipationId, UserId, BandId, IsLeader);
            Participation participation2 = participation1;

            Assert.IsTrue(participation1.Equals(participation2));
            Assert.IsTrue(participation1.Equals((object) participation2));
            Assert.IsFalse(participation1.Equals(null));

            object participationObject = participation1;

            Assert.IsFalse(participationObject.Equals(2));
            Assert.IsFalse(participationObject.Equals(null));
        }
    }
}