using System.Configuration;
using Microsoft.Data.SqlClient;
using Shared;

namespace Server.EntityIdGenerator
{
    /// <summary>
    /// Get a generator from a database.
    /// </summary>
    public sealed class GeneratorFromDatabase : IGeneratorStrategy
    {
        /// <summary>
        /// Gets the current highest Id for the
        /// </summary>
        /// <returns></returns>
        public int GetCurrentHighestId(EntityTable table)
        {
            var currentId = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

            string getMaxIdQuery = $"SELECT MAX(Id) as Id FROM {table}";

            using var databaseConnection = new SqlConnection(connectionString);
            using var getHighestIdCommand = new SqlCommand(getMaxIdQuery, databaseConnection);
            databaseConnection.Open();

            using SqlDataReader reader = getHighestIdCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("Id")))
                    {
                        currentId = reader.GetInt32(reader.GetOrdinal("Id"));
                    }
                }
            }

            return currentId;
        }
    }
}