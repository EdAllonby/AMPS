using System;
using System.Diagnostics.Contracts;

namespace Shared.Domain
{
    /// <summary>
    /// Models a Jam in the system as an entity.
    /// </summary>
    [Serializable]
    public sealed class Jam : Entity
    {
        private readonly int bandId;
        private readonly DateTime jamEndDate;
        private bool isActive = true;

        /// <summary>
        /// Create a new Jam entity with an Id
        /// </summary>
        /// <param name="id">Create a new Jam with an Id.</param>
        /// <param name="bandId">The <see cref="Band" /> this Jam belongs to.</param>
        /// <param name="jamEndDate">The date in which the <see cref="Jam"/> ends.</param>
        public Jam(int id, int bandId, DateTime jamEndDate) : base(id)
        {
            Contract.Requires(id > 0);
            Contract.Requires(bandId > 0);

            this.bandId = bandId;
            this.jamEndDate = jamEndDate;
        }

        /// <summary>
        /// The <see cref="Band" /> Id that this <see cref="Jam" /> belongs to.
        /// </summary>
        public int BandId
        {
            get { return bandId; }
        }

        /// <summary>
        /// The date in which the <see cref="Jam" /> ends.
        /// </summary>
        public DateTime JamEndDate
        {
            get { return jamEndDate; }
        }

        /// <summary>
        /// Is the current <see cref="Jam" /> active?
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (value.Equals(false))
                {
                    if (DateTime.UtcNow > jamEndDate)
                    {
                        isActive = value;
                    }
                }
            }
        }
    }
}