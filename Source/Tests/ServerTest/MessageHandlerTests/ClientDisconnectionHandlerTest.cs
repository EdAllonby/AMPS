﻿using System;
using NUnit.Framework;
using Server;
using Server.MessageHandler;
using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Message.LoginMessage;
using Shared.Repository;

namespace ServerTest.MessageHandlerTests
{
    [TestFixture]
    public class ClientDisconnectionHandlerTest : MessageHandlerTestFixture
    {
        [SetUp]
        public override void BeforeEachTest()
        {
            base.BeforeEachTest();
            clientDisconnectionHandler = new ClientDisconnectionHandler(ServiceRegistry);
            clientDisconnection = new ClientDisconnection(DefaultUser.Id);
        }

        private ClientDisconnectionHandler clientDisconnectionHandler;
        private ClientDisconnection clientDisconnection;

        public override void HandleMessage(IMessage message)
        {
            clientDisconnectionHandler.HandleMessage(message);
        }

        [TestFixture]
        public class HandleMessageTest : ClientDisconnectionHandlerTest
        {
            [Test]
            public void ClientHandlerGetsRemovedFromClientManager()
            {
                HandleMessage(clientDisconnection);
                Assert.IsFalse(ServiceRegistry.GetService<IClientManager>().HasClientHandler(DefaultUser.Id));
            }

            [Test]
            public void RepositoryUpdatesUser()
            {
                var isUserUpdated = false;
                ServiceRegistry.GetService<IRepositoryManager>().GetRepository<User>().EntityUpdated += (sender, eventArgs) => isUserUpdated = true;
                HandleMessage(clientDisconnection);
                Assert.IsTrue(isUserUpdated);
            }

            [Test]
            public void ThrowsExceptionWhenNotGivenClientDisconnection()
            {
                Assert.Throws<InvalidCastException>(() => HandleMessage(new LoginRequest("user", "pa$$word")));
            }

            [Test]
            public void UserGetsSetToDisconnectedInUserRepository()
            {
                HandleMessage(clientDisconnection);
                IReadOnlyEntityRepository<User> userRepository = ServiceRegistry.GetService<IRepositoryManager>().GetRepository<User>();
                User defaultUser = userRepository.FindEntityById(DefaultUser.Id);
                Assert.IsTrue(defaultUser.ConnectionStatus.UserConnectionStatus.Equals(ConnectionStatus.Status.Disconnected));
            }
        }
    }
}