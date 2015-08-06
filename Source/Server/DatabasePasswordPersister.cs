using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Server
{
    /// <summary>
    /// Persist Passwords in a database.
    /// </summary>
    public sealed class DatabasePasswordPersister : IPasswordPersister
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

        /// <summary>
        /// Store the password.
        /// </summary>
        /// <param name="userId">The User's Id to store the password.</param>
        /// <param name="generatedPasswordHash">The password hash to store.</param>
        /// <returns>Whether the operation was successful.</returns>
        public bool StorePasswordHash(int userId, string generatedPasswordHash)
        {
            const string InsertPasswordHashQuery = "INSERT INTO UserLogins (UserId,PasswordHash) VALUES (@userId,@passwordHash)";

            int rowsUpdated;

            using (SqlConnection databaseConnection = new SqlConnection(connectionString))
            using (SqlCommand command = CreatePasswordCommand(InsertPasswordHashQuery, userId, generatedPasswordHash, databaseConnection))
            {
                databaseConnection.Open();
                rowsUpdated = command.ExecuteNonQuery();
                databaseConnection.Close();
            }

            return rowsUpdated == 1;
        }

        /// <summary>
        /// Gets the password hash for a user's Id to get.
        /// </summary>
        /// <param name="userId">The Id of the user to get.</param>
        /// <returns>The user's password hash (null if not found).</returns>
        public string GetPasswordHash(int userId)
        {
            string storedPasswordHash = null;

            string getUserQuery = $"SELECT PasswordHash FROM UserLogins where UserId = {userId}";

            using (SqlConnection databaseConnection = new SqlConnection(connectionString))
            using (SqlCommand getUserCommand = new SqlCommand(getUserQuery, databaseConnection))
            {
                databaseConnection.Open();

                using (SqlDataReader reader = getUserCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            storedPasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash"));
                        }

                        return storedPasswordHash;
                    }
                    return null;
                }
            }
        }

        private static SqlCommand CreatePasswordCommand(string query, int userId, string generatedPasswordHash, SqlConnection databaseConnection)
        {
            SqlCommand command = new SqlCommand(query, databaseConnection);

            command.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@passwordHash", SqlDbType.VarChar).Value = generatedPasswordHash;

            return command;
        }
    }
}