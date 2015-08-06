using System;
using Shared.Domain;

namespace Shared
{
    /// <summary>
    /// Packages a finished <see cref="Jam" /> in an <see cref="EventArgs" /> object.
    /// </summary>
    public sealed class JamEndedEventArgs : EventArgs
    {
        /// <summary>
        /// Create a new <see cref="EventArgs" />
        /// </summary>
        /// <param name="finishedJam">The finished <see cref="Jam" />.</param>
        public JamEndedEventArgs(Jam finishedJam)
        {
            FinishedJam = finishedJam;
        }

        /// <summary>
        /// The packaged <see cref="Jam" /> that has ended.
        /// </summary>
        public Jam FinishedJam { get; }
    }
}