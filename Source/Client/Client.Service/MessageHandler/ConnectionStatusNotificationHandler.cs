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
        public ConnectionStatusNotificationHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
        }

        protected override void HandleMessage(ConnectionStatusNotification message)
        {
            var userRepository = (UserRepository) ServiceRegistry.GetService<RepositoryManager>().GetRepository<User>();

            userRepository.UpdateUserConnectionStatus(message.ConnectionStatus);
        }
    }
}