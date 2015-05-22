using System;
using System.Net.Sockets;
using Server;
using Shared;
using Shared.Message;
using Shared.Message.LoginMessage;
using Utility;

namespace ServerTest
{
    /// <summary>
    /// Mocks the functionality of the <see cref="ClientHandler" />.
    /// </summary>
    public sealed class MockClientHandler : IClientHandler
    {
        /// <summary>
        /// Not implemented in Mock Handler: Fires when a message has been received to the <see cref="IClientHandler" />.
        /// </summary>
        public event EventHandler<MessageEventArgs> MessageReceived;

        /// <summary>
        /// No implementation.
        /// </summary>
        public LoginResponse InitialiseClient(TcpClient tcpClient, IServiceRegistry serviceRegistry)
        {
            return null;
        }

        /// <summary>
        /// Sends an <see cref="IMessage" /> to a client.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public void SendMessage(IMessage message)
        {
            EventUtility.SafeFireEvent(MessageSent, this, new MessageEventArgs(message));
        }

        /// <summary>
        /// Disconnect the client.
        /// </summary>
        public void DisconnectClient()
        {
        }

        /// <summary>
        /// Fires when a message has been sent from the <see cref="IMessageHandler" />.
        /// </summary>
        public event EventHandler<MessageEventArgs> MessageSent;
    }
}