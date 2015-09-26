namespace Shared
{
    /// <summary>
    /// Get a generator from initial settings (starting at 1).
    /// </summary>
    public sealed class NewGenerator : IGeneratorStrategy
    {
        public int GetCurrentHighestId()
        {
            return 0;
        }

        /// <summary>
        /// Return the default Id to begin iterating.
        /// </summary>
        /// <param name="table">Not used.</param>
        /// <returns>The default Id to being iterating.</returns>
        public int GetCurrentHighestId(EntityTable table)
        {
            return 0;
        }
    }
}