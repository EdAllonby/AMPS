using System;
using NUnit.Framework;
using Shared;
using Shared.Message;
using Shared.Message.LoginMessage;

namespace SharedTest
{
    [TestFixture]
    public sealed class MessageEventArgsTest
    {
        [Test]
        public void HoldsAnIMessage()
        {
            IMessage message = new LoginRequest("1", "1");

            var messageEventArgs = new MessageEventArgs(message);

            Assert.AreEqual(message, messageEventArgs.Message);
        }

        [Test]
        public void IsAnEventArgs()
        {
            var messageEventArgs = new MessageEventArgs(new LoginRequest("1", "1"));

            Assert.IsInstanceOf<EventArgs>(messageEventArgs);
        }
    }
}