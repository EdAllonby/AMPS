using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Shared.Domain;

namespace Shared.Persistence
{
    /// <summary>
    /// A mapper for <see cref="Jam" />s.
    /// </summary>
    internal sealed class JamMapper : EntityMapper<Jam>
    {
        /// <summary>
        /// Columns for Jams.
        /// </summary>
        protected override List<string> Columns => new List<string> { "Id", "BandId", "EndDate", "IsActive" };

        protected override EntityTable Table => EntityTable.Jams;
        public override bool UpdateEntity(Jam entity)
        {
            string updateJamQuery = $"UPDATE Jams SET Id=@id,BandId=@bandId,EndDate=@endDate,IsActive=@isActive WHERE Id = {entity.Id}";
            int rowsUpdated;

            using (var databaseConnection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(updateJamQuery, databaseConnection))
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

        protected override void DoInsert(Jam entity, SqlCommand insertCommand)
        {
            insertCommand.Parameters.Add("@bandId", SqlDbType.Int).Value = entity.Band.Id;
            insertCommand.Parameters.Add("@endDate", SqlDbType.DateTime).Value = entity.JamEndDate;
            insertCommand.Parameters.Add("@isActive", SqlDbType.Bit).Value = entity.IsActive;
        }

        protected override Jam DoLoad(int id, SqlDataReader dataRecord)
        {
            int bandId = dataRecord.GetInt32(dataRecord.GetOrdinal("BandId"));
            DateTime jamEndDate = dataRecord.GetDateTime(dataRecord.GetOrdinal("EndDate"));
            bool isActive = dataRecord.GetBoolean(dataRecord.GetOrdinal("IsActive"));

            var jam = new Jam(id, bandId, jamEndDate) { IsActive = isActive };

            Log.DebugFormat("Jam with Id {0} retrieved from Database.", jam.Id);

            return jam;
        }
    }
}