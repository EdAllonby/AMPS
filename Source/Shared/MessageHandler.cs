using Shared.Message;

namespace Shared
{
    /// <summary>
    /// Defines an <see langword="IMessageHandler" /> that is to be used for handling incoming <see cref="IMessage" />s.
    /// </summary>
    public abstract class MessageHandler<TMessage> : IMessageHandler where TMessage : IMessage
    {
        /// <summary>
        /// Handles the incoming <see cref="IMessage" />.
        /// </summary>
        /// <param name="message">The message that has been received and needs to be handled.</param>
        /// <param name="serviceRegistry">The services needed to handle the message correctly.</param>
        public void HandleMessage(IMessage message, IServiceRegistry serviceRegistry)
        {
            HandleMessage((TMessage) message, serviceRegistry);
        }

        /// <summary>
        /// Handle the incoming message.
        /// </summary>
        /// <param name="message">The message to handle.</param>
        /// <param name="serviceRegistry">The service registry given.</param>
        public abstract void HandleMessage(TMessage message, IServiceRegistry serviceRegistry);
    }
}