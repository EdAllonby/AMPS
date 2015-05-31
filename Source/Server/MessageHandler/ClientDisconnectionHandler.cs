using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Server.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="ClientDisconnection" /> the Server received.
    /// </summary>
    internal sealed class ClientDisconnectionHandler : IMessageHandler
    {
        /// <summary>
        /// Handles the incoming <see cref="ClientDisconnection" />.
        /// </summary>
        /// <param name="message">The <see cref="ClientDisconnection" /> that has been received and needs to be handled.</param>
        /// <param name="serviceRegistry">The services needed to handle the message correctly.</param>
        public void HandleMessage(IMessage message, IServiceRegistry serviceRegistry)
        {
            var clientDisconnection = (ClientDisconnection) message;

            var clientManager = serviceRegistry.GetService<IClientManager>();

            clientManager.DisconnectClient(clientDisconnection.UserId);

            var connectionStatus = new ConnectionStatus(clientDisconnection.UserId, ConnectionStatus.Status.Disconnected);

            var userRepository = (UserRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<User>();

            userRepository.UpdateUserConnectionStatus(connectionStatus);
        }
    }
}