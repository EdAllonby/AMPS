using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Shared.Domain;

namespace Shared.Repository
{
    /// <summary>
    /// Models a <see cref="Repository" /> for a <see cref="Task" />.
    /// </summary>
    public sealed class TaskRepository : EntityRepository<Task>
    {
        /// <summary>
        /// Intialises the repository with any <see cref="Task" /> entities it finds from the data layer.
        /// </summary>
        /// <param name="entityPersister">The <see cref="Task" /> entity-to-table mapper to use.</param>
        public TaskRepository(IEntityPersister<Task> entityPersister) : base(entityPersister)
        {
            Contract.Requires(entityPersister != null);
        }

        /// <summary>
        /// Gets all <see cref="Task" />s in a <see cref="Band" />s backlog.
        /// </summary>
        /// <param name="bandId">The <see cref="Band" />'s Id to check for task backlog.</param>
        /// <returns><see cref="Task" />s in the <see cref="Band" />s backlog.</returns>
        public IEnumerable<Task> GetTasksInBandBacklog(int bandId)
        {
            return GetTasksAssociatedWithBand(bandId).Where(task => !task.IsInJam);
        }

        /// <summary>
        /// Gets all <see cref="Task" />s which are associated with the <see cref="Band" /> Id.
        /// </summary>
        /// <param name="bandId">The <see cref="Band" /> to get all <see cref="Task" />s.</param>
        /// <returns>A collection of <see cref="Task" />s.</returns>
        public IEnumerable<Task> GetTasksAssociatedWithBand(int bandId)
        {
            return GetAllEntities().Where(task => task.BandId.Equals(bandId));
        }

        /// <summary>
        /// Find all <see cref="Task" />s that match a <see cref="Jam" />'s Id.
        /// </summary>
        /// <param name="jamId">The <see cref="Jam" />'s Id to get <see cref="Task" />s.</param>
        /// <returns><see cref="Task" />s in the specified <see cref="Jam" />.</returns>
        public IEnumerable<Task> GetTasksInJam(int jamId)
        {
            return GetAllEntities().Where(task => task.JamId.Equals(jamId));
        }
    }
}