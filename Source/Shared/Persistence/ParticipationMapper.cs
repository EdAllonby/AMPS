using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Shared.Domain;

namespace Shared.Persistence
{
    /// <summary>
    /// A mapper for <see cref="Participation" />s.
    /// </summary>
    internal sealed class ParticipationMapper : EntityMapper<Participation>
    {
        /// <summary>
        /// Columns for Participation.
        /// </summary>
        private const string Columns = " Id, UserId, BandId, IsLeader ";

        protected override string FindStatement => "SELECT " + Columns +
                                                   " FROM Participations" +
                                                   " WHERE Id = @id ";

        protected override string InsertStatement => "INSERT INTO Participations VALUES (@id,@userId,@bandId,@isLeader)";

        public override bool UpdateEntity(Participation entity)
        {
            string updateParticipationQuery = $"UPDATE Participations SET Id=@id,UserId=@userId,BandId=@bandId,IsLeader=@isLeader WHERE Id = {entity.Id}";
            int rowsUpdated;

            using (var databaseConnection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(updateParticipationQuery, databaseConnection))
            {
                DoInsert(entity, command);

                databaseConnection.Open();
                rowsUpdated = command.ExecuteNonQuery();
                databaseConnection.Close();
            }

            return rowsUpdated == 1;
        }

        public override IEnumerable<Participation> GetAllEntities()
        {
            return FindMany(new FindAllParticipations());
        }

        protected override bool DoDelete(int entityId)
        {
            return DeleteEntity("Participations", entityId);
        }

        protected override Participation DoLoad(int id, SqlDataReader reader)
        {
            int userId = reader.GetInt32(reader.GetOrdinal("UserId"));
            int bandId = reader.GetInt32(reader.GetOrdinal("BandId"));
            bool isLeader = reader.GetBoolean(reader.GetOrdinal("IsLeader"));

            var participation = new Participation(id, userId, bandId, isLeader);

            Log.DebugFormat("Participation with Id {0} retrieved from Database.", participation.Id);

            return participation;
        }

        protected override void DoInsert(Participation entity, SqlCommand insertCommand)
        {
            insertCommand.Parameters.Add("@id", SqlDbType.Int).Value = entity.Id;
            insertCommand.Parameters.Add("@userId", SqlDbType.Int).Value = entity.User.Id;
            insertCommand.Parameters.Add("@bandId", SqlDbType.Int).Value = entity.Band.Id;
            insertCommand.Parameters.Add("@isLeader", SqlDbType.Bit).Value = entity.IsLeader;
        }

        private class FindAllParticipations : IStatementSource
        {
            public string Sql => "SELECT " + Columns +
                                 " FROM Participations";

            public IList<string> Parameters => new List<string>();
        }
    }
}