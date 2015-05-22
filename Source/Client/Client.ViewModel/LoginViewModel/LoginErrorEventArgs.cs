using System;
using Shared.Message.LoginMessage;

namespace Client.ViewModel.LoginViewModel
{
    /// <summary>
    /// Event Args for a log in error.
    /// </summary>
    public class LoginErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Encapsulate the errors in a new login error event args.
        /// </summary>
        /// <param name="result">The <see cref="LoginResult" /> supplied by the server.</param>
        /// <param name="errorDescription">The description of the login error.</param>
        public LoginErrorEventArgs(LoginResult result, string errorDescription)
        {
            Result = result;
            ErrorDescription = errorDescription;
        }

        /// <summary>
        /// The description of the login error.
        /// </summary>
        public string ErrorDescription { get; private set; }

        /// <summary>
        /// The <see cref="LoginResult" /> supplied by the server.
        /// </summary>
        public LoginResult Result { get; private set; }
    }
}