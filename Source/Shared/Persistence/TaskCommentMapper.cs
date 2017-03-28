using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Shared.Domain;

namespace Shared.Persistence
{
    internal class TaskCommentMapper : EntityMapper<TaskComment>
    {
        protected override List<string> Columns => new List<string> { "Id", "TaskId", "CommenterId", "ParentCommentId", "Comment" };
        protected override EntityTable Table => EntityTable.TaskComments;

        public override bool UpdateEntity(TaskComment entity)
        {
            string updateTaskQuery = $"UPDATE TaskComments SET @id,@taskId,@commenterId,@parentCommentId,@comment WHERE Id = {entity.Id}";
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

        protected override bool DoDelete(int entityId)
        {
            return DeleteEntity("TaskComments", entityId);
        }

        protected override void DoInsert(TaskComment entity, SqlCommand insertCommand)
        {
            insertCommand.Parameters.Add("@taskId", SqlDbType.Int).Value = entity.Task.Id;
            insertCommand.Parameters.Add("@commenterId", SqlDbType.VarChar).Value = entity.Commenter.Id;
            insertCommand.Parameters.Add("@parentCommentId", SqlDbType.VarChar).Value = entity.ParentComment != null ? (object) entity.ParentComment.Id : DBNull.Value;
            insertCommand.Parameters.Add("@comment", SqlDbType.Int).Value = entity.Comment;
        }

        protected override TaskComment DoLoad(int id, SqlDataReader reader)
        {
            int taskId = reader.GetInt32(reader.GetOrdinal("TaskId"));
            int commenterId = reader.GetInt32(reader.GetOrdinal("CommenterId"));
            int? parentCommentId = reader.GetNullableInt(reader.GetOrdinal("ParentCommentId"));

            TaskComment parentComment = null;

            if (parentCommentId != null)
            {
                parentComment = GetEntityById(parentCommentId.Value);
            }

            string comment = reader.GetString(reader.GetOrdinal("Comment"));

            var taskComment = new TaskComment(id, new TaskComment(comment, taskId, commenterId, parentComment), DateTime.Now);

            Log.DebugFormat("TaskComment with Id {0} retrieved from Database.", taskComment.Id);

            return taskComment;
        }
    }
}