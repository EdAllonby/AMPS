using System.Collections.Generic;
using System.Linq;
using log4net;
using Shared.Domain;
using Shared.Message;

namespace Server
{
    /// <summary>
    /// Holds logic for a collection of <see cref="ClientHandler" />s.
    /// </summary>
    internal sealed class ClientManager : IClientManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (ClientManager));
        private readonly Dictionary<int, IClientHandler> clientHandlersIndexedByUserId = new Dictionary<int, IClientHandler>();

        /// <summary>
        /// Sends an <see cref="IMessage" /> to all clients.
        /// </summary>
        /// <param name="message">The <see cref="IMessage" /> to send to all clients.</param>
        public void SendMessageToClients(IMessage message)
        {
            foreach (IClientHandler clientHandler in clientHandlersIndexedByUserId.Values)
            {
                clientHandler.SendMessage(message);
            }
        }

        /// <summary>
        /// Sends an <see cref="IMessage" /> to selected clients.
        /// </summary>
        /// <param name="message">The <see cref="IMessage" /> to send to selected clients.</param>
        /// <param name="userIds">The users to send the <see cref="IMessage" /> to.</param>
        public void SendMessageToClients(IMessage message, IEnumerable<int> userIds)
        {
            foreach (int userId in userIds.Where(HasClientHandler))
            {
                clientHandlersIndexedByUserId[userId].SendMessage(message);
            }
        }

        /// <summary>
        /// Sends an <see cref="IMessage" /> to a client.
        /// </summary>
        /// <param name="message">The <see cref="IMessage" /> to send to a client.</param>
        /// <param name="userId">The client's <see cref="User" /> Id to send the <see cref="IMessage" /> to.</param>
        public void SendMessageToClient(IMessage message, int userId)
        {
            if (HasClientHandler(userId))
            {
                clientHandlersIndexedByUserId[userId].SendMessage(message);
            }
        }

        /// <summary>
        /// Add a <see cref="ClientHandler" />.
        /// </summary>
        /// <param name="userId">The client's <see cref="User" /> Id.</param>
        /// <param name="clientHandler">The <see cref="ClientHandler" /> to add.</param>
        public void AddClientHandler(int userId, IClientHandler clientHandler)
        {
            clientHandlersIndexedByUserId.Add(userId, clientHandler);
        }

        /// <summary>
        /// Returns whether a <see cref="ClientHandler" /> exists for the <see cref="User" /> Id.
        /// </summary>
        /// <param name="userId">The <see cref="User" /> Id to check for a <see cref="ClientHandler" />.</param>
        /// <returns>Whether a <see cref="ClientHandler" /> exists for the <see cref="User" />.</returns>
        public bool HasClientHandler(int userId)
        {
            return clientHandlersIndexedByUserId.ContainsKey(userId);
        }

        public void DisconnectClient(int userId)
        {
            IClientHandler clientHandler = clientHandlersIndexedByUserId[userId];

            clientHandler.DisconnectClient();

            RemoveClientHandler(userId);
        }

        /// <summary>
        /// Removes a <see cref="ClientHandler" />.
        /// </summary>
        /// <param name="userId">The <see cref="ClientHandler" />'s associated <see cref="User" /> Id.</param>
        private void RemoveClientHandler(int userId)
        {
            clientHandlersIndexedByUserId.Remove(userId);
            Log.InfoFormat("ClientHandler with User Id {0} Removed from ClientManager.", userId);
        }
    }
}