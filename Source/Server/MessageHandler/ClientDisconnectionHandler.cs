using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Server.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="ClientDisconnection" /> the Server received.
    /// </summary>
    internal sealed class ClientDisconnectionHandler : MessageHandler<ClientDisconnection>
    {
        public ClientDisconnectionHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
        }

        /// <summary>
        /// Handles the incoming <see cref="ClientDisconnection" />.
        /// </summary>
        /// <param name="message">The <see cref="ClientDisconnection" /> that has been received and needs to be handled.</param>
        protected override void HandleMessage(ClientDisconnection message)
        {
            var clientManager = ServiceRegistry.GetService<IClientManager>();

            clientManager.DisconnectClient(message.UserId);

            var connectionStatus = new ConnectionStatus(message.UserId, ConnectionStatus.Status.Disconnected);

            var userRepository = (UserRepository) ServiceRegistry.GetService<RepositoryManager>().GetRepository<User>();

            userRepository.UpdateUserConnectionStatus(connectionStatus);
        }
    }
}