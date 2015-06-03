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
        private readonly string title;

        private readonly List<TaskComment> replies = new List<TaskComment>();
        private readonly TaskComment parentComment;

        /// <summary>
        /// Create a task comment for a task.
        /// </summary>
        /// <param name="title">Title of the comment.</param>
        /// <param name="comment">The details of the comment.</param>
        /// <param name="parentComment">If this comment is replying to a previous comment, that comment.</param>
        public TaskComment(string title, string comment, TaskComment parentComment)
        {
            this.title = title;
            this.comment = comment;
            this.parentComment = parentComment;
        }

        public TaskComment(int id, TaskComment incompleteTaskComment) : base(id)
        {
            Contract.Requires(id > 0);
            Contract.Requires(incompleteTaskComment != null);

            parentComment = incompleteTaskComment.ParentComment;

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
    }
}