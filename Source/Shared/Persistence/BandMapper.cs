using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using JetBrains.Annotations;
using Shared.Domain;

namespace Shared.Persistence
{
    /// <summary>
    /// A mapper for <see cref="Band" />s.
    /// </summary>
    [UsedImplicitly]
    internal sealed class BandMapper : EntityMapper<Band>
    {
        /// <summary>
        /// Columns for Band.
        /// </summary>
        protected override IEnumerable<string> Columns => new List<string> { "Id", "Name" };

        protected override EntityTable Table => EntityTable.Bands;

        protected override Band DoLoad(int id, SqlDataReader reader)
        {
            string bandName = reader.GetString(reader.GetOrdinal("Name"));
            var band = new Band(id, bandName);

            Log.DebugFormat("Band with Id {0} retrieved from Database.", band.Id);

            return band;
        }

        protected override void AddSpecificParameters(Band entity, SqlCommand insertCommand)
        {
            insertCommand.Parameters.Add("@name", SqlDbType.NChar).Value = entity.Name;
        }
    }
}