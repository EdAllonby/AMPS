﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Server.MessageHandler;
using Shared.Domain;
using Shared.Message;

namespace ServerTest.MessageHandlerTests
{
    [TestFixture]
    public class JamSnaphotRequestHandlerTest : MessageHandlerTestFixture
    {
        public override void BeforeEachTest()
        {
            base.BeforeEachTest();

            jamSnapshotRequestHandler = new JamSnapshotRequestHandler(ServiceRegistry);

            jamSnapshotRequest = new EntitySnapshotRequest<Jam>(DefaultUser.Id);
        }

        private JamSnapshotRequestHandler jamSnapshotRequestHandler;
        private EntitySnapshotRequest<Jam> jamSnapshotRequest;

        public override void HandleMessage(IMessage message)
        {
            jamSnapshotRequestHandler.HandleMessage(message);
        }

        [TestFixture]
        public class HandleMessageTest : JamSnaphotRequestHandlerTest
        {
            [Test]
            public void CanManuallySetJamToInActiveIfAfterActivePeriod()
            {
                DateTime jamEndDate = DateTime.UtcNow.AddMinutes(-60);
                var jam = new Jam(1, 1, jamEndDate) { IsActive = false };

                Assert.IsFalse(jam.IsActive);
            }

            [Test]
            public void JamEndDateTest()
            {
                var jamEndDate = new DateTime(1, 1, 1);
                var jam = new Jam(1, 1, jamEndDate);

                Assert.AreEqual(jam.JamEndDate, jamEndDate);
            }

            [Test]
            public void JamIsActiveBeforeSpecifiedEndDateTest()
            {
                DateTime jamEndDate = DateTime.UtcNow.AddMinutes(60);
                var jam = new Jam(1, 1, jamEndDate);

                Assert.IsTrue(jam.IsActive);
            }

            [Test]
            public void JamIsNotActiveAfterSpecifiedEndDateTest()
            {
                DateTime jamEndDate = DateTime.UtcNow.AddMinutes(-60);
                var jam = new Jam(1, 1, jamEndDate);

                Assert.IsTrue(jam.IsActive);
            }

            [Test]
            public void JamSnapshotSentContainsAllJamsUserIsIn()
            {
                IMessage message = null;

                ConnectedUserClientHandler.MessageSent += (sender, eventArgs) => message = eventArgs.Message;

                HandleMessage(jamSnapshotRequest);

                var jamSnapshot = (EntitySnapshot<Jam>) message;

                List<int> jamIds = jamSnapshot.Entities.Select(jam => jam.Id).ToList();

                Assert.AreEqual(DefaultJamIdDefaultUserIsIn, jamIds.Distinct().First());
            }

            [Test]
            public void SendsAJamSnapshotMessage()
            {
                IMessage message = null;

                ConnectedUserClientHandler.MessageSent += (sender, eventArgs) => message = eventArgs.Message;

                HandleMessage(jamSnapshotRequest);

                Assert.IsTrue(message.MessageIdentifier == MessageIdentifier.JamSnapshot);
            }

            [Test]
            public void SendsAMessage()
            {
                var isMessageSent = false;
                ConnectedUserClientHandler.MessageSent += (sender, eventArgs) => isMessageSent = true;

                HandleMessage(jamSnapshotRequest);

                Assert.IsTrue(isMessageSent);
            }

            [Test]
            public void ThrowsExceptionWhenMessageIsNotAJamSnapshotRequest()
            {
                var participationSnapshotRequest = new EntitySnapshotRequest<Participation>(DefaultUser.Id);
                Assert.Throws<InvalidCastException>(() => HandleMessage(participationSnapshotRequest));
            }
        }
    }
}