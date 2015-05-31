using System.Collections.Generic;
using Shared;
using Shared.Domain;
using Shared.Message;

namespace Server
{
    /// <summary>
    /// Manages use of the <see cref="IClientHandler" />s the <see cref="Server" /> holds.
    /// </summary>
    internal interface IClientManager : IService
    {
        /// <summary>
        /// Sends an <see cref="IMessage" /> to all clients.
        /// </summary>
        /// <param name="message">The <see cref="IMessage" /> to send to all clients.</param>
        void SendMessageToClients(IMessage message);

        /// <summary>
        /// Sends an <see cref="IMessage" /> to selected clients.
        /// </summary>
        /// <param name="message">The <see cref="IMessage" /> to send to selected clients.</param>
        /// <param name="userIds">The users to send the <see cref="IMessage" /> to.</param>
        void SendMessageToClients(IMessage message, IEnumerable<int> userIds);

        /// <summary>
        /// Sends an <see cref="IMessage" /> to a client.
        /// </summary>
        /// <param name="message">The <see cref="IMessage" /> to send to a client.</param>
        /// <param name="userId">The client's <see cref="User" /> Id to send the <see cref="IMessage" /> to.</param>
        void SendMessageToClient(IMessage message, int userId);

        /// <summary>
        /// Add an <see cref="IClientHandler" />.
        /// </summary>
        /// <param name="userId">The client's <see cref="User" /> Id.</param>
        /// <param name="clientHandler">The <see cref="IClientHandler" /> to add.</param>
        void AddClientHandler(int userId, IClientHandler clientHandler);

        /// <summary>
        /// Check to see if <see cref="ClientHandler" /> exists.
        /// </summary>
        /// <param name="userId">The <see cref="ClientHandler" />'s associated <see cref="User" /> Id.</param>
        /// <returns></returns>
        bool HasClientHandler(int userId);

        /// <summary>
        /// Disconnect a client.
        /// </summary>
        /// <param name="userId">The client to disconnect.</param>
        void DisconnectClient(int userId);
    }
}