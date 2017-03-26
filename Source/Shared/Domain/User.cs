using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Shared.Repository;

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
        public User([NotNull] string username)
        {
            Username = username;
        }

        /// <summary>
        /// Creates a user entity with an Id.
        /// </summary>
        /// <param name="id">The unique Id of the user.</param>
        /// <param name="username">The name of the user.</param>
        /// <param name="status">The current status of the user.</param>
        public User(int id, [NotNull] string username, ConnectionStatus status) : base(id)
        {
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

        public IEnumerable<Band> Bands
        {
            get
            {
                IReadOnlyEntityRepository<Participation> participationRepository = RepositoryManager.GetRepository<Participation>();
                IEnumerable<Participation> participations = participationRepository.GetAllEntities().Where(p => p.User.Equals(this));
                return participations.Select(x => x.Band);
            }
        }
    }
}