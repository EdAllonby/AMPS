using System;
using System.Diagnostics.Contracts;
using Shared.Domain;

namespace Shared.Message.LoginMessage
{
    /// <summary>
    /// Packages a <see cref="User" /> for the Client to send to the Server
    /// </summary>
    [Serializable]
    public sealed class LoginRequest : IMessage
    {
        /// <summary>
        /// Requests a <see cref="User" /> to be logged in to the Server.
        /// </summary>
        /// <param name="username">The <see cref="User" />s username to login.</param>
        /// <param name="password">The <see cref="User" />s password used to validate login.</param>
        public LoginRequest(string username, string password)
        {
            Contract.Requires(username != null);

            Username = username;
            Password = password;
        }

        /// <summary>
        /// The <see cref="User" />'s username to login.
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// The <see cref="User" />'s password used to validate login.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// The message identifier for this <see cref="IMessage" /> request.
        /// </summary>
        public MessageIdentifier MessageIdentifier => MessageIdentifier.LoginRequest;
    }
}