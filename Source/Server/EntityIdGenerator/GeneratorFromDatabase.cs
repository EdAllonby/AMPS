using System.Configuration;
using System.Data.SqlClient;
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
        public int GetCurrentHighestId(string table)
        {
            int currentId = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

            string getMaxIdQuery = string.Format("SELECT MAX(Id) as Id FROM {0}", table);

            using (var databaseConnection = new SqlConnection(connectionString))
            using (var getHighestIdCommand = new SqlCommand(getMaxIdQuery, databaseConnection))
            {
                databaseConnection.Open();

                using (var reader = getHighestIdCommand.ExecuteReader())
                {
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
                }
            }

            return currentId;
        }
    }
}