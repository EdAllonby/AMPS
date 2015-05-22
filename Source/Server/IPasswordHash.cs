namespace Server
{
    /// <summary>
    /// Used to hash and validate hashes for passwords.
    /// </summary>
    public interface IPasswordHash
    {
        /// <summary>
        /// Creates a hash of the password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>The hash of the password.</returns>
        string CreateHash(string password);

        /// <summary>
        /// Validates a password given a hash of the correct one.
        /// </summary>
        /// <param name="password">The password to check.</param>
        /// <param name="correctHash">A hash of the correct password.</param>
        /// <returns>True if the password is correct. False otherwise.</returns>
        bool ValidatePassword(string password, string correctHash);
    }
}