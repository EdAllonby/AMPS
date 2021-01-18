using System.ComponentModel;

namespace Client.Model.LoginModel
{
    /// <summary>
    /// Models data for login.
    /// </summary>
    public sealed class LoginModel : NotifiableModel, IDataErrorInfo
    {
        private string exposedPassword;
        private string ipAddress;
        private string port;
        private string username;

        /// <summary>
        /// Initialise a new login model.
        /// </summary>
        public LoginModel()
        {
            Password = string.Empty;
        }

        /// <summary>
        /// The username to use.
        /// </summary>
        public string Username
        {
            get { return username; }
            set
            {
                if (value == username) return;
                username = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The Password to login with, behind the asterisk facade.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// The new password, normally in the form of ******s where s is the last input letter.
        /// </summary>
        public string ExposedPassword
        {
            get { return exposedPassword; }
            set
            {
                if (value == exposedPassword) return;
                if (PasswordCharacterRemoved(value))
                {
                    int lengthDifference = Password.Length - value.Length;

                    Password = Password.Remove(Password.Length - lengthDifference);
                }
                if (PasswordCharacterAdded(value))
                {
                    string lastPasswordCharacter = value.Substring(value.Length - 1, 1);

                    Password = Password + lastPasswordCharacter;
                }

                exposedPassword = CreateAsterisks(value.Length);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The IP to use.
        /// </summary>
        public string IPAddress
        {
            get { return ipAddress; }
            set
            {
                if (value == ipAddress) return;
                ipAddress = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The port to use.
        /// </summary>
        public string Port
        {
            get { return port; }
            set
            {
                if (value == port) return;
                port = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Error checking.
        /// </summary>
        /// <param name="columnName">The column to check.</param>
        /// <returns>Any errors.</returns>
        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (string.IsNullOrEmpty(Username))
                {
                    Error = "Username is required.";
                }

                if (string.IsNullOrEmpty(ExposedPassword))
                {
                    Error = "Password is required.";
                }

                if (string.IsNullOrEmpty(IPAddress))
                {
                    Error = "IP Address is required.";
                }

                if (!int.TryParse(Port, out int portInt))
                {
                    Error = "Port must only consist of digits.";
                }
                if (portInt < 0 || portInt > 65535)
                {
                    Error = "Port must be between 0 and 65535.";
                }

                return Error;
            }
        }

        /// <summary>
        /// Error in the model.
        /// </summary>
        public string Error { get; private set; }

        private bool PasswordCharacterAdded(string value)
        {
            return value.Length > Password.Length;
        }

        private bool PasswordCharacterRemoved(string value)
        {
            return value.Length < Password.Length;
        }

        private static string CreateAsterisks(int length)
        {
            string asterisks = string.Empty;

            for (var asteriskCount = 0; asteriskCount < length; asteriskCount++)
            {
                asterisks += "*";
            }

            return asterisks;
        }
    }
}