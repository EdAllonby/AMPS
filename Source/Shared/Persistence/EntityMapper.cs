using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Shared.Domain;

namespace Shared.Persistence
{
    /// <summary>
    /// Moves data between entities and a database table.
    /// </summary>
    /// <typeparam name="T">The <see cref="Entity" /> that is mapped.</typeparam>
    public abstract class EntityMapper<T> where T : Entity
    {
        /// <summary>
        /// Logging for the mapper.
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(EntityMapper<T>));

        /// <summary>
        /// The database connection string.
        /// </summary>
        protected readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

        /// <summary>
        /// Loaded entities to reduce database calls.
        /// </summary>
        private readonly IDictionary<int, T> loadedEntitiesIndexedById = new ConcurrentDictionary<int, T>();

        /// <summary>
        /// The <see cref="Entity" /> find statement.
        /// </summary>
        protected abstract string FindStatement { get; }

        /// <summary>
        /// The <see cref="Entity" /> insert statement
        /// </summary>
        protected abstract string InsertStatement { get; }

        /// <summary>
        /// Updates an <see cref="Entity" />.
        /// </summary>
        /// <param name="entity">The updated <see cref="Entity" />.</param>
        /// <returns>If the update was successful.</returns>
        public abstract bool UpdateEntity(T entity);

        /// <summary>
        /// Delete an <see cref="Entity" /> from the map.
        /// </summary>
        /// <param name="entityId">The <see cref="Entity" /> Id to delete.</param>
        /// <returns>If the delete was successful.</returns>
        public bool DeleteEntity(int entityId)
        {
            return DoDelete(entityId);
        }

        /// <summary>
        /// Get all <see cref="Entity" /> objects from the data map.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<T> GetAllEntities();

        /// <summary>
        /// Gets an <see cref="Entity" /> from the Database by its Id.
        /// </summary>
        /// <param name="id">The Id of the <see cref="Entity" /> to get.</param>
        /// <returns>The <see cref="Entity" /> from the Database.</returns>
        public T GetEntityById(int id)
        {
            T entity;

            if (loadedEntitiesIndexedById.TryGetValue(id, out entity))
            {
                return entity;
            }

            using (var databaseConnection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(FindStatement, databaseConnection))
            {
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                databaseConnection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            entity = Load(reader);
                        }
                    }
                }
            }

            return entity;
        }

        /// <summary>
        /// Insert an <see cref="Entity" /> into the data map.
        /// </summary>
        /// <param name="entity">The <see cref="Entity" /> to insert.</param>
        /// <returns>Whether the insert was successful.</returns>
        public bool InsertEntity(T entity)
        {
            using (var databaseConnection = new SqlConnection(ConnectionString))
            using (var insertCommand = new SqlCommand(InsertStatement, databaseConnection))
            {
                databaseConnection.Open();

                DoInsert(entity, insertCommand);

                int rowsUpdated = insertCommand.ExecuteNonQuery();

                loadedEntitiesIndexedById.Add(entity.Id, entity);

                return rowsUpdated == 1;
            }
        }

        /// <summary>
        /// Delete an <see cref="Entity" /> from a table.
        /// </summary>
        /// <param name="entityId">The <see cref="Entity" /> to delete.</param>
        /// <returns>If the delete was successful.</returns>
        protected abstract bool DoDelete(int entityId);

        /// <summary>
        /// Deletes an <see cref="Entity" />.
        /// </summary>
        /// <param name="tableName">The tablename to delete the entity from.</param>
        /// <param name="entityId">The <see cref="Entity" /> to delete.</param>
        /// <returns>If the delete was successful.</returns>
        protected bool DeleteEntity(string tableName, int entityId)
        {
            const string DeleteEntityQuery = "DELETE FROM @tableName WHERE Id = @id";
            int rowsUpdated;

            using (var databaseConnection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(DeleteEntityQuery, databaseConnection))
            {
                command.Parameters.Add("@tableName", SqlDbType.Int).Value = tableName;
                command.Parameters.Add("@id", SqlDbType.Int).Value = entityId;

                databaseConnection.Open();
                rowsUpdated = command.ExecuteNonQuery();
                databaseConnection.Close();
            }

            return rowsUpdated == 1;
        }

        /// <summary>
        /// Prepare the insert command.
        /// </summary>
        /// <param name="entity">The <paramref name="entity" /> to insert.</param>
        /// <param name="insertCommand">The insert command.</param>
        protected abstract void DoInsert(T entity, SqlCommand insertCommand);

        /// <summary>
        /// Attempt to get an entity based on the reader and its Id.
        /// </summary>
        /// <param name="id">The entity's Id.</param>
        /// <param name="reader">The reader containing the entity.</param>
        /// <returns></returns>
        protected abstract T DoLoad(int id, SqlDataReader reader);

        /// <summary>
        /// Creates an entity based on the reader.
        /// </summary>
        /// <param name="reader">The database reader.</param>
        /// <returns>The entity based on the reader results.</returns>
        private T Load(SqlDataReader reader)
        {
            int entityId = reader.GetInt32(0);

            T entity;

            if (loadedEntitiesIndexedById.TryGetValue(entityId, out entity))
            {
                return entity;
            }

            entity = DoLoad(entityId, reader);

            loadedEntitiesIndexedById.Add(entityId, entity);

            return entity;
        }

        /// <summary>
        /// Find many <see cref="Entity" /> objects based on a search query specified in <see cref="IStatementSource" />.
        /// </summary>
        /// <param name="source">The search query.</param>
        /// <returns>A list of <see cref="Entity" /> objects that match the search query.</returns>
        protected List<T> FindMany(IStatementSource source)
        {
            using (var databaseConnection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(source.Sql, databaseConnection))
            {
                for (var i = 0; i < source.Parameters.Count; i++)
                {
                    command.Parameters.Insert(i + 1, source.Parameters[i]);
                }

                databaseConnection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return LoadAll(reader);
                }
            }
        }

        /// <summary>
        /// Creates a list of entities found in the reader.
        /// </summary>
        /// <param name="reader">The reader containing potential entities.</param>
        /// <returns>The entities in the reader.</returns>
        private List<T> LoadAll(SqlDataReader reader)
        {
            var entities = new List<T>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    entities.Add(Load(reader));
                }
            }

            return entities;
        }
    }
}