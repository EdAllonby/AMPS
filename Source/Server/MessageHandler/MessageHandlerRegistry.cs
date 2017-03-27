using System.Collections.Generic;
using Shared;
using Shared.Message;

namespace Server.MessageHandler
{
    /// <summary>
    /// Holds the link between an <see cref="IMessage" /> and their implementation of an <see cref="IMessageHandler" /> to be
    /// used by the Server.
    /// </summary>
    internal class MessageHandlerRegistry
    {
        /// <summary>
        /// A dictionary of <see cref="IMessageHandler" /> implementations indexed by their relevant
        /// <see cref="MessageIdentifier" /> to be used by the Server.
        /// </summary>
        public readonly IReadOnlyDictionary<MessageIdentifier, IMessageHandler> MessageHandlersIndexedByMessageIdentifier;

        public MessageHandlerRegistry(IServiceRegistry serviceRegistry)
        {
            MessageHandlersIndexedByMessageIdentifier = new Dictionary<MessageIdentifier, IMessageHandler>
            {
                { MessageIdentifier.UserSnapshotRequest, new UserSnapshotRequestHandler(serviceRegistry) },
                { MessageIdentifier.JamSnapshotRequest, new JamSnapshotRequestHandler(serviceRegistry) },
                { MessageIdentifier.ParticipationSnapshotRequest, new ParticipationSnapshotRequestHandler(serviceRegistry) },
                { MessageIdentifier.ClientDisconnection, new ClientDisconnectionHandler(serviceRegistry) },
                { MessageIdentifier.JamRequest, new JamRequestHandler(serviceRegistry) },
                { MessageIdentifier.BandSnapshotRequest, new BandSnapshotRequestHandler(serviceRegistry) },
                { MessageIdentifier.BandRequest, new BandRequestHandler(serviceRegistry) },
                { MessageIdentifier.ParticipationRequest, new ParticipationRequestHandler(serviceRegistry) },
                { MessageIdentifier.TaskRequest, new TaskRequestHandler(serviceRegistry) },
                { MessageIdentifier.TaskSnapshotRequest, new TaskSnapshotRequestHandler(serviceRegistry) },
                { MessageIdentifier.TaskUpdateRequest, new TaskUpdateRequestHandler(serviceRegistry) },
                { MessageIdentifier.TaskCommentRequest, new TaskCommentRequestHandler(serviceRegistry) },
                { MessageIdentifier.UnrecognisedMessage, new UnrecognisedMessageHandler() }
            };
        }
    }
}