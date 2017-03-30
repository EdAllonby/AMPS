using System;
using Shared.Domain;

namespace Shared.Message.TaskMessage
{
    /// <summary>
    /// Packages a <see cref="Task" /> without an Id for the Client to send to the Server.
    /// </summary>
    [Serializable]
    public sealed class TaskRequest : IMessage
    {
        /// <summary>
        /// A new <see cref="Task" /> request.
        /// </summary>
        /// <param name="task">The new <see cref="Task" />.</param>
        public TaskRequest(Task task)
        {
            Task = task;
        }

        /// <summary>
        /// The new <see cref="Task" />.
        /// </summary>
        public Task Task { get; }

        /// <summary>
        /// The identifier associated with the particular <see cref="IMessage" />.
        /// </summary>
        public MessageIdentifier MessageIdentifier => MessageIdentifier.TaskRequest;
    }
}