using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using JetBrains.Annotations;
using Shared.Domain;

namespace Shared.Persistence
{
    /// <summary>
    /// A mapper for <see cref="User" />s.
    /// </summary>
    [UsedImplicitly]
    internal sealed class UserMapper : EntityMapper<User>
    {
        /// <summary>
        /// Columns for <see cref="User" />.
        /// </summary>
        protected override IEnumerable<string> Columns => new List<string> { "Id", "Username" };

        protected override EntityTable Table => EntityTable.Users;

        protected override User DoLoad(int id, SqlDataReader reader)
        {
            string userName = reader.GetString(reader.GetOrdinal("Username"));

            var user = new User(id, userName, new ConnectionStatus(id, ConnectionStatus.Status.Disconnected));

            Log.DebugFormat("User with Id {0} retrieved from Database.", user.Id);

            return user;
        }

        protected override void AddSpecificParameters(User entity, SqlCommand insertCommand)
        {
            insertCommand.Parameters.Add("@username", SqlDbType.VarChar).Value = entity.Username;
        }
    }
}