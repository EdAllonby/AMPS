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
    public class BandSnapshotRequestHandlerTest : MessageHandlerTestFixture
    {
        public override void BeforeEachTest()
        {
            base.BeforeEachTest();

            bandSnapshotRequest = new EntitySnapshotRequest<Band>(DefaultUser.Id);
        }

        private readonly BandSnapshotRequestHandler bandSnapshotRequestHandler = new BandSnapshotRequestHandler();
        private EntitySnapshotRequest<Band> bandSnapshotRequest;

        public override void HandleMessage(IMessage message)
        {
            bandSnapshotRequestHandler.HandleMessage(message, ServiceRegistry);
        }

        [TestFixture]
        public class HandleMessageTest : BandSnapshotRequestHandlerTest
        {
            [Test]
            public void BandSnapshotSentContainsAllBandsUserIsIn()
            {
                IMessage message = null;

                ConnectedUserClientHandler.MessageSent += (sender, eventArgs) => message = eventArgs.Message;

                HandleMessage(bandSnapshotRequest);

                var bandSnapshot = (EntitySnapshot<Band>) message;

                List<int> bandIds = bandSnapshot.Entities.Select(band => band.Id).ToList();

                Assert.AreEqual(DefaultBandIdDefaultUserIsIn, bandIds.Distinct().First());
            }

            [Test]
            public void SendsABandSnapshotMessage()
            {
                IMessage message = null;

                ConnectedUserClientHandler.MessageSent += (sender, eventArgs) => message = eventArgs.Message;

                HandleMessage(bandSnapshotRequest);

                Assert.IsTrue(message.MessageIdentifier == MessageIdentifier.BandSnapshot);
            }

            [Test]
            public void SendsAMessage()
            {
                bool isMessageSent = false;
                ConnectedUserClientHandler.MessageSent += (sender, eventArgs) => isMessageSent = true;

                HandleMessage(bandSnapshotRequest);

                Assert.IsTrue(isMessageSent);
            }

            [Test]
            public void ThrowsExceptionWhenMessageIsNotABandSnapshotRequest()
            {
                var participationSnapshotRequest = new EntitySnapshotRequest<Participation>(DefaultUser.Id);
                Assert.Throws<InvalidCastException>(() => HandleMessage(participationSnapshotRequest));
            }
        }
    }
}