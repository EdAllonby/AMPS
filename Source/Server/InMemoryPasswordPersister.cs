using System.Collections.Generic;

namespace Server
{
    /// <summary>
    /// Persist Passwords in memory.
    /// </summary>
    public sealed class InMemoryPasswordPersister : IPasswordPersister
    {
        private readonly Dictionary<int, string> userHashesIndexedById = new();

        /// <summary>
        /// Store the password.
        /// </summary>
        /// <param name="userId">The User's Id to store the password.</param>
        /// <param name="generatedPasswordHash">The password hash to store.</param>
        /// <returns>Whether the operation was successful.</returns>
        public bool StorePasswordHash(int userId, string generatedPasswordHash)
        {
            userHashesIndexedById.Add(userId, generatedPasswordHash);

            return true;
        }

        /// <summary>
        /// Gets the password hash for a user's Id to get.
        /// </summary>
        /// <param name="userId">The Id of the user to get.</param>
        /// <returns>The user's password hash (null if not found).</returns>
        public string GetPasswordHash(int userId)
        {
            return userHashesIndexedById.TryGetValue(userId, out string value) ? value : null;
        }
    }
}