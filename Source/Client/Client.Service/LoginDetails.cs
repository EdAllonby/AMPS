using System.Net;

namespace Client.Service
{
    /// <summary>
    /// Holds login information
    /// </summary>
    public class LoginDetails
    {
        private readonly IPAddress address;
        private readonly string password;
        private readonly int port;
        private readonly string username;

        /// <summary>
        /// Encapsulates Login information.
        /// </summary>
        /// <param name="username">The username to login with.</param>
        /// <param name="password">The password to validate the login.</param>
        /// <param name="address">The endpoint of the server.</param>
        /// <param name="port">The port which the server is listening to.</param>
        public LoginDetails(string username, string password, IPAddress address, int port)
        {
            this.username = username;
            this.password = password;
            this.address = address;
            this.port = port;
        }

        /// <summary>
        /// The username used to login.
        /// </summary>
        public string Username
        {
            get { return username; }
        }

        /// <summary>
        /// The password used to validate the login.
        /// </summary>
        public string Password
        {
            get { return password; }
        }

        /// <summary>
        /// The IP targeted Address used to login.
        /// </summary>
        public IPAddress Address
        {
            get { return address; }
        }

        /// <summary>
        /// The targeted Port used to login.
        /// </summary>
        public int Port
        {
            get { return port; }
        }
    }
}