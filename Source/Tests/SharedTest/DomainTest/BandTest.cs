using NUnit.Framework;
using Shared.Domain;

namespace SharedTest.DomainTest
{
    [TestFixture]
    public class BandTest
    {
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(12)]
        [TestCase(22)]
        public void AssignCustomIdToBandTest(int bandId)
        {
            const string BandName = "Band1";

            var band = new Band(bandId, BandName);
            Assert.AreEqual(band.Id, bandId);
        }

        [Test]
        public void AssignNameToBandTest()
        {
            const int BandId = 1;
            const string BandName = "The Band's Name";

            var band = new Band(BandId, BandName);
            Assert.AreEqual(band.Name, BandName);
        }

        [Test]
        public void BandEqualityTest()
        {
            const int BandId = 1;
            const string BandName = "The Band's Name";

            var band = new Band(BandId, BandName);
            var otherBand = new Band(BandId, BandName);
            Assert.AreEqual(band, otherBand);
        }

        [Test]
        public void BandEqualsTest()
        {
            const int BandId = 4;
            const string BandName = "The Band's Name";

            var band = new Band(BandId, BandName);
            var otherBand = new Band(BandId, BandName);
            Assert.AreEqual(band, otherBand);
            Assert.IsTrue(band.Equals((object) otherBand));
        }

        [Test]
        public void BandHashCodeTest()
        {
            const int BandId = 12;
            const string BandName = "The Band's Name";

            var band = new Band(BandId, BandName);
            var otherBand = new Band(BandId, BandName);

            Assert.AreEqual(band.GetHashCode(), otherBand.GetHashCode());
        }

        [Test]
        public void BandReferenceEqualsTest()
        {
            const int BandId = 12;
            const string BandName = "The Band's Name";

            var band = new Band(BandId, BandName);
            Band otherBand = band;

            Assert.IsTrue(band.Equals(otherBand));
            Assert.IsTrue(band.Equals((object) otherBand));
            Assert.IsFalse(band.Equals(null));

            object bandObject = band;

            Assert.IsFalse(bandObject.Equals(2));
            Assert.IsFalse(bandObject.Equals(null));
        }
    }
}