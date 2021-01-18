using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using log4net;
using Shared.Domain;
using Utility;

namespace Shared.Persistence
{
    /// <summary>
    /// Moves data between entities and a database table.
    /// </summary>
    /// <typeparam name="TEntity">The <see cref="Entity" /> that is mapped.</typeparam>
    public abstract class EntityMapper<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// Logging for the mapper.
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(EntityMapper<TEntity>));

        /// <summary>
        /// The database connection string.
        /// </summary>
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

        /// <summary>
        /// Loaded entities to reduce database calls.
        /// </summary>
        private readonly IDictionary<int, TEntity> loadedEntitiesIndexedById = new ConcurrentDictionary<int, TEntity>();


        protected abstract IEnumerable<string> Columns { get; }

        protected abstract EntityTable Table { get; }

        private IEnumerable<string> EntityColumns => new List<string>(Columns) { "CreatedDate", "UpdatedDate" };

        private string CommaSeperatedEntityColumns => string.Join(", ", EntityColumns);

        private IEnumerable<string> EntityColumnsAsParameters => EntityColumns.Select(c => $"@{c.FirstCharacterToLower()}");

        /// <summary>
        /// Gets an <see cref="Entity" /> from the Database by its Id.
        /// </summary>
        /// <param name="id">The Id of the <see cref="Entity" /> to get.</param>
        /// <returns>The <see cref="Entity" /> from the Database.</returns>
        public TEntity GetEntityById(int id)
        {
            if (loadedEntitiesIndexedById.TryGetValue(id, out var entity))
            {
                return entity;
            }

            string findStatement = $"SELECT {string.Join(", ", EntityColumns)} FROM {Table} where Id = @id";

            using var databaseConnection = new SqlConnection(connectionString);
            using var command = new SqlCommand(findStatement, databaseConnection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;

            databaseConnection.Open();

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    entity = Load(reader);
                }
            }

            return entity;
        }

        /// <summary>
        /// Insert an <see cref="Entity" /> into the data map.
        /// </summary>
        /// <param name="entity">The <see cref="Entity" /> to insert.</param>
        /// <returns>Whether the insert was successful.</returns>
        public bool InsertEntity(TEntity entity)
        {
            string insertStatement = $"INSERT INTO {Table} VALUES ({string.Join(", ", EntityColumnsAsParameters)})";

            using var databaseConnection = new SqlConnection(connectionString);
            using var insertCommand = new SqlCommand(insertStatement, databaseConnection);
            databaseConnection.Open();

            AddEntityParameters(entity, insertCommand);

            AddSpecificParameters(entity, insertCommand);

            int rowsUpdated = insertCommand.ExecuteNonQuery();

            loadedEntitiesIndexedById.Add(entity.Id, entity);

            return rowsUpdated == 1;
        }

        /// <summary>
        /// Updates an <see cref="Entity" />.
        /// </summary>
        /// <param name="entity">The updated <see cref="Entity" />.</param>
        /// <returns>If the update was successful.</returns>
        public bool UpdateEntity(TEntity entity)
        {
            IEnumerable<string> columnsAndParameters = EntityColumns.Zip(EntityColumnsAsParameters, (first, second) => first + "=" + second);
            string updateStatement = $"UPDATE {Table} SET {string.Join(", ", columnsAndParameters)} WHERE Id={entity.Id}";

            int rowsUpdated;

            using var databaseConnection = new SqlConnection(connectionString);
            using var command = new SqlCommand(updateStatement, databaseConnection);
            AddParameters(entity, command);

            databaseConnection.Open();
            rowsUpdated = command.ExecuteNonQuery();
            databaseConnection.Close();

            return rowsUpdated == 1;
        }

        /// <summary>
        /// Get all <see cref="Entity" /> objects from the data map.
        /// </summary>
        /// <returns></returns>
        public List<TEntity> GetAllEntities()
        {
            string selectAllStatement = $"Select {CommaSeperatedEntityColumns} from {Table}";

            using var databaseConnection = new SqlConnection(connectionString);
            using var command = new SqlCommand(selectAllStatement, databaseConnection);
            databaseConnection.Open();

            using SqlDataReader reader = command.ExecuteReader();
            return LoadAll(reader);
        }

        /// <summary>
        /// Deletes an <see cref="Entity" />.
        /// </summary>
        /// <param name="entityId">The <see cref="Entity" /> to delete.</param>
        /// <returns>If the delete was successful.</returns>
        public bool DeleteEntity(int entityId)
        {
            const string DeleteEntityQuery = "DELETE FROM @tableName WHERE Id = @id";

            using var databaseConnection = new SqlConnection(connectionString);
            using var command = new SqlCommand(DeleteEntityQuery, databaseConnection);
            command.Parameters.Add("@tableName", SqlDbType.Int).Value = Table;
            command.Parameters.Add("@id", SqlDbType.Int).Value = entityId;

            databaseConnection.Open();
            int rowsUpdated = command.ExecuteNonQuery();
            databaseConnection.Close();

            return rowsUpdated == 1;
        }

        /// <summary>
        /// Creates an entity based on the reader.
        /// </summary>
        /// <param name="reader">The database reader.</param>
        /// <returns>The entity based on the reader results.</returns>
        private TEntity Load(SqlDataReader reader)
        {
            int entityId = reader.GetInt32(0);

            if (loadedEntitiesIndexedById.TryGetValue(entityId, out var entity))
            {
                return entity;
            }

            entity = DoLoad(entityId, reader);

            entity.CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"));
            entity.UpdatedDate = reader.GetNullableDateTime(reader.GetOrdinal("UpdatedDate"));

            loadedEntitiesIndexedById.Add(entityId, entity);

            return entity;
        }

        /// <summary>
        /// Creates a list of entities found in the reader.
        /// </summary>
        /// <param name="reader">The reader containing potential entities.</param>
        /// <returns>The entities in the reader.</returns>
        private List<TEntity> LoadAll(SqlDataReader reader)
        {
            var entities = new List<TEntity>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    entities.Add(Load(reader));
                }
            }

            return entities;
        }

        private static void AddEntityParameters(TEntity entity, SqlCommand insertCommand)
        {
            insertCommand.Parameters.Add("@id", SqlDbType.Int).Value = entity.Id;
            insertCommand.Parameters.Add("@createdDate", SqlDbType.DateTime2).Value = entity.CreatedDate;
            insertCommand.Parameters.Add("@updatedDate", SqlDbType.DateTime2).Value = entity.UpdatedDate.HasValue ? (object) entity.UpdatedDate : DBNull.Value;
        }

        /// <summary>
        /// Prepare commands with parameters.
        /// </summary>
        /// <param name="entity">The <paramref name="entity" /> to insert.</param>
        /// <param name="insertCommand">The insert command.</param>
        protected void AddParameters(TEntity entity, SqlCommand insertCommand)
        {
            AddEntityParameters(entity, insertCommand);
            AddSpecificParameters(entity, insertCommand);
        }

        protected abstract void AddSpecificParameters(TEntity entity, SqlCommand insertCommand);

        /// <summary>
        /// Attempt to get an entity based on the reader and its Id.
        /// </summary>
        /// <param name="id">The entity's Id.</param>
        /// <param name="reader">The reader containing the entity.</param>
        /// <returns></returns>
        protected abstract TEntity DoLoad(int id, SqlDataReader reader);
    }
}