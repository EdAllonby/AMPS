using System.Collections.Generic;
using log4net;
using Shared;
using Shared.Domain;

namespace Server
{
    /// <summary>
    /// Holds the Id generators for entities.
    /// </summary>
    public class EntityIdAllocatorFactory : IService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(EntityIdAllocatorFactory));
        private readonly EntityIdGeneratorRegistry entityIdGeneratorRegistry;

        /// <summary>
        /// Creates a new registry of generators based on the initialisation strategy used.
        /// </summary>
        /// <param name="generatorStrategy"></param>
        public EntityIdAllocatorFactory(IGeneratorStrategy generatorStrategy)
        {
            entityIdGeneratorRegistry = new EntityIdGeneratorRegistry(generatorStrategy);
        }

        /// <summary>
        /// Gets the next available unique Id for the given entity.
        /// </summary>
        /// <typeparam name="T">The entity which wants a unique Id.</typeparam>
        /// <returns>A unique Id for the entity.</returns>
        public int AllocateEntityId<T>() where T : Entity
        {
            EntityIdGenerator.EntityIdGenerator generator;

            bool isEntityGeneratorFound = entityIdGeneratorRegistry.EntityIdGeneratorsIndexedByEntityType.TryGetValue(typeof(T), out generator);

            if (!isEntityGeneratorFound)
            {
                Log.ErrorFormat("Entity generator for type {0} does not exist in {1}.", typeof(T), entityIdGeneratorRegistry);
                throw new KeyNotFoundException();
            }

            return generator.GenerateNextAvailableId();
        }
    }
}