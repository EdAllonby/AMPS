using System.Collections.Generic;
using Shared;
using Shared.Message;

namespace Server.MessageHandler
{
    /// <summary>
    /// Holds the link between an <see cref="IMessage"/> and their implementation of an <see cref="IMessageHandler"/> to be used by the Server.
    /// </summary>
    internal static class MessageHandlerRegistry
    {
        /// <summary>
        /// A dictionary of <see cref="IMessageHandler"/> implementations indexed by their relevant <see cref="MessageIdentifier"/> to be used by the Server.
        /// </summary>
        public static readonly IReadOnlyDictionary<MessageIdentifier, IMessageHandler>
            MessageHandlersIndexedByMessageIdentifier = new Dictionary<MessageIdentifier, IMessageHandler>
            {
                {MessageIdentifier.UserSnapshotRequest, new UserSnapshotRequestHandler()},
                {MessageIdentifier.JamSnapshotRequest, new JamSnapshotRequestHandler()},
                {MessageIdentifier.ParticipationSnapshotRequest, new ParticipationSnapshotRequestHandler()},
                {MessageIdentifier.ClientDisconnection, new ClientDisconnectionHandler()},
                {MessageIdentifier.JamRequest, new JamRequestHandler()},
                {MessageIdentifier.BandSnapshotRequest, new BandSnapshotRequestHandler()},
                {MessageIdentifier.BandRequest, new BandRequestHandler()},
                {MessageIdentifier.ParticipationRequest, new ParticipationRequestHandler()},
                {MessageIdentifier.TaskRequest, new TaskRequestHandler()},
                {MessageIdentifier.TaskSnapshotRequest, new TaskSnapshotRequestHandler()},
                {MessageIdentifier.TaskUpdateRequest, new TaskUpdateRequestHandler()}
            };
    }
}