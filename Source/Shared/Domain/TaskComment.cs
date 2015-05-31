using System;
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
        private readonly int parentCommentId;
        private readonly string title;

        /// <summary>
        /// Create a task comment for a task.
        /// </summary>
        /// <param name="title">Title of the comment.</param>
        /// <param name="comment">The details of the comment.</param>
        /// <param name="parentCommentId">If this comment is replying to a previous comment.</param>
        public TaskComment(string title, string comment, int parentCommentId = 0)
        {
            this.parentCommentId = parentCommentId;

            this.title = title;
            this.comment = comment;
        }

        public TaskComment(int id, TaskComment incompleteTaskComment) : base(id)
        {
            Contract.Requires(id > 0);
            Contract.Requires(incompleteTaskComment != null);

            parentCommentId = incompleteTaskComment.ParentCommentId;

            title = incompleteTaskComment.Title;
            comment = incompleteTaskComment.Comment;
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