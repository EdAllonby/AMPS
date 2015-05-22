namespace Server
{
    /// <summary>
    /// A password persister.
    /// </summary>
    public interface IPasswordPersister
    {
        /// <summary>
        /// Store the password.
        /// </summary>
        /// <param name="userId">The User's Id to store the password.</param>
        /// <param name="generatedPasswordHash">The password hash to store.</param>
        /// <returns>Whether the operation was successful.</returns>
        bool StorePasswordHash(int userId, string generatedPasswordHash);

        /// <summary>
        /// Gets the password hash for a user's Id to get.
        /// </summary>
        /// <param name="userId">The Id of the user to get.</param>
        /// <returns>The user's password hash (null if not found).</returns>
        string GetPasswordHash(int userId);
    }
}