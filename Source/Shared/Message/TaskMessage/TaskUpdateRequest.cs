using System;
using JetBrains.Annotations;
using Shared.Domain;

namespace Shared.Message.TaskMessage
{
    /// <summary>
    /// Packages a <see cref="Task" /> to update.
    /// </summary>
    [Serializable]
    public sealed class TaskUpdateRequest : IMessage
    {
        /// <summary>
        /// The <see cref="Task" /> to update request.
        /// </summary>
        /// <param name="updatedTask">The <see cref="Task" /> to update.</param>
        public TaskUpdateRequest([NotNull] Task updatedTask)
        {
            UpdatedTask = updatedTask;
        }

        /// <summary>
        /// The <see cref="Task" /> to update.
        /// </summary>
        public Task UpdatedTask { get; }

        /// <summary>
        /// The identifier associated with the particular <see cref="IMessage" />.
        /// </summary>
        public MessageIdentifier MessageIdentifier => MessageIdentifier.TaskUpdateRequest;
    }
}