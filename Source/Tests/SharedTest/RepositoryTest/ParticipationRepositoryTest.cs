using System.Collections.Generic;
using NUnit.Framework;
using Shared.Domain;
using Shared.Repository;

namespace SharedTest.RepositoryTest
{
    [TestFixture]
    public class ParticipationRepositoryTest
    {
        [Test]
        public void AddParticipationsTest()
        {
            var participationRepository = new ParticipationRepository { EntityPersister = new InMemoryEntityPersister<Participation>() };

            const int BandId = 1;

            IList<Participation> participations = new List<Participation>
            {
                new(1, 1, BandId, true),
                new(2, 2, BandId, false)
            };

            foreach (Participation participation in participations)
            {
                participationRepository.AddEntity(participation);
            }

            Assert.AreEqual(participations, participationRepository.GetAllEntities());
        }

        [Test]
        public void AddParticipationTest()
        {
            var participationRepository = new ParticipationRepository { EntityPersister = new InMemoryEntityPersister<Participation>() };

            const bool IsLeader = true;
            const int ParticipationId = 1;
            var participation = new Participation(ParticipationId, 1, 1, IsLeader);
            participationRepository.AddEntity(participation);
            Assert.AreEqual(participation, participationRepository.FindEntityById(ParticipationId));
        }

        [Test]
        public void DoesBandWithUsersExistTest()
        {
            var participationRepository = new ParticipationRepository { EntityPersister = new InMemoryEntityPersister<Participation>() };
            var participation = new Participation(1, 1, 1, true);
            participationRepository.AddEntity(participation);

            Assert.True(participationRepository.DoesBandWithUsersExist(new List<int> { participation.User.Id }));
        }

        [Test]
        public void GetAllParticipationsTest()
        {
            var participationRepository = new ParticipationRepository { EntityPersister = new InMemoryEntityPersister<Participation>() };

            const int BandId = 1;
            IList<Participation> participations = new List<Participation>
            {
                new(1, 1, BandId, false),
                new(2, 2, BandId, true),
                new(3, 3, BandId, false),
                new(4, 5, BandId, false)
            };
            foreach (Participation participation in participations)
            {
                participationRepository.AddEntity(participation);
            }

            Assert.AreEqual(participations, participationRepository.GetAllEntities());
        }

        [Test]
        public void GetParticipationsByBandIdTest()
        {
            const int BandId = 10;

            var participationRepository = new ParticipationRepository { EntityPersister = new InMemoryEntityPersister<Participation>() };
            var participation1 = new Participation(1, 1, BandId, false);
            var participation2 = new Participation(2, 2, BandId, true);

            participationRepository.AddEntity(participation1);
            participationRepository.AddEntity(participation2);

            var expectedParticipations = new List<Participation> { participation1, participation2 };

            IEnumerable<Participation> actualParticipations =
                participationRepository.GetParticipationsByBandId(BandId);

            Assert.AreEqual(expectedParticipations, actualParticipations);
        }
    }
}