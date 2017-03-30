using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using JetBrains.Annotations;
using Shared.Domain;

namespace Shared.Persistence
{
    /// <summary>
    /// A mapper for <see cref="Task" />s.
    /// </summary>
    [UsedImplicitly]
    internal sealed class TaskMapper : EntityMapper<Task>
    {
        private readonly TaskCategoryMapper taskCategoryMapper = new TaskCategoryMapper();

        protected override IEnumerable<string> Columns => new List<string> { "Id", "BandId", "Title", "Description", "AssignedUserId", "IsCompleted", "Points", "JamId", "TaskCategoryId" };
        protected override EntityTable Table => EntityTable.Tasks;

        private static int NullableColumnToInt(int? possibleNullColumn)
        {
            var columnValue = 0;

            if (possibleNullColumn.HasValue)
            {
                columnValue = possibleNullColumn.Value;
            }

            return columnValue;
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

        protected override void AddSpecificParameters(Task entity, SqlCommand insertCommand)
        {
            insertCommand.Parameters.Add("@bandId", SqlDbType.Int).Value = entity.BandId;
            insertCommand.Parameters.Add("@title", SqlDbType.VarChar).Value = entity.Title;
            insertCommand.Parameters.Add("@description", SqlDbType.VarChar).Value = entity.Description;
            insertCommand.Parameters.Add("@assignedUserId", SqlDbType.Int).Value = entity.HasAssignedUser ? (object) entity.AssignedUserId : DBNull.Value;
            insertCommand.Parameters.Add("@isCompleted", SqlDbType.Bit).Value = entity.IsCompleted;
            insertCommand.Parameters.Add("@points", SqlDbType.Int).Value = entity.Points;
            insertCommand.Parameters.Add("@jamId", SqlDbType.Int).Value = entity.IsInJam ? (object) entity.JamId : DBNull.Value;
            insertCommand.Parameters.Add("@taskCategoryId", SqlDbType.Int).Value = taskCategoryMapper.GetCategoryId(entity.Category);
        }
    }
}