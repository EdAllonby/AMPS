using System.Net.Sockets;
using Shared.Domain;
using Shared.Message;
using Shared.Message.TaskMessage;
using Shared.Serialiser.EntitySerialiser;

namespace Shared.Serialiser.MessageSerialiser.TaskMessageSerialiser
{
    /// <summary>
    /// Used to serialise and deserialise the <see cref="TaskRequest" /> <see cref="IMessage"/>.
    /// </summary>
    internal sealed class TaskRequestSerialiser : MessageSerialiser<TaskRequest>
    {
        private readonly EntitySerialiser<Task> taskSerialiser = new EntitySerialiser<Task>();

        /// <summary>
        /// Serialise a <see cref="TaskRequest" /> down the wire.
        /// </summary>
        /// <param name="networkStream">The networkStream that connects the Client and Server.</param>
        /// <param name="message">The <see cref="TaskRequest" /> to serialise.</param>
        protected override void Serialise(NetworkStream networkStream, TaskRequest message)
        {
            taskSerialiser.Serialise(networkStream, message.Task);
        }

        /// <summary>
        /// Deserialises a <see cref="TaskRequest" /> from the <see cref="NetworkStream" />.
        /// </summary>
        /// <param name="networkStream">The <see cref="NetworkStream" /> containing the serialised <see cref="TaskRequest" />.</param>
        /// <returns>The deserialised <see cref="TaskRequest" />.</returns>
        public override IMessage Deserialise(NetworkStream networkStream)
        {
            Log.Debug("Waiting for a task request message to deserialise");
            Task task = taskSerialiser.Deserialise(networkStream);
            var taskRequest = new TaskRequest(task);

            Log.Info("Task request message deserialised");

            return taskRequest;
        }
    }
}