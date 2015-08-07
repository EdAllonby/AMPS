using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Client.Service.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="ConnectionStatusNotification" /> the Client received.
    /// </summary>
    internal sealed class ConnectionStatusNotificationHandler : MessageHandler<ConnectionStatusNotification>
    {
        public override void HandleMessage(ConnectionStatusNotification message, IServiceRegistry serviceRegistry)
        {
            var userRepository = (UserRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<User>();

            userRepository.UpdateUserConnectionStatus(message.ConnectionStatus);
        }
    }
}