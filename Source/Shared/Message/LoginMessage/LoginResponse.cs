using System;
using Shared.Domain;

namespace Shared.Message.LoginMessage
{
    /// <summary>
    /// Packages a <see cref="User" /> for the Client to send to the Server
    /// </summary>
    [Serializable]
    public sealed class LoginResponse : IMessage
    {
        /// <summary>
        /// A login repsponse message.
        /// </summary>
        /// <param name="user">The <see cref="User" /> created from the login attempt. May be null.</param>
        /// <param name="loginResult">The result of the login attempt.</param>
        public LoginResponse(User user, LoginResult loginResult)
        {
            User = user;
            LoginResult = loginResult;
        }

        /// <summary>
        /// Whether or not the Client is allowed on to the Server.
        /// </summary>
        public LoginResult LoginResult { get; }

        /// <summary>
        /// The <see cref="User" /> object created by the Server.
        /// </summary>
        public User User { get; }

        /// <summary>
        /// The type of message this is.
        /// </summary>
        public MessageIdentifier MessageIdentifier => MessageIdentifier.LoginResponse;
    }
}