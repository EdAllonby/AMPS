using Shared;
using Shared.Domain;
using Shared.Repository;

namespace Server.EntityChangedHandler
{
    /// <summary>
    /// Handles entities that have changed in the <see cref="EntityRepository{T}" />
    /// </summary>
    internal abstract class EntityChangedHandler
    {
        protected EntityChangedHandler(IServiceRegistry serviceRegistry)
        {
            RepositoryManager = serviceRegistry.GetService<RepositoryManager>();
            ClientManager = serviceRegistry.GetService<IClientManager>();
        }

        protected RepositoryManager RepositoryManager { get; }
        protected IClientManager ClientManager { get; }

        /// <summary>
        /// Removes event subscriptions to <see cref="EntityRepository{T}" /> <see cref="Entity" /> changes.
        /// </summary>
        public abstract void StopOnMessageChangedHandling();
    }
}