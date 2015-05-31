using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Server.MessageHandler;
using Shared.Domain;
using Shared.Message;

namespace ServerTest.MessageHandlerTests
{
    [TestFixture]
    public class ParticipationSnapshotRequestHandlerTest : MessageHandlerTestFixture
    {
        public override void BeforeEachTest()
        {
            base.BeforeEachTest();

            participationSnapshotRequest = new EntitySnapshotRequest<Participation>(DefaultUser.Id);
        }

        private readonly ParticipationSnapshotRequestHandler participationSnapshotRequestHandler = new ParticipationSnapshotRequestHandler();
        private EntitySnapshotRequest<Participation> participationSnapshotRequest;

        public override void HandleMessage(IMessage message)
        {
            participationSnapshotRequestHandler.HandleMessage(message, ServiceRegistry);
        }

        [TestFixture]
        public class HandleMessageTest : ParticipationSnapshotRequestHandlerTest
        {
            [Test]
            public void ParticipationSnapshotIsForCorrectUser()
            {
                IMessage message = null;

                ConnectedUserClientHandler.MessageSent += (sender, eventArgs) => message = eventArgs.Message;

                HandleMessage(participationSnapshotRequest);

                var participationSnapshot = (EntitySnapshot<Participation>) message;

                int userId = participationSnapshot.Entities.Select(participation => participation.UserId).First();

                Assert.AreEqual(DefaultUser.Id, userId);
            }

            [Test]
            public void ParticipationSnapshotSentContainsAllJamsUserIsIn()
            {
                IMessage message = null;

                ConnectedUserClientHandler.MessageSent += (sender, eventArgs) => message = eventArgs.Message;

                HandleMessage(participationSnapshotRequest);

                var participationSnapshot = (EntitySnapshot<Participation>) message;

                List<int> jamIds = participationSnapshot.Entities.Select(participation => participation.BandId).ToList();

                Assert.AreEqual(DefaultJamIdDefaultUserIsIn, jamIds.Distinct().First());
            }

            [Test]
            public void SendsAMessage()
            {
                bool isMessageSent = false;
                ConnectedUserClientHandler.MessageSent += (sender, eventArgs) => isMessageSent = true;

                HandleMessage(participationSnapshotRequest);

                Assert.IsTrue(isMessageSent);
            }

            [Test]
            public void SendsAParticipationSnapshotMessage()
            {
                IMessage message = null;

                ConnectedUserClientHandler.MessageSent += (sender, eventArgs) => message = eventArgs.Message;

                HandleMessage(participationSnapshotRequest);

                Assert.IsTrue(message.MessageIdentifier == MessageIdentifier.ParticipationSnapshot);
            }

            [Test]
            public void ThrowsExceptionWhenGivenMessageThatIsNotParticipationSnapshot()
            {
                Assert.Throws<InvalidCastException>(() => HandleMessage(new EntitySnapshotRequest<User>(31)));
            }
        }
    }
}