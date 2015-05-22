using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net.Sockets;
using log4net;
using Shared.Message;
using Shared.Serialiser;
using Utility;

namespace Shared
{
    /// <summary>
    /// Listens for incoming messages from the tcp connection. When a new <see cref="IMessage" /> is received,
    /// it will then fire off an <see cref="MessageReceived" /> event where subscribers will be notified.
    /// </summary>
    public sealed class MessageReceiver
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (MessageReceiver));

        /// <summary>
        /// Fires a <see cref="MessageEventArgs"/> encapsulating an <see cref="IMessage"/> when a new message is received.
        /// </summary>
        public event EventHandler<MessageEventArgs> MessageReceived;

        /// <summary>
        /// Listens for incoming messages on the <see cref="NetworkStream"/>.
        /// Fires a <see cref="MessageReceived"/> event when a new <see cref="IMessage"/> has been received.
        /// </summary>
        /// <param name="clientUserId">The Id of the user the NetworkStream is linked to.</param>
        /// <param name="tcpClient">The stream between the Client and the Server.</param>
        public void ReceiveMessages(int clientUserId, TcpClient tcpClient)
        {
            Contract.Requires(clientUserId > 0);
            Contract.Requires(tcpClient != null);

            using (NetworkStream networkStream = tcpClient.GetStream())
            {
                try
                {
                    while (true)
                    {
                        MessageIdentifier messageIdentifier = MessageIdentifierSerialiser.DeserialiseMessageIdentifier(networkStream);

                        IMessageSerialiser messageSerialiser = SerialiserFactory.GetSerialiser(messageIdentifier);

                        IMessage message = messageSerialiser.Deserialise(networkStream);

                        EventUtility.SafeFireEvent(MessageReceived, this, new MessageEventArgs(message));
                    }
                }
                catch (IOException)
                {
                    Log.Info("Detected client disconnection, notifying Server of ClientDisconnection.");
                    IMessage message = new ClientDisconnection(clientUserId);

                    EventUtility.SafeFireEvent(MessageReceived, this, new MessageEventArgs(message));
                }
                catch (ObjectDisposedException)
                {
                    // TODO: Cardinal sin.
                }
                catch (ArgumentException)
                {
                    // TODO: Cardinal sin.
                }
            }
        }
    }
}