using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using log4net;
using Shared.Message;
using Shared.Serialiser;

namespace Shared
{
    /// <summary>
    /// This is in charge of abstracting away the <see cref="TcpClient"/> work for sending and receiving <see cref="IMessage"/>s.
    /// This class has no logic other than to send and receive messages to and from a <see cref="NetworkStream"/>.
    /// This class is identified by the <see cref="clientUserId"/>.
    /// </summary>
    public sealed class ConnectionHandler : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (ConnectionHandler));
        private static int totalListenerThreads;
        private readonly int clientUserId;
        private readonly MessageReceiver messageReceiver = new MessageReceiver();
        private readonly object messageSenderLock = new object();
        private readonly TcpClient tcpClient;

        /// <summary>
        /// Initialises the object so it can begin to send and receive <see cref="IMessage"/>s through <see cref="tcpClient"/>.
        /// </summary>
        /// <param name="clientUserId">A unique value that identifies the client.</param>
        /// <param name="tcpClient">The connection between the server and the client.</param>
        public ConnectionHandler(int clientUserId, TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            this.clientUserId = clientUserId;

            Log.Info("New connection handler created");

            CreateReceiverThread();

            messageReceiver.MessageReceived += OnMessageReceiverMessageReceived;
        }

        /// <summary>
        /// Gets the IP details of the remove connections.
        /// </summary>
        public IPEndPoint RemoteEndPoint
        {
            get { return (IPEndPoint) tcpClient.Client.RemoteEndPoint; }
        }

        /// <summary>
        /// Closes the <see cref="TcpClient"/>.
        /// </summary>
        public void Dispose()
        {
            tcpClient.Close();
        }

        /// <summary>
        /// Gets called when a message is received.
        /// </summary>
        public event EventHandler<MessageEventArgs> MessageReceived;

        /// <summary>
        /// Sends an <see cref="IMessage"/> across the <see cref="ConnectionHandler"/>'s <see cref="NetworkStream"/>.
        /// </summary>
        /// <param name="message">The message to send across the socket connection defined for this object.</param>
        public void SendMessage(IMessage message)
        {
            Contract.Requires(message != null);

            lock (messageSenderLock)
            {
                IMessageSerialiser messageSerialiser = SerialiserFactory.GetSerialiser(message.MessageIdentifier);

                if (tcpClient.Connected)
                {
                    messageSerialiser.Serialise(tcpClient.GetStream(), message);
                    Log.DebugFormat("Sent message with identifier {0} associated user with id {1}", message.MessageIdentifier, clientUserId);
                }
                else
                {
                    Log.WarnFormat("No TCP connection. Will not attempt to send {0} message", message.MessageIdentifier);
                }
            }
        }

        private void CreateReceiverThread()
        {
            var messageListenerThread = new Thread(() => messageReceiver.ReceiveMessages(clientUserId, tcpClient))
            {
                Name = "ReceiveMessageThread" + (totalListenerThreads++)
            };

            messageListenerThread.Start();
        }

        private void OnMessageReceiverMessageReceived(object sender, MessageEventArgs e)
        {
            EventHandler<MessageEventArgs> messageReceivedCopy = MessageReceived;

            if (messageReceivedCopy != null)
            {
                messageReceivedCopy(sender, e);
            }
        }

        /// <summary>
        /// Disconnect the socket.
        /// </summary>
        public void Disconnect()
        {
            if (tcpClient.Connected)
            {
                tcpClient.GetStream().Close();
                tcpClient.Close();
            }

            Log.InfoFormat("TCP connection closed for client with Id: {0}.", clientUserId);
        }
    }
}