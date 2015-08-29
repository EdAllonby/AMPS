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
    public class MessageHandlerRegistry
    {
        /// <summary>
        /// A dictionary of <see cref="IMessageHandler" /> implementations indexed by their relevant
        /// <see cref="MessageIdentifier" /> to be used by the Client.
        /// </summary>
        public readonly IReadOnlyDictionary<MessageIdentifier, IMessageHandler> MessageHandlersIndexedByMessageIdentifier;

        public MessageHandlerRegistry(IServiceRegistry serviceRegistry)
        {
            MessageHandlersIndexedByMessageIdentifier = new Dictionary<MessageIdentifier, IMessageHandler>
            {
                // Snapshot Handlers
                {MessageIdentifier.UserSnapshot, new EntityBootstrapper<User>(serviceRegistry)},
                {MessageIdentifier.JamSnapshot, new EntityBootstrapper<Jam>(serviceRegistry)},
                {MessageIdentifier.ParticipationSnapshot, new EntityBootstrapper<Participation>(serviceRegistry)},
                {MessageIdentifier.BandSnapshot, new EntityBootstrapper<Band>(serviceRegistry)},
                {MessageIdentifier.TaskSnapshot, new EntityBootstrapper<Task>(serviceRegistry)},

                // Entity Notification Handlers
                {MessageIdentifier.UserNotification, new UserNotificationHandler(serviceRegistry)},
                {MessageIdentifier.ConnectionStatusNotification, new ConnectionStatusNotificationHandler(serviceRegistry)},
                {MessageIdentifier.JamNotification, new JamNotificationHandler(serviceRegistry)},
                {MessageIdentifier.ParticipationNotification, new ParticipationNotificationHandler(serviceRegistry)},
                {MessageIdentifier.BandNotification, new BandNotificationHandler(serviceRegistry)},
                {MessageIdentifier.TaskNotification, new TaskNotificationHandler(serviceRegistry)}
            };
        }

        public IReadOnlyCollection<IBootstrapper> Bootstrappers => new List<IBootstrapper>
        {
            (IBootstrapper) MessageHandlersIndexedByMessageIdentifier[MessageIdentifier.BandSnapshot],
            (IBootstrapper) MessageHandlersIndexedByMessageIdentifier[MessageIdentifier.JamSnapshot],
            (IBootstrapper) MessageHandlersIndexedByMessageIdentifier[MessageIdentifier.ParticipationSnapshot],
            (IBootstrapper) MessageHandlersIndexedByMessageIdentifier[MessageIdentifier.TaskSnapshot],
            (IBootstrapper) MessageHandlersIndexedByMessageIdentifier[MessageIdentifier.UserSnapshot]
        };
    }
}