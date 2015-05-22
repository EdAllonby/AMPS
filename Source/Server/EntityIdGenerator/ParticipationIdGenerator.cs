using Shared;

namespace Server.EntityIdGenerator
{
    internal sealed class ParticipationIdGenerator : EntityIdGenerator
    {
        /// <summary>
        /// Starts the Id Generator with the current Id.
        /// </summary>
        public ParticipationIdGenerator(IGeneratorStrategy generatorStrategy)
        {
            CurrentId = generatorStrategy.GetCurrentHighestId("Participations");
        }
    }
}