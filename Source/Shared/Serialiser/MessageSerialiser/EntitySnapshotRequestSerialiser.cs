using System.Net.Sockets;
using Shared.Domain;
using Shared.Message;

namespace Shared.Serialiser.MessageSerialiser
{
    /// <summary>
    /// Used to serialise and deserialise the  <see cref="EntitySnapshotRequest{T}" /> <see cref="IMessage"/>.
    /// </summary>
    internal sealed class EntitySnapshotRequestSerialiser<T> : MessageSerialiser<EntitySnapshotRequest<T>> where T : Entity
    {
        private readonly ISerialisationType serialiser = new BinarySerialiser();

        /// <summary>
        /// Serialises a <see cref="EntitySnapshotRequest{T}" /> down the wire.
        /// </summary>
        /// <param name="networkStream">The networkStream that connects the Client and Server.</param>
        /// <param name="message">The <see cref="EntitySnapshotRequest{T}"/> to serialise.</param>
        protected override void Serialise(NetworkStream networkStream, EntitySnapshotRequest<T> message)
        {
            serialiser.Serialise(networkStream, message);
        }

        /// <summary>
        /// Deserialises a <see cref="EntitySnapshotRequest{T}" /> from the <see cref="NetworkStream" />.
        /// </summary>
        /// <param name="networkStream">The <see cref="NetworkStream" /> containing the serialised <see cref="EntitySnapshotRequest{T}" />.</param>
        /// <returns>The deserialised <see cref="EntitySnapshotRequest{T}" />.</returns>
        public override IMessage Deserialise(NetworkStream networkStream)
        {
            var entitySnapshotRequest = (EntitySnapshotRequest<T>) serialiser.Deserialise(networkStream);
            Log.InfoFormat("Network stream has received data and deserialised to a {0} object", entitySnapshotRequest.MessageIdentifier);
            return entitySnapshotRequest;
        }
    }
}