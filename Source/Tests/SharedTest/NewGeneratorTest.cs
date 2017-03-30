using NUnit.Framework;
using Shared;

namespace SharedTest
{
    [TestFixture]
    public sealed class NewGeneratorTest
    {
        [Test]
        public void GetHighestIdReturnsZero()
        {
            var newGenerator = new NewGenerator();

            Assert.IsTrue(newGenerator.GetCurrentHighestId().Equals(0));
        }
    }
}