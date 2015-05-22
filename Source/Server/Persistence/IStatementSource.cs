using System.Collections.Generic;

namespace Server.Persistence
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