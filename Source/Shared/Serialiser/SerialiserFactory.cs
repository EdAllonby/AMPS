using System;
using Shared.Message;
using Shared.Serialiser.MessageSerialiser;

namespace Shared.Serialiser
{
    /// <summary>
    /// Get the correct MessageSerialiser for the given <see cref="IMessage" /> identifier.
    /// </summary>
    public static class SerialiserFactory
    {
        /// <summary>
        /// Returns the correct MessageSerialiser for the <see cref="IMessage" /> object identifier
        /// defined in the <see cref="MessageIdentifierSerialiser" /> class.
        /// </summary>
        /// <typeparam name="T">="T">The type of message that will be sent.</typeparam>
        /// <returns>The MessageSerialiser used to serialise and deserialise the message.</returns>
        public static MessageSerialiser<T> GetSerialiser<T>() where T : IMessage
        {
            return SerialiserRegistry.SerialisersByMessageType[typeof(T)] as MessageSerialiser<T>;
        }

        /// <summary>
        /// Returns the correct MessageSerialiser for the <see cref="IMessage" /> object identifier
        /// defined in the <see cref="MessageIdentifierSerialiser" /> class.
        /// </summary>
        /// <param name="identifier">The unique name of the <see cref="IMessage" />.</param>
        /// <returns>The MessageSerialiser to get for the <see cref="IMessage" />.</returns>
        public static IMessageSerialiser GetSerialiser(MessageIdentifier identifier)
        {
            if (identifier == MessageIdentifier.UnrecognisedMessage)
            {
                throw new UnrecognisedMessageException("Message Identifier is unrecognised.");
            }

            IMessageSerialiser serialiser;
            bool serialiserFound = SerialiserRegistry.SerialisersByMessageIdentifier.TryGetValue(identifier, out serialiser);

            if (!serialiserFound)
            {
                throw new ArgumentException($"A Serialiser does not exist for message type {identifier}.");
            }

            return serialiser;
        }
    }

    [Serializable]
    public class UnrecognisedMessageException : Exception
    {
        public UnrecognisedMessageException()
        {
        }

        public UnrecognisedMessageException(string message) : base(message)
        {
        }

        public UnrecognisedMessageException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}