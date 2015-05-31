using System;
using NUnit.Framework;
using Shared;
using Shared.Domain;

namespace SharedTest
{
    [TestFixture]
    public sealed class JamEndedEventArgsTest
    {
        [Test]
        public void HoldsJam()
        {
            Jam jam = new Jam(1, 1, new DateTime());

            JamEndedEventArgs jamEndedEventArgs = new JamEndedEventArgs(jam);

            Assert.AreEqual(jam, jamEndedEventArgs.FinishedJam);
        }

        [Test]
        public void IsAnEventArgs()
        {
            JamEndedEventArgs jamEndedEventArgs = new JamEndedEventArgs(new Jam(1, 1, new DateTime()));

            Assert.IsInstanceOf<EventArgs>(jamEndedEventArgs);
        }
    }
}