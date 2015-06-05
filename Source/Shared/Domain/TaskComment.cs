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

        private readonly List<TaskComment> replies = new List<TaskComment>();
        private readonly TaskComment parentComment;
        private readonly int taskId;

        /// <summary>
        /// Create a task comment for a task.
        /// </summary>
        /// <param name="comment">The details of the comment.</param>
        /// <param name="taskId">Related task.</param>
        /// <param name="parentComment">If this comment is replying to a previous comment, that comment.</param>
        public TaskComment(string comment, int taskId, TaskComment parentComment)
        {
            this.comment = comment;
            this.parentComment = parentComment;
            this.taskId = taskId;
        }

        public TaskComment(int id, TaskComment incompleteTaskComment) : base(id)
        {
            Contract.Requires(id > 0);
            Contract.Requires(incompleteTaskComment != null);

            parentComment = incompleteTaskComment.ParentComment;

            comment = incompleteTaskComment.Comment;
            taskId = incompleteTaskComment.taskId;
        }

        /// <summary>
        /// Details of the comment.
        /// </summary>
        public string Comment
        {
            get { return comment; }
        }

        /// <summary>
        /// If equal 0, no parent.
        /// </summary>
        public TaskComment ParentComment
        {
            get { return parentComment; }
        }

        public void AddReply(TaskComment taskComment)
        {
            replies.Add(taskComment);
        }

        public IEnumerable<TaskComment> Replies
        {
            get { return replies; }
        }

        public int TaskId
        {
            get { return taskId; }
        }
    }
}