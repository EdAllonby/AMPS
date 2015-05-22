using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Shared.Domain;

namespace Server.Persistence
{
    /// <summary>
    /// A mapper for <see cref="Jam" />s.
    /// </summary>
    internal sealed class JamMapper : EntityMapper<Jam>
    {
        /// <summary>
        /// Columns for Jams.
        /// </summary>
        private const string Columns = " Id, BandId, EndDate, IsActive ";

        protected override string FindStatement
        {
            get
            {
                return "SELECT " + Columns +
                       " FROM Tasks" +
                       " WHERE Id = @id ";
            }
        }

        protected override string InsertStatement
        {
            get { return "INSERT INTO Jams VALUES (@id,@bandId,@endDate,@isActive)"; }
        }

        public override bool UpdateEntity(Jam entity)
        {
            string updateJamQuery = string.Format("UPDATE Jams SET Id=@id,BandId=@bandId,EndDate=@endDate,IsActive=@isActive WHERE Id = {0}", entity.Id);
            int rowsUpdated;

            using (var databaseConnection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(updateJamQuery, databaseConnection))
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
            return DeleteEntity("Jams", entityId);
        }

        public override IEnumerable<Jam> GetAllEntities()
        {
            return FindMany(new FindAllJams());
        }

        protected override void DoInsert(Jam entity, SqlCommand insertCommand)
        {
            insertCommand.Parameters.Add("@id", SqlDbType.Int).Value = entity.Id;
            insertCommand.Parameters.Add("@bandId", SqlDbType.Int).Value = entity.BandId;
            insertCommand.Parameters.Add("@endDate", SqlDbType.DateTime).Value = entity.JamEndDate;
            insertCommand.Parameters.Add("@isActive", SqlDbType.Bit).Value = entity.IsActive;
        }

        protected override Jam DoLoad(int id, SqlDataReader dataRecord)
        {
            int bandId = dataRecord.GetInt32(dataRecord.GetOrdinal("BandId"));
            DateTime jamEndDate = dataRecord.GetDateTime(dataRecord.GetOrdinal("EndDate"));
            bool isActive = dataRecord.GetBoolean(dataRecord.GetOrdinal("IsActive"));

            var jam = new Jam(id, bandId, jamEndDate) {IsActive = isActive};

            Log.DebugFormat("Jam with Id {0} retrieved from Database.", jam.Id);

            return jam;
        }

        private class FindAllJams : IStatementSource
        {
            public string Sql
            {
                get
                {
                    return "SELECT " + Columns +
                           " FROM Jams ";
                }
            }

            public IList<string> Parameters
            {
                get { return new List<string>(); }
            }
        }
    }
}