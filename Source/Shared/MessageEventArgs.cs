using System;
using JetBrains.Annotations;
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
        public MessageEventArgs([NotNull] IMessage message)
        {
            Message = message;
        }

        /// <summary>
        /// The message being carried.
        /// </summary>
        public IMessage Message { get; }
    }
}