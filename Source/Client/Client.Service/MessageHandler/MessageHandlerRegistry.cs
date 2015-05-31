using System.Collections.Generic;
using Shared;
using Shared.Domain;
using Shared.Message;

namespace Client.Service.MessageHandler
{
    /// <summary>
    /// Holds the link between an <see cref="IMessage" /> and their implementation of an <see cref="IMessageHandler" /> to be
    /// used by the Client.
    /// </summary>
    internal static class MessageHandlerRegistry
    {
        /// <summary>
        /// A dictionary of <see cref="IMessageHandler" /> implementations indexed by their relevant
        /// <see cref="MessageIdentifier" /> to be used by the Client.
        /// </summary>
        public static readonly IReadOnlyDictionary<MessageIdentifier, IMessageHandler>
            MessageHandlersIndexedByMessageIdentifier = new Dictionary<MessageIdentifier, IMessageHandler>
            {
                // Snapshot Handlers
                {MessageIdentifier.UserSnapshot, new EntitySnapshotHandler<User>()},
                {MessageIdentifier.JamSnapshot, new EntitySnapshotHandler<Jam>()},
                {MessageIdentifier.ParticipationSnapshot, new EntitySnapshotHandler<Participation>()},
                {MessageIdentifier.BandSnapshot, new EntitySnapshotHandler<Band>()},
                {MessageIdentifier.TaskSnapshot, new EntitySnapshotHandler<Task>()},

                // Entity Notification Handlers
                {MessageIdentifier.UserNotification, new UserNotificationHandler()},
                {MessageIdentifier.ConnectionStatusNotification, new ConnectionStatusNotificationHandler()},
                {MessageIdentifier.JamNotification, new JamNotificationHandler()},
                {MessageIdentifier.ParticipationNotification, new ParticipationNotificationHandler()},
                {MessageIdentifier.BandNotification, new BandNotificationHandler()},
                {MessageIdentifier.TaskNotification, new TaskNotificationHandler()}
            };
    }
}