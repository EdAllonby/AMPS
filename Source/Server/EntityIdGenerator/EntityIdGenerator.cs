using System.Threading;
using Shared.Domain;

namespace Server.EntityIdGenerator
{
    /// <summary>
    /// An <see cref="Entity" /> Id Generator.
    /// </summary>
    internal abstract class EntityIdGenerator
    {
        protected int CurrentId;

        /// <summary>
        /// Creates a thread-safe unique Id number for an entity.
        /// </summary>
        /// <returns>A unique Id number.</returns>
        public int GenerateNextAvailableId()
        {
            return Interlocked.Increment(ref CurrentId);
        }
    }
}