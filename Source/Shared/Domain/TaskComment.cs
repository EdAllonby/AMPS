using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Shared.Domain
{
    /// <summary>
    /// A comment to a task
    /// </summary>
    [Serializable]
    public sealed class TaskComment : Entity
    {
        private readonly int commenterId;
        private readonly List<TaskComment> replies = new List<TaskComment>();
        private readonly int taskId;

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
            this.taskId = taskId;
            this.commenterId = commenterId;
        }

        public TaskComment(int id, [NotNull] TaskComment incompleteTaskComment, DateTime timePosted) : base(id)
        {
            ParentComment = incompleteTaskComment.ParentComment;

            Comment = incompleteTaskComment.Comment;
            taskId = incompleteTaskComment.taskId;
            commenterId = incompleteTaskComment.commenterId;
            TimePosted = timePosted;
        }

        /// <summary>
        /// Details of the comment.
        /// </summary>
        public string Comment { get; }

        public User Commenter => RepositoryManager.GetRepository<User>().FindEntityById(commenterId);

        public DateTime TimePosted { get; }

        /// <summary>
        /// If null then has no parent.
        /// </summary>
        public TaskComment ParentComment { get; }

        public IEnumerable<TaskComment> Replies => RepositoryManager.GetRepository<TaskComment>().GetAllEntities().Where(x => x.ParentComment.Equals(this));

        public Task Task => RepositoryManager.GetRepository<Task>().FindEntityById(taskId);

        public void AddReply(TaskComment taskComment)
        {
            replies.Add(taskComment);
        }
    }
}