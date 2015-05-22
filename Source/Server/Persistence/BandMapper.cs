using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Shared.Domain;

namespace Server.Persistence
{
    /// <summary>
    /// A mapper for <see cref="Band" />s.
    /// </summary>
    internal sealed class BandMapper : EntityMapper<Band>
    {
        /// <summary>
        /// Columns for Band.
        /// </summary>
        private const string Columns = " Id, Name ";

        protected override string FindStatement
        {
            get
            {
                return "SELECT " + Columns +
                       " FROM Bands" +
                       " WHERE Id = @id ";
            }
        }

        protected override string InsertStatement
        {
            get { return "INSERT INTO Bands VALUES (@id,@name)"; }
        }

        public override bool UpdateEntity(Band entity)
        {
            var updateBandQuery = string.Format("UPDATE Bands SET Id=@id,Name=@name WHERE Id = {0}", entity.Id);
            int rowsUpdated;

            using (var databaseConnection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(updateBandQuery, databaseConnection))
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
            var bandName = reader.GetString(reader.GetOrdinal("Name"));
            var band = new Band(id, bandName);

            Log.DebugFormat("Band with Id {0} retrieved from Database.", band.Id);

            return band;
        }

        public override IEnumerable<Band> GetAllEntities()
        {
            return FindMany(new FindAllBands());
        }

        protected override void DoInsert(Band entity, SqlCommand insertCommand)
        {
            insertCommand.Parameters.Add("@id", SqlDbType.Int).Value = entity.Id;
            insertCommand.Parameters.Add("@name", SqlDbType.NChar).Value = entity.Name;
        }

        private class FindAllBands : IStatementSource
        {
            public string Sql
            {
                get
                {
                    return "SELECT " + Columns +
                           " FROM Bands";
                }
            }

            public IList<string> Parameters
            {
                get
                {
                    {
                        return new List<string>();
                    }
                }
            }
        }
    }
}