using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Shared.Domain
{
    /// <summary>
    /// Model's a task entity. A task is used to contain a piece of work needed.
    /// </summary>
    [Serializable]
    public class Task : Entity
    {
        private readonly int bandId;
        private readonly TaskCategory category;
        private readonly string description;
        private readonly string title;
        private int assignedUserId;
        private bool isCompleted;
        private int points;
        private readonly List<TaskComment> comments = new List<TaskComment>(); 
        /// <summary>
        /// Creates a new <see cref="Task" /> with no assigned Id.
        /// </summary>
        /// <param name="title">The title of the <see cref="Task" />.</param>
        /// <param name="description">The description of the <see cref="Task" />.</param>
        /// <param name="points">The number of points the <see cref="Task" /> has.</param>
        /// <param name="bandId">The <see cref="Band" /> associated with the <see cref="Task" />.</param>
        /// <param name="assignedUserId">The <see cref="User" /> who needs to complete the <see cref="Task" />.</param>
        /// <param name="category">The <see cref="Task" />'s <see cref="Category" />.</param>
        public Task(string title, string description, int points, int bandId, int assignedUserId, TaskCategory category)
        {
            Contract.Requires(bandId > 0);
            Contract.Requires(!string.IsNullOrWhiteSpace(title));
            Contract.Requires(!string.IsNullOrWhiteSpace(description));

            this.title = title;
            this.description = description;
            Points = points;
            this.bandId = bandId;
            AssignedUserId = assignedUserId;
            this.category = category;
        }

        /// <summary>
        /// Creates a <see cref="Task" /> with an Id.
        /// </summary>
        /// <param name="id">The assigned <see cref="Task" /> Id.</param>
        /// <param name="incompleteTask">The previous incomplete <see cref="Task" />.</param>
        public Task(int id, Task incompleteTask) : base(id)
        {
            Contract.Requires(incompleteTask != null);
            Contract.Requires(id > 0);

            title = incompleteTask.Title;
            description = incompleteTask.Description;
            points = incompleteTask.Points;
            bandId = incompleteTask.BandId;
            assignedUserId = incompleteTask.AssignedUserId;
            category = incompleteTask.Category;
        }

        /// <summary>
        /// Returns whether this task belongs in a <see cref="Jam" />.
        /// </summary>
        public bool IsInJam
        {
            get { return JamId > 0; }
        }

        /// <summary>
        /// Returns whether the <see cref="Task" /> has an assigned <see cref="User" />.
        /// </summary>
        public bool HasAssignedUser
        {
            get { return AssignedUserId > 0; }
        }

        /// <summary>
        /// Returns whether the <see cref="Task" /> has points given to it.
        /// </summary>
        public bool HasPoints
        {
            get { return Points > 0; }
        }

        /// <summary>
        /// The overview of the <see cref="Task" />.
        /// </summary>
        public string Title
        {
            get { return title; }
        }

        /// <summary>
        /// The detailed description of the <see cref="Task" />.
        /// </summary>
        public string Description
        {
            get { return description; }
        }

        /// <summary>
        /// The number of points this <see cref="Task" /> has.
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
        public int BandId
        {
            get { return bandId; }
        }

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

        /// <summary>
        /// The <see cref="Jam" /> this <see cref="Task" /> belongs to.
        /// </summary>
        public int JamId { get; private set; }

        /// <summary>
        /// Comments for the <see cref="Task" />.
        /// </summary>
        public List<TaskComment> Comments
        {
            get { return comments; }
        }

        /// <summary>
        /// The <see cref="Task" />'s <see cref="Category" />.
        /// </summary>
        public TaskCategory Category
        {
            get { return category; }
        }

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
        /// Give the task a <see cref="Jam" />.
        /// </summary>
        /// <param name="newTaskJamId">The <see cref="Jam" /> Id the task is assigned with.</param>
        public void AssignTaskToJam(int newTaskJamId)
        {
            Contract.Requires(newTaskJamId > 0);

            JamId = newTaskJamId;
        }
    }
}