using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Client.Service.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="ConnectionStatusNotification" /> the Client received.
    /// </summary>
    internal sealed class ConnectionStatusNotificationHandler : IMessageHandler
    {
        public void HandleMessage(IMessage message, IServiceRegistry serviceRegistry)
        {
            var connectionStatusNotification = (ConnectionStatusNotification) message;

            var userRepository = (UserRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<User>();

            userRepository.UpdateUserConnectionStatus(connectionStatusNotification.ConnectionStatus);
        }
    }
}