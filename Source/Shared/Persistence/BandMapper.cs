﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Shared.Domain;

namespace Shared.Persistence
{
    /// <summary>
    /// A mapper for <see cref="Band" />s.
    /// </summary>
    internal sealed class BandMapper : EntityMapper<Band>
    {
        /// <summary>
        /// Columns for Band.
        /// </summary>
        protected override List<string> Columns => new List<string> { "Id", "Name" };

        protected override EntityTable Table => EntityTable.Bands;

        public override bool UpdateEntity(Band entity)
        {
            string updateBandQuery = $"UPDATE Bands SET Id=@id,Name=@name WHERE Id = {entity.Id}";
            int rowsUpdated;

            using (var databaseConnection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(updateBandQuery, databaseConnection))
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
            return DeleteEntity("Bands", entityId);
        }

        protected override Band DoLoad(int id, SqlDataReader reader)
        {
            string bandName = reader.GetString(reader.GetOrdinal("Name"));
            var band = new Band(id, bandName);

            Log.DebugFormat("Band with Id {0} retrieved from Database.", band.Id);

            return band;
        }

        protected override void DoInsert(Band entity, SqlCommand insertCommand)
        {
            insertCommand.Parameters.Add("@name", SqlDbType.NChar).Value = entity.Name;
        }
    }
}