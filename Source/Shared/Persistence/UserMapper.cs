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
        private const string Columns = " Id, Username ";

        protected override string FindStatement => "SELECT " + Columns +
                                                   " FROM Users" +
                                                   " WHERE Id = @id ";

        protected override string InsertStatement => "INSERT INTO Users VALUES (@id,@username)";

        public override bool UpdateEntity(User entity)
        {
            string updateUserQuery = $"UPDATE Participations SET Id=@id,Username=@username WHERE Id = {entity.Id}";
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

        public override IEnumerable<User> GetAllEntities()
        {
            return FindMany(new FindAllUsers());
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
            insertCommand.Parameters.Add("@id", SqlDbType.Int).Value = entity.Id;
            insertCommand.Parameters.Add("@username", SqlDbType.VarChar).Value = entity.Username;
        }

        private class FindAllUsers : IStatementSource
        {
            public string Sql => "SELECT " + Columns +
                                 " FROM Users ";

            public IList<string> Parameters => new List<string>();
        }
    }
}