using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using log4net;

namespace Shared.Domain
{
    /// <summary>
    /// Defines something as a domain entity in our application.
    /// </summary>
    [Serializable]
    public abstract class Entity : IEquatable<Entity>
    {
        /// <summary>
        /// The Log for an <see cref="Entity" />.
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(Entity));

        private DateTime createdDate;

        [NonSerialized] private IRepositoryManager repositoryManager;

        /// <summary>
        /// Gives an entity an id
        /// </summary>
        /// <param name="id"></param>
        protected Entity(int id)
        {
            Id = id;
        }

        /// <summary>
        /// If Entity hasn't been assigned an id, set <see cref="IsNew" /> flag.
        /// Implemented Fowler PoEAA p. 221
        /// </summary>
        protected Entity()
        {
            IsNew = true;
        }

        /// <summary>
        /// An entity can be new if it has not yet been assigned an ID.
        /// </summary>
        public bool IsNew { get; }

        /// <summary>
        /// The unique Id of this entity.
        /// </summary>
        public int Id { get; }

        public IRepositoryManager RepositoryManager
        {
            get => repositoryManager;
            set => repositoryManager = value;
        }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }


        /// <summary>
        /// Check for <see cref="Entity" /> equality.
        /// </summary>
        /// <param name="other">The other <see cref="Entity" /> to check equality with.</param>
        /// <returns>If the two <see cref="Entity" /> objects are equal.</returns>
        public bool Equals(Entity other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id == other.Id;
        }

        /// <summary>
        /// Clones an <see cref="Entity" />.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Entity" /> to clone.</typeparam>
        /// <param name="entity">The <see cref="Entity" /> to clone.</param>
        /// <returns>A cloned <see cref="Entity" />.</returns>
        public static T DeepClone<T>([NotNull] T entity) where T : Entity
        {
            using (var memoryStream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, entity);
                memoryStream.Position = 0;

                return (T) formatter.Deserialize(memoryStream);
            }
        }

        /// <summary>
        /// Check for <see cref="Entity" /> equality.
        /// </summary>
        /// <param name="obj">The other object to check equality with.</param>
        /// <returns>If the two objects are equal.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj is Entity && Equals((Entity) obj);
        }

        /// <summary>
        /// Returns the hash code for the <see cref="Entity" />.
        /// </summary>
        /// <returns>The hash code of the <see cref="Entity" />.</returns>
        public override int GetHashCode()
        {
            return Id;
        }
    }
}