using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Shared.Domain;

namespace Server.Persistence
{
    internal sealed class TaskCategoryDataAccess
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

        /// <summary>
        /// Gets the <see cref="TaskCategory" /> from its Id.
        /// </summary>
        /// <param name="categoryId">The category Id.</param>
        /// <returns>The <see cref="TaskCategory" />.</returns>
        public TaskCategory GetCategory(int categoryId)
        {
            TaskCategory taskCategory = TaskCategory.Other;

            var getTaskCategoryQuery = $"SELECT Category FROM TaskCategories where Id = {categoryId}";

            using (var databaseConnection = new SqlConnection(connectionString))
            using (var getTaskCommand = new SqlCommand(getTaskCategoryQuery, databaseConnection))
            {
                databaseConnection.Open();

                using (SqlDataReader reader = getTaskCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            taskCategory = CreateEntityFromRecord(reader);
                        }
                    }
                }
            }

            return taskCategory;
        }

        public int GetCategoryId(TaskCategory category)
        {
            const string GetTaskCategoryIdQuery = "SELECT Id FROM TaskCategories where Category = @taskCategory";

            int categoryId = 0;

            using (var databaseConnection = new SqlConnection(connectionString))
            using (var getTaskCategoryIdCommand = new SqlCommand(GetTaskCategoryIdQuery, databaseConnection))
            {
                getTaskCategoryIdCommand.Parameters.Add("@taskCategory", SqlDbType.VarChar).Value = category.ToString();

                databaseConnection.Open();

                using (SqlDataReader reader = getTaskCategoryIdCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            categoryId = reader.GetInt32(reader.GetOrdinal("Id"));
                        }
                    }
                }
            }

            return categoryId;
        }

        private static TaskCategory CreateEntityFromRecord(IDataRecord dataRecord)
        {
            string taskCategoryString = dataRecord.GetString(dataRecord.GetOrdinal("Category"));

            TaskCategory taskCategory;

            bool didParse = Enum.TryParse(taskCategoryString, out taskCategory);

            if (!didParse)
            {
                taskCategory = TaskCategory.Other;
            }

            return taskCategory;
        }
    }
}