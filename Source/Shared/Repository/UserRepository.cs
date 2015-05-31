using System.Diagnostics.Contracts;
using System.Linq;
using Shared.Domain;

namespace Shared.Repository
{
    /// <summary>
    /// Models a <see cref="Repository" /> for a <see cref="User" />.
    /// </summary>
    public sealed class UserRepository : EntityRepository<User>
    {
        /// <summary>
        /// Create a repository for storing <see cref="User" />s with a data mapping implementation.
        /// </summary>
        /// <param name="entityPersister">The <see cref="IEntityDataMapper{User}" /> implementation to use.</param>
        public UserRepository(IEntityPersister<User> entityPersister) : base(entityPersister)
        {
            Contract.Requires(entityPersister != null);
        }

        /// <summary>
        /// Updates a <see cref="User" />'s <see cref="ConnectionStatus" />
        /// </summary>
        /// <param name="connectionStatus">The new connection status of the user.</param>
        public void UpdateUserConnectionStatus(ConnectionStatus connectionStatus)
        {
            Contract.Requires(connectionStatus != null);

            User user = FindEntityById(connectionStatus.UserId);

            User previousUser = Entity.DeepClone(user);

            user.ConnectionStatus = connectionStatus;

            OnEntityUpdated(user, previousUser);
        }

        /// <summary>
        /// Gets a <see cref="User" /> <see cref="Entity" /> by username.
        /// </summary>
        /// <param name="username">The username that is used to find the <see cref="User" />.</param>
        /// <returns>The <see cref="User" /> that matches the username.</returns>
        public User FindUserByUsername(string username)
        {
            return GetAllEntities().Where(user => user.Username.Equals(username)).Select(user => user).FirstOrDefault();
        }
    }
}