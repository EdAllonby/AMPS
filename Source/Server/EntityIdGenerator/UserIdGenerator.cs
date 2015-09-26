using Shared;

namespace Server.EntityIdGenerator
{
    /// <summary>
    /// Used to create a unique Id number for an entity.
    /// </summary>
    internal sealed class UserIdGenerator : EntityIdGenerator
    {
        /// <summary>
        /// Starts the ID Generator with the correct next available Id.
        /// </summary>
        public UserIdGenerator(IGeneratorStrategy generatorStrategy)
        {
            CurrentId = generatorStrategy.GetCurrentHighestId(EntityTable.Users);
        }
    }
}