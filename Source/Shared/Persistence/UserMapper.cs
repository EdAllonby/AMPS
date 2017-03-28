using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Shared.Domain;

namespace Shared.Persistence
{
    /// <summary>
    /// A mapper for <see cref="User" />s.
    /// </summary>
    internal sealed class UserMapper : EntityMapper<User>
    {
        /// <summary>
        /// Columns for <see cref="User" />.
        /// </summary>
        protected override List<string> Columns => new List<string> { "Id", "Username" };

        protected override EntityTable Table => EntityTable.Users;

        public override bool UpdateEntity(User entity)
        {
            string updateUserQuery = $"UPDATE Users SET Id=@id,Username=@username WHERE Id = {entity.Id}";
            int rowsUpdated;

            using (var databaseConnection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(updateUserQuery, databaseConnection))
            {
                DoInsert(entity, command);

                databaseConnection.Open();
                rowsUpdated = command.ExecuteNonQuery();
                databaseConnection.Close();
            }

            return rowsUpdated == 1;
        }


        protected override bool DoDelete(int entityId)
        {
            return DeleteEntity("Users", entityId);
        }

        protected override User DoLoad(int id, SqlDataReader reader)
        {
            string userName = reader.GetString(reader.GetOrdinal("Username"));

            var user = new User(id, userName, new ConnectionStatus(id, ConnectionStatus.Status.Disconnected));

            Log.DebugFormat("User with Id {0} retrieved from Database.", user.Id);

            return user;
        }

        protected override void DoInsert(User entity, SqlCommand insertCommand)
        {
            insertCommand.Parameters.Add("@username", SqlDbType.VarChar).Value = entity.Username;
        }
    }
}