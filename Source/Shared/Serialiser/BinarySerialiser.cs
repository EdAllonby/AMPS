using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Shared.Serialiser
{
    /// <summary>
    /// An implementation of <see cref="ISerialiser" /> to serialise data over a <see cref="NetworkStream" /> in binary
    /// format.
    /// </summary>
    internal sealed class BinarySerialiser : ISerialiser
    {
        private readonly BinaryFormatter binaryFormatter = new BinaryFormatter();

        /// <summary>
        /// Serialise an <see langword="object" /> across the stream.
        /// </summary>
        /// <param name="serialisationStream">The stream to serialise across.</param>
        /// <param name="serialisableObject">The object to serialise.</param>
        public void Serialise(NetworkStream serialisationStream, object serialisableObject)
        {
            binaryFormatter.Serialize(serialisationStream, serialisableObject);
        }

        /// <summary>
        /// Deserialise an <see langword="object" /> send across the stream.
        /// </summary>
        /// <param name="serialisationStream">The stream to deserialise.</param>
        /// <returns>An <see langword="object" /> that has been deserialised.</returns>
        public object Deserialise(NetworkStream serialisationStream)
        {
            return binaryFormatter.Deserialize(serialisationStream);
        }
    }
}