namespace Shared
{
    /// <summary>
    /// Holds references to services.
    /// </summary>
    public interface IServiceRegistry
    {
        /// <summary>
        /// Gets an <see cref="IService" /> from its type.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IService" /> to return.</typeparam>
        /// <returns>The service requested.</returns>
        T GetService<T>() where T : IService;

        /// <summary>
        /// Adds an <see cref="IService" /> to the registry.
        /// </summary>
        /// <typeparam name="T">The type of service to add.</typeparam>
        /// <param name="service">The <see cref="IService" /> instance to add.</param>
        void RegisterService<T>(IService service) where T : IService;
    }
}