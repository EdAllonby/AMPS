﻿using System.IO;
using System.Net.Sockets;
using log4net;
using Shared.Message;

namespace Shared.Serialiser.MessageSerialiser
{
    /// <summary>
    /// Generic MessageSerialiser which serialises a type which inherits <see cref="IMessage" /> down the networkStream.
    /// </summary>
    /// <typeparam name="T"><see cref="IMessage" /> object which is any message that can be used in this protocol.</typeparam>
    public class MessageSerialiser<T> : IMessageSerialiser where T : IMessage
    {
        /// <summary>
        /// The <see cref="MessageSerialiser{T}" /> Log.
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(MessageSerialiser<T>));


        private readonly ISerialiser serialiser = new BinarySerialiser();

        /// <summary>
        /// Serialise the <see cref="IMessage" /> down the wire.
        /// </summary>
        /// <param name="networkStream">The stream that connects the Client and Server.</param>
        /// <param name="message">The <see cref="IMessage" /> to send.</param>
        public void Serialise(NetworkStream networkStream, IMessage message)
        {
            try
            {
                MessageIdentifierSerialiser.Serialise(networkStream, message.MessageIdentifier);

                Log.DebugFormat("Serialising {0} across the network stream.", message.MessageIdentifier);
                Serialise(networkStream, (T) message);
                Log.InfoFormat("Finished Serialising {0} across the network stream.", message.MessageIdentifier);
            }
            catch (IOException)
            {
                Log.Debug($"Could not serialise {message.MessageIdentifier}. Client has stopped its connection.");
            }
        }

        /// <summary>
        /// Deserialises an <see cref="IMessage" /> that has been received.
        /// </summary>
        /// <param name="networkStream">The stream that connects the Client and Server.</param>
        /// <returns>The <see cref="IMessage" /> that was received from the networkStream.</returns>
        public virtual IMessage Deserialise(NetworkStream networkStream)
        {
            try
            {
                var requestMessage = (IMessage) serialiser.Deserialise(networkStream);
                Log.InfoFormat("Network stream has received data and deserialised to a {0} object", requestMessage.MessageIdentifier);
                return requestMessage;
            }
            catch (IOException)
            {
                Log.Debug("Could not deserialise message. Client has stopped its connection. Passing Unrecognised message.");
                return new UnrecognisedMessage();
            }
        }

        /// <summary>
        /// Serialise <see cref="T" /> down the wire.
        /// </summary>
        /// <param name="networkStream">The networkStream that connects the Client and Server.</param>
        /// <param name="message">The message which inherits from <see cref="IMessage" />.</param>
        protected virtual void Serialise(NetworkStream networkStream, T message)
        {
            serialiser.Serialise(networkStream, message);
        }

        private class UnrecognisedMessage : IMessage
        {
            public MessageIdentifier MessageIdentifier => MessageIdentifier.UnrecognisedMessage;
        }
    }
}