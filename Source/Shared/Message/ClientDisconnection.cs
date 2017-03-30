using System;

namespace Shared.Message
{
    /// <summary>
    /// Used to describe a client disconnection.
    /// This message is either gen when the Server decides a Client has disconnected (due to a socket disconnect exception)
    /// by using the associated clientUserId in the <see cref="ConnectionHandler" />, or when a client willingly logs out.
    /// </summary>
    [Serializable]
    public sealed class ClientDisconnection : IMessage
    {
        /// <summary>
        /// A client disconnection message.
        /// </summary>
        /// <param name="userId">The client to disconnect.</param>
        public ClientDisconnection(int userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// The User to disconnect.
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// <see cref="ClientDisconnection" /> message type.
        /// </summary>
        public MessageIdentifier MessageIdentifier => MessageIdentifier.ClientDisconnection;
    }
}