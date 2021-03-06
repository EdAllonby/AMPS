﻿using System;

namespace Shared.Domain
{
    /// <summary>
    /// Models a Jam in the system as an entity.
    /// </summary>
    [Serializable]
    public sealed class Jam : Entity
    {
        private readonly int bandId;
        private bool isActive = true;

        /// <summary>
        /// Create a new Jam entity with an Id
        /// </summary>
        /// <param name="id">Create a new Jam with an Id.</param>
        /// <param name="bandId">The <see cref="Band" /> this Jam belongs to.</param>
        /// <param name="jamEndDate">The date in which the <see cref="Jam" /> ends.</param>
        public Jam(int id, int bandId, DateTime jamEndDate) : base(id)
        {
            this.bandId = bandId;
            JamEndDate = jamEndDate;
        }

        /// <summary>
        /// The <see cref="Band" /> that this <see cref="Jam" /> belongs to.
        /// </summary>
        public Band Band => RepositoryManager.GetRepository<Band>().FindEntityById(bandId);

        /// <summary>
        /// The date in which the <see cref="Jam" /> ends.
        /// </summary>
        public DateTime JamEndDate { get; }

        /// <summary>
        /// Is the current <see cref="Jam" /> active?
        /// </summary>
        public bool IsActive
        {
            get => isActive;
            set
            {
                if (value.Equals(false))
                {
                    if (DateTime.UtcNow > JamEndDate)
                    {
                        isActive = value;
                    }
                }
            }
        }
    }
}