using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using JetBrains.Annotations;
using Shared.Domain;

namespace Shared.Persistence
{
    /// <summary>
    /// A mapper for <see cref="Participation" />s.
    /// </summary>
    [UsedImplicitly]
    internal sealed class ParticipationMapper : EntityMapper<Participation>
    {
        /// <summary>
        /// Columns for Participation.
        /// </summary>
        protected override IEnumerable<string> Columns => new List<string> { "Id", "UserId", "BandId", "IsLeader" };

        protected override EntityTable Table => EntityTable.Participations;

        protected override Participation DoLoad(int id, SqlDataReader reader)
        {
            int userId = reader.GetInt32(reader.GetOrdinal("UserId"));
            int bandId = reader.GetInt32(reader.GetOrdinal("BandId"));
            bool isLeader = reader.GetBoolean(reader.GetOrdinal("IsLeader"));

            var participation = new Participation(id, userId, bandId, isLeader);

            Log.DebugFormat("Participation with Id {0} retrieved from Database.", participation.Id);

            return participation;
        }

        protected override void AddSpecificParameters(Participation entity, SqlCommand insertCommand)
        {
            insertCommand.Parameters.Add("@userId", SqlDbType.Int).Value = entity.User.Id;
            insertCommand.Parameters.Add("@bandId", SqlDbType.Int).Value = entity.BandId;
            insertCommand.Parameters.Add("@isLeader", SqlDbType.Bit).Value = entity.IsLeader;
        }
    }
}