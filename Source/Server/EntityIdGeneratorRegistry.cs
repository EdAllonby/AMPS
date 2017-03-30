using System;
using System.Collections.Generic;
using Server.EntityIdGenerator;
using Shared;
using Shared.Domain;

namespace Server
{
    /// <summary>
    /// Holds the <see cref="UserIdGenerator" />s for each entity type that needs one.
    /// </summary>
    internal sealed class EntityIdGeneratorRegistry
    {
        /// <summary>
        /// A read only version of an IDGenerator dictionary. No one can alter this dictionary after compiling.
        /// </summary>
        public readonly IReadOnlyDictionary<Type, EntityIdGenerator.EntityIdGenerator> EntityIdGeneratorsIndexedByEntityType;

        /// <summary>
        /// Initialises the generators with values based on the generator strategy used.
        /// </summary>
        /// <param name="generatorStrategy"></param>
        public EntityIdGeneratorRegistry(IGeneratorStrategy generatorStrategy)
        {
            EntityIdGeneratorsIndexedByEntityType =
                new Dictionary<Type, EntityIdGenerator.EntityIdGenerator>
                {
                    { typeof(User), new UserIdGenerator(generatorStrategy) },
                    { typeof(Band), new BandIdGenerator(generatorStrategy) },
                    { typeof(Jam), new JamIdGenerator(generatorStrategy) },
                    { typeof(Participation), new ParticipationIdGenerator(generatorStrategy) },
                    { typeof(Task), new TaskIdGenerator(generatorStrategy) },
                    { typeof(TaskComment), new TaskCommentIdGenerator(generatorStrategy) }
                };
        }
    }
}