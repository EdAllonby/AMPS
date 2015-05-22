using Shared;

namespace Server.EntityIdGenerator
{
    /// <summary>
    /// Used to create a unique Id number for an entity.
    /// </summary>
    internal sealed class JamIdGenerator : EntityIdGenerator
    {
        /// <summary>
        /// Starts the Id Generator with the correct next available Id.
        /// </summary>
        public JamIdGenerator(IGeneratorStrategy generatorStrategy)
        {
            CurrentId = generatorStrategy.GetCurrentHighestId("Jams");
        }
    }
}