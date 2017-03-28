using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using JetBrains.Annotations;
using Shared.Domain;

namespace Shared.Persistence
{
    /// <summary>
    /// A mapper for <see cref="Jam" />s.
    /// </summary>
    [UsedImplicitly]
    internal sealed class JamMapper : EntityMapper<Jam>
    {
        /// <summary>
        /// Columns for Jams.
        /// </summary>
        protected override IEnumerable<string> Columns => new List<string> { "Id", "BandId", "EndDate", "IsActive" };

        protected override EntityTable Table => EntityTable.Jams;

        protected override void AddSpecificParameters(Jam entity, SqlCommand insertCommand)
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