using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Shared.Repository;

namespace Shared.Domain
{
    /// <summary>
    /// Model's a task entity. A task is used to contain a piece of work needed.
    /// </summary>
    [Serializable]
    public class Task : Entity
    {
        private int assignedUserId;
        private DateTime completedDate = DateTime.MinValue;
        private bool isCompleted;
        private int points;

        /// <summary>
        /// Creates a new <see cref="Task" /> with no assigned Id.
        /// </summary>
        /// <param name="title">The title of the <see cref="Task" /> .</param>
        /// <param name="description">
        /// The description of the <see cref="Task" /> .
        /// </param>
        /// <param name="points">
        /// The number of points the <see cref="Task" /> has.
        /// </param>
        /// <param name="bandId">
        /// The <see cref="Band" /> associated with the <see cref="Task" /> .
        /// </param>
        /// <param name="assignedUserId">
        /// The <see cref="User" /> who needs to complete the
        /// <see cref="Task" /> .
        /// </param>
        /// <param name="category">
        /// The <see cref="Task" /> 's
        /// <see cref="Shared.Domain.Task.Category" /> .
        /// </param>
        public Task([NotNull] string title, [NotNull] string description, int points, int bandId, int assignedUserId, TaskCategory category)
        {
            Title = title;
            Description = description;
            Points = points;
            BandId = bandId;
            AssignedUserId = assignedUserId;
            Category = category;
        }

        /// <summary>
        /// Creates a <see cref="Task" /> with an Id.
        /// </summary>
        /// <param name="id">The assigned <see cref="Task" /> Id.</param>
        /// <param name="incompleteTask">
        /// The previous incomplete <see cref="Task" /> .
        /// </param>
        public Task(int id, [NotNull] Task incompleteTask) : base(id)
        {
            Title = incompleteTask.Title;
            Description = incompleteTask.Description;
            points = incompleteTask.Points;
            BandId = incompleteTask.BandId;
            assignedUserId = incompleteTask.AssignedUserId;
            Category = incompleteTask.Category;
        }

        /// <summary>
        /// Returns whether this task belongs in a <see cref="Jam" /> .
        /// </summary>
        public bool IsInJam => JamId > 0;

        /// <summary>
        /// Returns whether the <see cref="Task" /> has an assigned
        /// <see cref="User" /> .
        /// </summary>
        public bool HasAssignedUser => AssignedUserId > 0;

        /// <summary>
        /// <para>
        /// Returns whether the <see cref="Task" /> has
        /// <see cref="Shared.Domain.Task.points" />
        /// </para>
        /// <para>given to it.</para>
        /// </summary>
        public bool HasPoints => Points > 0;

        /// <summary>
        /// The overview of the <see cref="Task" /> .
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// The detailed <see cref="Description" /> of the
        /// <see cref="Task" /> .
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The number of <see cref="Shared.Domain.Task.points" /> this
        /// <see cref="Task" /> has.
        /// </summary>
        public int Points
        {
            get { return points; }
            set
            {
                if (points >= 0)
                {
                    points = value;
                }
            }
        }

        /// <summary>
        /// The <see cref="Band" /> Id this <see cref="Task" /> is in.
        /// </summary>
        public int BandId { get; }

        /// <summary>
        /// The <see cref="User" /> Id this <see cref="Task" /> is assigned to.
        /// </summary>
        public int AssignedUserId
        {
            get { return assignedUserId; }
            set
            {
                if (assignedUserId >= 0)
                {
                    assignedUserId = value;
                }
            }
        }

        public User AssignedUser => RepositoryManager.GetRepository<User>().FindEntityById(AssignedUserId);

        /// <summary>
        /// The <see cref="Jam" /> this <see cref="Task" /> belongs to.
        /// </summary>
        public int JamId { get; private set; }

        /// <summary>
        /// <see cref="Shared.Domain.Task.Comments" /> for the
        /// <see cref="Task" /> .
        /// </summary>
        public IEnumerable<TaskComment> Comments => RepositoryManager.GetRepository<TaskComment>().GetAllEntities().Where(tc => tc.Task.Equals(this));

        /// <summary>
        /// The <see cref="Task" /> 's
        /// <see cref="Shared.Domain.Task.Category" /> .
        /// </summary>
        public TaskCategory Category { get; }

        /// <summary>
        /// Has the <see cref="Task" /> been completed?
        /// </summary>
        public bool IsCompleted
        {
            get { return isCompleted; }
            set
            {
                if (value && !IsCompleted)
                {
                    isCompleted = true;
                }
            }
        }

        /// <summary>
        /// The date the <see cref="Task" /> was completed, if completed.
        /// </summary>
        public DateTime CompletedDate
        {
            get { return completedDate; }
            set
            {
                if (CompletedDate == DateTime.MinValue && IsCompleted)
                {
                    completedDate = value;
                }
            }
        }

        public Band Band => RepositoryManager.GetRepository<Band>().FindEntityById(BandId);

        /// <summary>
        /// Give the task a <see cref="Jam" /> .
        /// </summary>
        /// <param name="newTaskJamId">
        /// The <see cref="Jam" /> Id the task is assigned with.
        /// </param>
        public void AssignTaskToJam(int newTaskJamId)
        {
            JamId = newTaskJamId;
        }

        /// <summary>
        /// Find and add a <paramref name="comment" /> to a comment.
        /// </summary>
        /// <param name="comment"></param>
        public void AddCommentToRelevantParent(TaskComment comment)
        {
            ((IEntityRepository<TaskComment>) RepositoryManager.GetRepository<TaskComment>()).AddEntity(comment);


            /*            if (comment.ParentComment != null)
            {
                foreach (TaskComment taskComment in Comments)
                {
                    TryFindParent(comment, taskComment);
                }
            }
            else
            {
                ((IEntityRepository<TaskComment>) RepositoryManager.GetRepository<TaskComment>()).AddEntity(comment);
            }*/
        }

        private static void TryFindParent(TaskComment reply, TaskComment comment)
        {
            bool added = TryAddReply(reply, comment);

            if (!added)
            {
                foreach (TaskComment possibleParent in comment.Replies)
                {
                    added = TryAddReply(reply, comment);

                    if (added)
                    {
                        return;
                    }

                    TryFindParent(reply, possibleParent);
                }
            }
        }

        private static bool TryAddReply(TaskComment reply, TaskComment comment)
        {
            if (comment.Equals(reply.ParentComment))
            {
                comment.AddReply(reply);

                return true;
            }

            return false;
        }
    }
}