using System;

namespace Shared.Domain
{
    /// <summary>
    /// Gives a user a current connection status.
    /// </summary>
    [Serializable]
    public sealed class ConnectionStatus
    {
        /// <summary>
        /// The types of status.
        /// </summary>
        public enum Status
        {
            /// <summary>
            /// The current status of the user is not known, indicating an error.
            /// </summary>
            Unknown = 0,

            /// <summary>
            /// The user is currently connected and is able to send to and receive messages from the server.
            /// </summary>
            Connected,

            /// <summary>
            /// The user is disconnected from the server. They will be unable to send and receive messages.
            /// Other clients will see the Client as disconnected and cannot send messages directly to them.
            /// </summary>
            Disconnected
        }

        /// <summary>
        /// Creates a new status for an associated <see cref="User" />.
        /// </summary>
        /// <param name="userId">The associated <see cref="User" />'s Id.</param>
        /// <param name="userConnectionStatus">The <see cref="Status" /> of the <see cref="User" />.</param>
        public ConnectionStatus(int userId, Status userConnectionStatus)
        {
            this.UserId = userId;
            this.UserConnectionStatus = userConnectionStatus;
        }

        /// <summary>
        /// The associated <see cref="User" />.
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// The current status of the user.
        /// </summary>
        public Status UserConnectionStatus { get; }
    }
}