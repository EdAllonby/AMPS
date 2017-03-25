using System.Collections.Generic;

namespace Shared.Persistence
{
    /// <summary>
    /// Defines an SQL Statement.
    /// </summary>
    public interface IStatementSource
    {
        /// <summary>
        /// The SQL statement.
        /// </summary>
        string Sql { get; }

        /// <summary>
        /// Any parameters for the SQL statement.
        /// </summary>
        IList<string> Parameters { get; }
    }
}