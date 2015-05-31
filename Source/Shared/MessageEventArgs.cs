using System;
using System.Diagnostics.Contracts;
using Shared.Message;

namespace Shared
{
    /// <summary>
    /// Holds an <see cref="IMessage" />.
    /// </summary>
    public sealed class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// Create a new <see cref="EventArgs" /> containing the <see cref="IMessage" />.
        /// </summary>
        /// <param name="message">the <see cref="IMessage" /> to contain in the <see cref="EventArgs" />.</param>
        public MessageEventArgs(IMessage message)
        {
            Contract.Requires(message != null);

            Message = message;
        }

        /// <summary>
        /// The message being carried.
        /// </summary>
        public IMessage Message { get; private set; }
    }
}