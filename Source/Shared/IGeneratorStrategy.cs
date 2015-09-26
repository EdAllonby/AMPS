namespace Shared
{
    /// <summary>
    /// A strategy for getting a generator.
    /// </summary>
    public interface IGeneratorStrategy
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        int GetCurrentHighestId(EntityTable entityTable);
    }
}