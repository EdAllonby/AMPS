using System.Data;

namespace Shared.Persistence
{
    /// <summary>
    /// Extension methods for SQL functions.
    /// </summary>
    public static class SqlExtensions
    {
        /// <summary>
        /// Safely gets a possible nullable <see langword="int" />.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="colIndex">Return nullable int with potential column value.</param>
        /// <returns></returns>
        public static int? GetNullableInt(this IDataRecord reader, int colIndex)
        {
            return !reader.IsDBNull(colIndex) ? reader.GetInt32(colIndex) : new int?();
        }
    }
}