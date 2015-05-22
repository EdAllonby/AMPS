using System;
using System.Net.Sockets;
using Shared.Domain;
using Shared.Message;
using Shared.Message.TaskMessage;
using Shared.Serialiser.EntitySerialiser;

namespace Shared.Serialiser.MessageSerialiser.TaskMessageSerialiser
{
    /// <summary>
    /// Used to serialise and deserialise the <see cref="TaskUpdateRequest" /> <see cref="IMessage"/>.
    /// </summary>
    [Serializable]
    internal sealed class TaskUpdateRequestSerialiser : MessageSerialiser<TaskUpdateRequest>
    {
        private readonly EntitySerialiser<Task> taskSerialiser = new EntitySerialiser<Task>();

        /// <summary>
        /// Serialise a <see cref="TaskUpdateRequest" /> down the wire.
        /// </summary>
        /// <param name="networkStream">The networkStream that connects the Client and Server.</param>
        /// <param name="message">The <see cref="TaskUpdateRequest" /> to serialise.</param>
        protected override void Serialise(NetworkStream networkStream, TaskUpdateRequest message)
        {
            taskSerialiser.Serialise(networkStream, message.UpdatedTask);
        }

        /// <summary>
        /// Deserialises a <see cref="TaskUpdateRequest" /> from the <see cref="NetworkStream" />.
        /// </summary>
        /// <param name="networkStream">The <see cref="NetworkStream" /> containing the serialised <see cref="TaskUpdateRequest" />.</param>
        /// <returns>The deserialised <see cref="TaskUpdateRequest" />.</returns>
        public override IMessage Deserialise(NetworkStream networkStream)
        {
            Log.Debug("Waiting for a task update request message to deserialise");
            Task updatedTask = taskSerialiser.Deserialise(networkStream);
            var taskUpdateRequest = new TaskUpdateRequest(updatedTask);

            Log.InfoFormat("{0} message deserialised", taskUpdateRequest.MessageIdentifier);

            return taskUpdateRequest;
        }
    }
}