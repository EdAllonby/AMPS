using System;
using System.Collections.Generic;
using Shared;
using Shared.Domain;

namespace Server.EntityChangedHandler
{
    internal sealed class EntityChangedHandlerRegistry
    {
        /// <summary>
        /// A read only version of an IDGenerator dictionary. No one can alter this dictionary after compiling.
        /// </summary>
        private readonly IReadOnlyDictionary<Type, EntityChangedHandler> entityIdGeneratorsIndexedByEntityType;

        /// <summary>
        /// Initialises the generators with values based on the generator strategy used.
        /// </summary>
        public EntityChangedHandlerRegistry(IServiceRegistry serviceRegistry)
        {
            entityIdGeneratorsIndexedByEntityType =
                new Dictionary<Type, EntityChangedHandler>
                {
                    { typeof(User), new UserChangedHandler(serviceRegistry) },
                    { typeof(Band), new BandChangedHandler(serviceRegistry) },
                    { typeof(Jam), new JamChangedHandler(serviceRegistry) },
                    { typeof(Participation), new ParticipationChangedHandler(serviceRegistry) },
                    { typeof(Task), new TaskChangedHandler(serviceRegistry) },
                    { typeof(TaskComment), new TaskCommentChangedHandler(serviceRegistry) }
                };
        }

        public void StopListening()
        {
            foreach (EntityChangedHandler entityChangedHandler in entityIdGeneratorsIndexedByEntityType.Values)
            {
                entityChangedHandler.StopOnMessageChangedHandling();
            }
        }
    }
}