using Shared.Message;

namespace Shared
{
    /// <summary>
    /// Defines an <see langword="object"/> that is to be used for handling incoming <see cref="IMessage" />s.
    /// </summary>
    public interface IMessageHandler
    {
        /// <summary>
        /// Handles the incoming <see cref="IMessage" />.
        /// </summary>
        /// <param name="message">The message that has been received and needs to be handled.</param>
        /// <param name="serviceRegistry">The services needed to handle the message correctly.</param>
        void HandleMessage(IMessage message, IServiceRegistry serviceRegistry);
    }
}