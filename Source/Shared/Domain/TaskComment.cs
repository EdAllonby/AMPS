using System;

namespace Shared.Domain
{
    /// <summary>
    /// A comment to a task
    /// </summary>
    [Serializable]
    public sealed class TaskComment : Entity
    {
        private readonly string comment;
        private readonly string title;
        private readonly int parentCommentId;

        /// <summary>
        /// Create a task comment for a task.
        /// </summary>
        /// <param name="parentCommentId">If this comment is replying to a previous comment.</param>
        /// <param name="title">Title of the comment.</param>
        /// <param name="comment">The details of the comment.</param>
        public TaskComment(int parentCommentId, string title, string comment) : this(title, comment)
        {
            this.parentCommentId = parentCommentId;
        }

        /// <summary>
        /// Create a task comment for a task.
        /// </summary>
        /// <param name="title">Title of the comment.</param>
        /// <param name="comment">The details of the comment.</param>
        public TaskComment(string title, string comment)
        {
            this.title = title;
            this.comment = comment;
        }

        /// <summary>
        /// Title of the comment.
        /// </summary>
        public string Title
        {
            get { return title; }
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
        public int ParentCommentId
        {
            get { return parentCommentId; }
        }
    }
}