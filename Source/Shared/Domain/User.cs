using System;
using System.Diagnostics.Contracts;

namespace Shared.Domain
{
    /// <summary>
    /// Models a user in the system as an entity.
    /// </summary>
    [Serializable]
    public sealed class User : Entity
    {
        /// <summary>
        /// Creates an incomplete user entity.
        /// </summary>
        public User(string username)
        {
            Contract.Requires(username != null);

            Username = username;
        }

        /// <summary>
        /// Creates a user entity with an Id.
        /// </summary>
        /// <param name="id">The unique Id of the user.</param>
        /// <param name="username">The name of the user.</param>
        /// <param name="status">The current status of the user.</param>
        public User(int id, string username, ConnectionStatus status) : base(id)
        {
            Contract.Requires(username != null);
            Contract.Requires(id > 0);

            Username = username;
            ConnectionStatus = status;
        }

        /// <summary>
        /// The name of the <see cref="User" />.
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// The current status of the <see cref="User" />.
        /// </summary>
        public ConnectionStatus ConnectionStatus { get; set; }
    }
}