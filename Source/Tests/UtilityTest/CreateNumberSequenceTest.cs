using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Utility;

namespace UtilityTest
{
    [TestFixture]
    public class CreateNumberSequenceTest
    {
        [Test]
        public void ShouldCreateNumbers1To10WithGapOf1()
        {
            IEnumerable<int> numbers = NumbersGenerator.CreateNumberSequence(1, 10, 1);

            IEnumerable<int> expectedResult = Enumerable.Range(1, 10);

            CollectionAssert.AreEqual(expectedResult, numbers);
        }

        [Test]
        public void ShouldCreateNumbers0To10WithGapOf1()
        {
            IEnumerable<int> numbers = NumbersGenerator.CreateNumberSequence(0, 10, 1);

            IEnumerable<int> expectedResult = Enumerable.Range(0, 10);

            CollectionAssert.AreEqual(expectedResult, numbers);
        }

        [Test]
        public void ShouldCreateNumbers0To10WithGapOf2()
        {
            IEnumerable<int> numbers = NumbersGenerator.CreateNumberSequence(0, 10, 2);
            List<int> expectedResult = new List<int> {0, 2, 4, 6, 8, 10, 12, 14, 16, 18};

            CollectionAssert.AreEqual(expectedResult, numbers);
        }

        [Test]
        public void ShouldCreateNumbers0To5WithGapOf5()
        {
            IEnumerable<int> numbers = NumbersGenerator.CreateNumberSequence(0, 2, 5);
            List<int> expectedResult = new List<int> {0, 5};

            CollectionAssert.AreEqual(expectedResult, numbers);
        }

        [Test]
        public void ShouldCreateNumbers1To11WithGapOf5()
        {
            IEnumerable<int> numbers = NumbersGenerator.CreateNumberSequence(1, 3, 5);
            List<int> expectedResult = new List<int> {1, 6, 11};

            CollectionAssert.AreEqual(expectedResult, numbers);
        }
    }
}