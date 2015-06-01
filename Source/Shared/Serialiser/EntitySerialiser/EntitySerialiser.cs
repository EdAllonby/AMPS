using System.Diagnostics.Contracts;
using System.Net.Sockets;
using log4net;
using Shared.Domain;

namespace Shared.Serialiser.EntitySerialiser
{
    /// <summary>
    /// Serialises an <see cref="Entity" />across the stream.
    /// </summary>
    /// <typeparam name="T">The <see cref="Entity" /> to serialise.</typeparam>
    internal sealed class EntitySerialiser<T> where T : Entity
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (EntitySerialiser<T>));
        private readonly ISerialiser serialiser = new BinarySerialiser();

        /// <summary>
        /// Serialises an <see cref="Entity" /> through the <see cref="NetworkStream" />.
        /// </summary>
        /// <param name="networkStream">The <see cref="NetworkStream" /> to serialise the <see cref="Entity" /> across.</param>
        /// <param name="entity">The <see cref="Entity" /> to serialise.</param>
        public void Serialise(NetworkStream networkStream, T entity)
        {
            Contract.Requires(entity != null);
            Contract.Requires(networkStream != null);

            serialiser.Serialise(networkStream, entity);
            Log.DebugFormat("{0} entity serialised and sent to network stream", entity);
        }

        /// <summary>
        /// Deserialises an <see cref="Entity" /> from the <see cref="NetworkStream" />.
        /// </summary>
        /// <param name="networkStream">The <see cref="NetworkStream" /> containing the serialised <see cref="Entity" />.</param>
        /// <returns>The deserialised <see cref="Entity" />.</returns>
        public T Deserialise(NetworkStream networkStream)
        {
            Contract.Requires(networkStream != null);

            var entity = (T) serialiser.Deserialise(networkStream);
            Log.DebugFormat("Network stream has received data and deserialised to a {0} entity", entity);
            return entity;
        }
    }
}