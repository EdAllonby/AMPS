using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Repository;

namespace Shared.Domain
{
    /// <summary>
    /// Models a Band entity.
    /// </summary>
    [Serializable]
    public sealed class Band : Entity
    {
        /// <summary>
        /// Creates a new <see cref="Band" /> which has an Id.
        /// </summary>
        /// <param name="id">The Id assigned to the <see cref="Band" />.</param>
        /// <param name="name">The name of the <see cref="Band" />.</param>
        public Band(int id, string name) : base(id)
        {
            Name = name;
        }

        /// <summary>
        /// The name of the <see cref="Band" />.
        /// </summary>
        public string Name { get; }

        public IEnumerable<Task> Backlog
        {
            get
            {
                var taskRepository = (TaskRepository) RepositoryManager.GetRepository<Task>();
                return taskRepository.GetTasksInBandBacklog(Id);
            }
        }

        /// <summary>
        /// Current members of this <see cref="Band" />.
        /// </summary>
        public IEnumerable<User> Members
        {
            get
            {
                var participationRepository = (ParticipationRepository) RepositoryManager.GetRepository<Participation>();
                List<Participation> participations = participationRepository.GetParticipationsByBandId(Id);
                return participations.Select(p => p.User);
            }
        }
    }
}