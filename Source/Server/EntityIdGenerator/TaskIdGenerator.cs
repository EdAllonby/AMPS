using Shared;

namespace Server.EntityIdGenerator
{
    internal sealed class TaskIdGenerator : EntityIdGenerator
    {
        /// <summary>
        /// Starts the Id Generator with the current Id.
        /// </summary>
        public TaskIdGenerator(IGeneratorStrategy generatorStrategy)
        {
            CurrentId = generatorStrategy.GetCurrentHighestId(EntityTable.Tasks);
        }
    }
}