using System;
using System.Collections.Generic;
using NUnit.Framework;
using Server.MessageHandler;
using Shared;
using Shared.Domain;
using Shared.Message;

namespace ServerTest.MessageHandlerTests
{
    [TestFixture]
    public class UserSnapshotRequestHandlerTest : MessageHandlerTestFixture
    {
        public override void BeforeEachTest()
        {
            base.BeforeEachTest();
            userSnapshotRequestHandler = new UserSnapshotRequestHandler(ServiceRegistry);
            userSnapshotRequest = new EntitySnapshotRequest<User>(DefaultUser.Id);
        }

        private UserSnapshotRequestHandler userSnapshotRequestHandler;
        private EntitySnapshotRequest<User> userSnapshotRequest;

        public override void HandleMessage(IMessage message)
        {
            userSnapshotRequestHandler.HandleMessage(message);
        }

        [TestFixture]
        public class HandleMessageTest : UserSnapshotRequestHandlerTest
        {
            [Test]
            public void SendsAMessage()
            {
                bool isMessageSent = false;
                ConnectedUserClientHandler.MessageSent += (sender, eventArgs) => isMessageSent = true;

                HandleMessage(userSnapshotRequest);

                Assert.IsTrue(isMessageSent);
            }

            [Test]
            public void SendsAUserSnapshotMessage()
            {
                IMessage message = null;

                ConnectedUserClientHandler.MessageSent += (sender, eventArgs) => message = eventArgs.Message;

                HandleMessage(userSnapshotRequest);

                Assert.IsTrue(message.MessageIdentifier == MessageIdentifier.UserSnapshot);
            }

            [Test]
            public void ThrowsExceptionWhenMessageIsNotUserSnapshotRequest()
            {
                var clientDisconnectionMessage = new ClientDisconnection(DefaultUser.Id);
                Assert.Throws<InvalidCastException>(() => HandleMessage(clientDisconnectionMessage));
            }

            [Test]
            public void UserSnapshotSentContainsAllUsers()
            {
                IMessage message = null;

                ConnectedUserClientHandler.MessageSent += (sender, eventArgs) => message = eventArgs.Message;

                HandleMessage(userSnapshotRequest);

                var userSnapshot = (EntitySnapshot<User>) message;

                IEnumerable<User> allUsers = ServiceRegistry.GetService<IRepositoryManager>().GetRepository<User>().GetAllEntities();

                Assert.AreEqual(userSnapshot.Entities, allUsers);
            }
        }
    }
}