using Shared;
using Shared.Domain;

namespace Server
{
    /// <summary>
    /// Handles Password queries / attempts / creation.
    /// </summary>
    public sealed class PasswordHandler : IService
    {
        private readonly IPasswordHash passwordHash = new Pbkdf2PasswordHash();
        private readonly IPasswordPersister passwordPersister;

        /// <summary>
        /// Creates a new Password Handler with a desired persistence strategy.
        /// </summary>
        /// <param name="passwordPersister">The strategy used to persist the passwords.</param>
        public PasswordHandler(IPasswordPersister passwordPersister)
        {
            this.passwordPersister = passwordPersister;
        }

        /// <summary>
        /// Stores a new password for the user.
        /// </summary>
        /// <param name="userId">The user to store the password for.</param>
        /// <param name="password">The password to store.</param>
        /// <returns></returns>
        public bool StorePassword(int userId, string password)
        {
            string generatedPasswordHash = passwordHash.CreateHash(password);

            return passwordPersister.StorePasswordHash(userId, generatedPasswordHash);
        }

        /// <summary>
        /// Checks if the password for a user is valid.
        /// </summary>
        /// <param name="userId">The <see cref="User" /> to check the password for.</param>
        /// <param name="passwordAttempt">The potential password for the <see cref="User" />.</param>
        /// <returns>Whether the password attempt matches the stored <see cref="User" /> password.</returns>
        public bool IsPasswordValid(int userId, string passwordAttempt)
        {
            string storedPasswordHash = passwordPersister.GetPasswordHash(userId);

            if (storedPasswordHash == null)
            {
                return StorePassword(userId, passwordAttempt);
            }

            return passwordHash.ValidatePassword(passwordAttempt, storedPasswordHash);
        }
    }
}