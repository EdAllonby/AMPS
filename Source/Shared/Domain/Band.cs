using System;
using System.Diagnostics.Contracts;

namespace Shared.Domain
{
    /// <summary>
    /// Models a Band entity.
    /// </summary>
    [Serializable]
    public sealed class Band : Entity
    {
        private readonly string name;

        /// <summary>
        /// Creates a new <see cref="Band" /> which has an Id.
        /// </summary>
        /// <param name="id">The Id assigned to the <see cref="Band" />.</param>
        /// <param name="name">The name of the <see cref="Band" />.</param>
        public Band(int id, string name) : base(id)
        {
            Contract.Requires(id > 0);

            this.name = name;
        }

        /// <summary>
        /// The name of the <see cref="Band" />.
        /// </summary>
        public string Name
        {
            get { return name; }
        }
    }
}