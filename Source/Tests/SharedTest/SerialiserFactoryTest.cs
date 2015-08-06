using NUnit.Framework;
using Shared.Message;
using Shared.Message.LoginMessage;
using Shared.Serialiser;
using Shared.Serialiser.MessageSerialiser;

namespace SharedTest
{
    [TestFixture]
    public sealed class SerialiserFactoryTest
    {
        [Test]
        public void GetSerialiserByMessageIdentifier()
        {
            IMessageSerialiser message = SerialiserFactory.GetSerialiser(MessageIdentifier.LoginRequest);

            Assert.That(message, Is.Not.Null);
        }

        [Test]
        public void GetSerialiserByType()
        {
            MessageSerialiser<LoginRequest> loginRequestSerialiser = SerialiserFactory.GetSerialiser<LoginRequest>();

            Assert.That(loginRequestSerialiser, Is.Not.Null);
        }
    }
}