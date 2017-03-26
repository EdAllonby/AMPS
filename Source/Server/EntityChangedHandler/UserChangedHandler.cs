using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;

namespace Server.EntityChangedHandler
{
    /// <summary>
    /// Handles <see cref="Entity" /> changes to the <see cref="UserRepository" />.
    /// </summary>
    internal sealed class UserChangedHandler : EntityChangedHandler
    {
        private readonly IReadOnlyEntityRepository<User> userRepository;

        /// <summary>
        /// Creates a new <see cref="UserRepository" /> <see cref="Entity" /> changed handler and wires up the change events.
        /// </summary>
        /// <param name="serviceRegistry">The server's service registry.</param>
        public UserChangedHandler(IServiceRegistry serviceRegistry)
            : base(serviceRegistry)
        {
            userRepository = serviceRegistry.GetService<IRepositoryManager>().GetRepository<User>();

            userRepository.EntityAdded += OnUserAdded;
            userRepository.EntityUpdated += OnUserUpdated;
        }

        private void OnUserAdded(object sender, EntityChangedEventArgs<User> e)
        {
            var userNotification = new EntityNotification<User>(e.Entity, NotificationType.Create);

            ClientManager.SendMessageToClients(userNotification);
        }

        private void OnUserUpdated(object sender, EntityChangedEventArgs<User> e)
        {
            if (e.PreviousEntity.ConnectionStatus.UserConnectionStatus != e.Entity.ConnectionStatus.UserConnectionStatus)
            {
                OnUserConnectionUpdated(e.Entity);
            }
        }

        private void OnUserConnectionUpdated(User user)
        {
            var userNotification = new ConnectionStatusNotification(user.ConnectionStatus, NotificationType.Update);

            ClientManager.SendMessageToClients(userNotification);
        }

        /// <summary>
        /// Removes event subscriptions to <see cref="UserRepository" /> <see cref="Entity" /> changes.
        /// </summary>
        public override void StopOnMessageChangedHandling()
        {
            userRepository.EntityAdded -= OnUserAdded;
            userRepository.EntityUpdated -= OnUserUpdated;
        }
    }
}