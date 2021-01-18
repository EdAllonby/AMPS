using System.Net.Sockets;
using Shared.Domain;
using Shared.Message;
using Shared.Serialiser.EntitySerialiser;

namespace Shared.Serialiser.MessageSerialiser
{
    internal class EntityNotificationSerialiser<T> : MessageSerialiser<EntityNotification<T>> where T : Entity
    {
        private readonly EntitySerialiser<T> entitySerialiser = new();
        private readonly NotificationTypeSerialiser notificationTypeSerialiser = new();

        /// <summary>
        /// Deserialises a <see cref="EntityNotification{T}" /> from the <see cref="NetworkStream" />.
        /// </summary>
        /// <param name="networkStream">
        /// The <see cref="NetworkStream" /> containing the serialised
        /// <see cref="EntityNotification{T}" />.
        /// </param>
        /// <param name="repositoryManager"></param>
        /// <returns>The deserialised <see cref="EntityNotification{T}" />.</returns>
        public override IMessage Deserialise(NetworkStream networkStream)
        {
            NotificationType notificationType = notificationTypeSerialiser.Deserialise(networkStream);

            var entityNotification = new EntityNotification<T>(entitySerialiser.Deserialise(networkStream), notificationType);

            Log.InfoFormat("{0} message deserialised", entityNotification.MessageIdentifier);

            return entityNotification;
        }

        /// <summary>
        /// Serialise a <see cref="EntityNotification{T}" /> down the wire.
        /// </summary>
        /// <param name="networkStream">The networkStream that connects the Client and Server.</param>
        /// <param name="message">The <see cref="EntityNotification{T}" /> to serialise.</param>
        protected override void Serialise(NetworkStream networkStream, EntityNotification<T> message)
        {
            notificationTypeSerialiser.Serialise(networkStream, message.NotificationType);
            entitySerialiser.Serialise(networkStream, message.Entity);
        }
    }
}