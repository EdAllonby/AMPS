using Shared;
using Shared.Domain;
using Shared.Repository;

namespace Server.EntityChangedHandler
{
    /// <summary>
    /// Handles entities that have changed in the <see cref="EntityRepository{T}"/>
    /// </summary>
    internal abstract class OnEntityChangedHandler
    {
        private readonly IClientManager clientManager;
        private readonly RepositoryManager repositoryManager;

        protected OnEntityChangedHandler(IServiceRegistry serviceRegistry)
        {
            repositoryManager = serviceRegistry.GetService<RepositoryManager>();
            clientManager = serviceRegistry.GetService<IClientManager>();
        }

        protected RepositoryManager RepositoryManager
        {
            get { return repositoryManager; }
        }

        protected IClientManager ClientManager
        {
            get { return clientManager; }
        }

        /// <summary>
        /// Removes event subscriptions to <see cref="EntityRepository{T}"/> <see cref="Entity"/> changes.
        /// </summary>
        public abstract void StopOnMessageChangedHandling();
    }
}