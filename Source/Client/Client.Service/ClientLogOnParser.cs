using System.Collections.Generic;
using System.Net;
using log4net;

namespace Client.Service
{
    /// <summary>
    /// This class is used to parse the IPAddress, Port and Username with validation.
    /// </summary>
    public sealed class ClientLogOnParser
    {
        private const int PortMaxBound = 65535;
        private const int PortMinBound = 0;
        private static readonly ILog Log = LogManager.GetLogger(typeof(ClientLogOnParser));
        private bool isParsed;
        private IPAddress targetedAddress;
        private string targetedPassword;
        private int targetedPort;
        private string targetedUsername;

        /// <summary>
        /// Tries to parse the command line arguments to a <see cref="LoginDetails" /> object.
        /// </summary>
        /// <param name="commandLineArguments">A collection of command line arguments containing the login details.</param>
        /// <param name="loginDetails">An object to store the parsed login details.</param>
        /// <returns>Whether the parse was successful.</returns>
        public bool TryParseCommandLineArguments(IEnumerable<string> commandLineArguments, out LoginDetails loginDetails)
        {
            var parameterName = "";

            foreach (string argument in commandLineArguments)
            {
                if (argument[0] == '/')
                {
                    parameterName = argument;
                }
                else
                {
                    if (parameterName == "/Username")
                    {
                        ParseUsername(argument);
                    }
                    if (parameterName == "/Password" && isParsed)
                    {
                        ParsePassword(argument);
                    }
                    if (parameterName == "/IPAddress" && isParsed)
                    {
                        ParseIPAddress(argument);
                    }
                    if (parameterName == "/Port" && isParsed)
                    {
                        ParsePort(argument);
                    }
                }
            }

            loginDetails = isParsed ? new LoginDetails(targetedUsername, targetedPassword, targetedAddress, targetedPort) : null;

            Log.Info(isParsed
                ? "Command line arguments successfully parsed"
                : "Command line arguments incomplete. Going to manual entry of Username, Server and Port");

            return isParsed;
        }

        /// <summary>
        /// Tries to parse the username, ipAddress and port strings to a <see cref="LoginDetails" /> object.
        /// </summary>
        /// <param name="username">The username wanted to be set.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="ipAddress">The IPAddress wanted to be set.</param>
        /// <param name="port">The port wanted to be set.</param>
        /// <param name="loginDetails">An object to store the parsed login details.</param>
        /// <returns>Whether the parse was successful.</returns>
        public bool TryParseLogonDetails(string username, string password, string ipAddress, string port, out LoginDetails loginDetails)
        {
            ParseUsername(username);

            if (isParsed)
            {
                ParsePassword(password);
            }

            if (isParsed)
            {
                ParseIPAddress(ipAddress);
            }
            if (isParsed)
            {
                ParsePort(port);
            }
            loginDetails = isParsed ? new LoginDetails(targetedUsername, targetedPassword, targetedAddress, targetedPort) : null;

            return isParsed;
        }

        private void ParseUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                isParsed = false;
            }

            targetedUsername = username;
            Log.Info("Username set as " + targetedUsername);

            isParsed = true;
        }

        private void ParsePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                isParsed = false;
            }

            targetedPassword = password;
            Log.Info("Password set.");

            isParsed = true;
        }

        private void ParseIPAddress(string ipString)
        {
            isParsed = IPAddress.TryParse(ipString, out var address);

            if (isParsed)
            {
                Log.Info("User entered target IP Address " + address);
                targetedAddress = address;
            }
            else
            {
                Log.Warn("IPAddress was not a valid entry");
            }
        }

        private void ParsePort(string portLine)
        {
            isParsed = int.TryParse(portLine, out int port);

            if (port > PortMaxBound || port < PortMinBound)
            {
                isParsed = false;
            }

            if (isParsed)
            {
                Log.Info("User entered port " + port);
                targetedPort = port;
            }
            else
            {
                Log.Warn("Port was not a valid entry");
            }
        }
    }
}