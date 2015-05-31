using System.Linq;
using NUnit.Framework;
using Server;

namespace ServerTest
{
    [TestFixture]
    public sealed class Pbkdf2PasswordHashTest
    {
        [Test]
        public void CreatesAHashWithThreeParts()
        {
            Pbkdf2PasswordHash pbkdf2PasswordHash = new Pbkdf2PasswordHash();

            string hash = pbkdf2PasswordHash.CreateHash("Password");

            int count = hash.Split(':').Count();

            Assert.IsTrue(count.Equals(3));
        }

        [Test]
        public void ReturnsFalseWhenIncorrectHashIsSuppliedForPassword()
        {
            Pbkdf2PasswordHash pbkdf2PasswordHash = new Pbkdf2PasswordHash();

            const string Password = "Password1!";

            string hash = pbkdf2PasswordHash.CreateHash(Password + "1");

            Assert.IsFalse(pbkdf2PasswordHash.ValidatePassword(Password, hash));
        }

        [Test]
        public void ReturnsTrueWhenCorrectHashIsSuppliedForPassword()
        {
            Pbkdf2PasswordHash pbkdf2PasswordHash = new Pbkdf2PasswordHash();

            const string Password = "Password1!";

            string hash = pbkdf2PasswordHash.CreateHash(Password);

            Assert.IsTrue(pbkdf2PasswordHash.ValidatePassword(Password, hash));
        }
    }
}