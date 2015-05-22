using Shared.Serialiser;

namespace Shared.Message
{
    /// <summary>
    /// Used to group the message objects into one type.
    /// This is used with <see cref="IMessageSerialiser" /> to create a <see cref="SerialiserFactory" />
    /// An IMessage is guaranteed to implement its own identifier.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// The identifier associated with the particular <see cref="IMessage"/>.
        /// </summary>
        MessageIdentifier MessageIdentifier { get; }
    }
}