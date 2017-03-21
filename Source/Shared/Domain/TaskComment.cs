using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Shared.Domain
{
    /// <summary>
    /// A comment to a task
    /// </summary>
    [Serializable]
    public sealed class TaskComment : Entity
    {
        private readonly List<TaskComment> replies = new List<TaskComment>();

        /// <summary>
        /// Create a task comment for a task.
        /// </summary>
        /// <param name="comment">The details of the comment.</param>
        /// <param name="taskId">Related task.</param>
        /// <param name="commenterId">The user who created this comment.</param>
        /// <param name="parentComment">If this comment is replying to a previous comment, that comment.</param>
        public TaskComment(string comment, int taskId, int commenterId, TaskComment parentComment)
        {
            Comment = comment;
            ParentComment = parentComment;
            TaskId = taskId;
            CommenterId = commenterId;
        }

        public TaskComment(int id, [NotNull] TaskComment incompleteTaskComment, DateTime timePosted) : base(id)
        {
            ParentComment = incompleteTaskComment.ParentComment;

            Comment = incompleteTaskComment.Comment;
            TaskId = incompleteTaskComment.TaskId;
            CommenterId = incompleteTaskComment.CommenterId;
            TimePosted = timePosted;
        }

        /// <summary>
        /// Details of the comment.
        /// </summary>
        public string Comment { get; }

        public int CommenterId { get; }
        public DateTime TimePosted { get; }

        /// <summary>
        /// If null then has no parent.
        /// </summary>
        public TaskComment ParentComment { get; }

        public IEnumerable<TaskComment> Replies => replies;

        public int TaskId { get; }

        public void AddReply(TaskComment taskComment)
        {
            replies.Add(taskComment);
        }
    }
}