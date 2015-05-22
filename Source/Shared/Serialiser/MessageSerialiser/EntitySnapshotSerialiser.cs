using System.Net.Sockets;
using Shared.Domain;
using Shared.Message;

namespace Shared.Serialiser.MessageSerialiser
{
    internal sealed class EntitySnapshotSerialiser<T> : MessageSerialiser<EntitySnapshot<T>> where T : Entity
    {
        private readonly ISerialisationType serialiser = new BinarySerialiser();

        /// <summary>
        /// Serialise a <see cref="EntitySnapshot{T}" /> down the wire.
        /// </summary>
        /// <param name="networkStream">The networkStream that connects the Client and Server.</param>
        /// <param name="message">The <see cref="EntitySnapshot{T}" /> to serialise.</param>
        protected override void Serialise(NetworkStream networkStream, EntitySnapshot<T> message)
        {
            serialiser.Serialise(networkStream, message);
        }

        /// <summary>
        /// Deserialises a <see cref="EntitySnapshot{T}" /> from the <see cref="NetworkStream" />.
        /// </summary>
        /// <param name="networkStream">The <see cref="NetworkStream" /> containing the serialised <see cref="EntitySnapshot{T}" />.</param>
        /// <returns>The deserialised <see cref="EntitySnapshot{T}" />.</returns>
        public override IMessage Deserialise(NetworkStream networkStream)
        {
            var entitySnapshot = (EntitySnapshot<T>) serialiser.Deserialise(networkStream);
            Log.InfoFormat("Network stream has received data and deserialised to a {0} object", entitySnapshot.MessageIdentifier);
            return entitySnapshot;
        }
    }
}