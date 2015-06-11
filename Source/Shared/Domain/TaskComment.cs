using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Shared.Domain
{
    /// <summary>
    /// A comment to a task
    /// </summary>
    [Serializable]
    public sealed class TaskComment : Entity
    {
        private readonly string comment;
        private readonly int commenterId;
        private readonly TaskComment parentComment;
        private readonly List<TaskComment> replies = new List<TaskComment>();
        private readonly int taskId;
        private readonly DateTime timePosted;

        /// <summary>
        /// Create a task comment for a task.
        /// </summary>
        /// <param name="comment">The details of the comment.</param>
        /// <param name="taskId">Related task.</param>
        /// <param name="commenterId">The user who created this comment.</param>
        /// <param name="parentComment">If this comment is replying to a previous comment, that comment.</param>
        public TaskComment(string comment, int taskId, int commenterId, TaskComment parentComment)
        {
            this.comment = comment;
            this.parentComment = parentComment;
            this.taskId = taskId;
            this.commenterId = commenterId;
        }

        public TaskComment(int id, TaskComment incompleteTaskComment, DateTime timePosted) : base(id)
        {
            Contract.Requires(id > 0);
            Contract.Requires(incompleteTaskComment != null);

            parentComment = incompleteTaskComment.ParentComment;

            comment = incompleteTaskComment.Comment;
            taskId = incompleteTaskComment.taskId;
            commenterId = incompleteTaskComment.commenterId;
            this.timePosted = timePosted;
        }

        /// <summary>
        /// Details of the comment.
        /// </summary>
        public string Comment
        {
            get { return comment; }
        }

        public int CommenterId
        {
            get { return commenterId; }
        }

        public DateTime TimePosted
        {
            get { return timePosted; }
        }

        /// <summary>
        /// If null then has no parent.
        /// </summary>
        public TaskComment ParentComment
        {
            get { return parentComment; }
        }

        public IEnumerable<TaskComment> Replies
        {
            get { return replies; }
        }

        public int TaskId
        {
            get { return taskId; }
        }

        public void AddReply(TaskComment taskComment)
        {
            replies.Add(taskComment);
        }
    }
}