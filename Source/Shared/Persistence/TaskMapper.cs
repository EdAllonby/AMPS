using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Shared.Domain;

namespace Shared.Persistence
{
    /// <summary>
    /// A mapper for <see cref="Task" />s.
    /// </summary>
    internal sealed class TaskMapper : EntityMapper<Task>
    {
        private const string Columns = " Id, BandId, Title, Description, AssignedUserId, IsCompleted, Points, JamId, TaskCategoryId ";

        private readonly TaskCategoryMapper taskCategoryMapper = new TaskCategoryMapper();

        private readonly EntityMapper<TaskComment> taskCommentMapper = new TaskCommentMapper();

        protected override string FindStatement => "SELECT " + Columns +
                                                   " FROM Tasks" +
                                                   " WHERE Id = @id ";

        protected override string InsertStatement => "INSERT INTO Tasks VALUES (@id,@bandId,@title,@description,@assignedUserId,@isCompleted,@points,@jamId,@taskCategoryId)";

        public override bool UpdateEntity(Task entity)
        {
            string updateTaskQuery = $"UPDATE Tasks SET Id=@id,BandId=@bandId,Title=@title,Description=@description,AssignedUserId=@assignedUserId,IsCompleted=@isCompleted,Points=@points,JamId=@jamId,TaskCategoryId=@taskCategoryId WHERE Id = {entity.Id}";
            int rowsUpdated;

            using (var databaseConnection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(updateTaskQuery, databaseConnection))
            {
                DoInsert(entity, command);

                databaseConnection.Open();
                rowsUpdated = command.ExecuteNonQuery();
                databaseConnection.Close();
            }

            return rowsUpdated == 1;
        }

        public override IEnumerable<Task> GetAllEntities()
        {
            List<Task> allTasks = FindMany(new FindAllTasks());

            List<TaskComment> allComments = taskCommentMapper.GetAllEntities().ToList();

            foreach (Task task in allTasks)
            {
                IEnumerable<TaskComment> taskComments = allComments.Where(comment => comment.Task.Id.Equals(task.Id));

                foreach (TaskComment taskComment in taskComments)
                {
                    task.AddCommentToRelevantParent(taskComment);
                }
            }

            return allTasks;
        }

        protected override bool DoDelete(int entityId)
        {
            return DeleteEntity("Tasks", entityId);
        }

        protected override Task DoLoad(int id, SqlDataReader reader)
        {
            int bandId = reader.GetInt32(reader.GetOrdinal("BandId"));
            string title = reader.GetString(reader.GetOrdinal("Title"));
            string description = reader.GetString(reader.GetOrdinal("Description"));
            int? possibleAssignedUserId = reader.GetNullableInt(reader.GetOrdinal("AssignedUserId"));
            int assignedUserId = NullableColumnToInt(possibleAssignedUserId);
            bool isCompleted = reader.GetBoolean(reader.GetOrdinal("IsCompleted"));
            int points = reader.GetInt32(reader.GetOrdinal("Points"));
            int? possibleJamId = reader.GetNullableInt(reader.GetOrdinal("JamId"));
            int jamId = NullableColumnToInt(possibleJamId);
            TaskCategory taskCategory = taskCategoryMapper.GetCategory(reader.GetInt32(reader.GetOrdinal("TaskCategoryId")));

            var task = new Task(id, new Task(title, description, points, bandId, assignedUserId, taskCategory)) { IsCompleted = isCompleted };

            if (jamId > 0)
            {
                task.AssignTaskToJam(jamId);
            }

            Log.DebugFormat("Task with Id {0} retrieved from Database.", task.Id);

            return task;
        }

        protected override void DoInsert(Task entity, SqlCommand insertCommand)
        {
            insertCommand.Parameters.Add("@id", SqlDbType.Int).Value = entity.Id;
            insertCommand.Parameters.Add("@bandId", SqlDbType.Int).Value = entity.BandId;
            insertCommand.Parameters.Add("@title", SqlDbType.VarChar).Value = entity.Title;
            insertCommand.Parameters.Add("@description", SqlDbType.VarChar).Value = entity.Description;
            insertCommand.Parameters.Add("@assignedUserId", SqlDbType.Int).Value = entity.HasAssignedUser ? (object) entity.AssignedUserId : DBNull.Value;
            insertCommand.Parameters.Add("@isCompleted", SqlDbType.Bit).Value = entity.IsCompleted;
            insertCommand.Parameters.Add("@points", SqlDbType.Int).Value = entity.Points;
            insertCommand.Parameters.Add("@jamId", SqlDbType.Int).Value = entity.IsInJam ? (object) entity.JamId : DBNull.Value;
            insertCommand.Parameters.Add("@taskCategoryId", SqlDbType.Int).Value = taskCategoryMapper.GetCategoryId(entity.Category);
        }

        private static int NullableColumnToInt(int? possibleNullColumn)
        {
            var columnValue = 0;

            if (possibleNullColumn.HasValue)
            {
                columnValue = possibleNullColumn.Value;
            }

            return columnValue;
        }

        private class FindAllTasks : IStatementSource
        {
            public string Sql => "SELECT " + Columns +
                                 " FROM Tasks";

            public IList<string> Parameters => new List<string>();
        }
    }
}