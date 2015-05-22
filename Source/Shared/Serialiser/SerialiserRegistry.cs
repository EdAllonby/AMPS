using System;
using System.Collections.Generic;
using Shared.Domain;
using Shared.Message;
using Shared.Message.BandMessage;
using Shared.Message.JamMessage;
using Shared.Message.LoginMessage;
using Shared.Message.ParticipationMessage;
using Shared.Message.TaskMessage;
using Shared.Serialiser.MessageSerialiser;
using Shared.Serialiser.MessageSerialiser.BandMessageSerialiser;
using Shared.Serialiser.MessageSerialiser.JamMessageSerialiser;
using Shared.Serialiser.MessageSerialiser.LoginMessageSerialiser;
using Shared.Serialiser.MessageSerialiser.ParticipationMessageSerialiser;
using Shared.Serialiser.MessageSerialiser.TaskMessageSerialiser;

namespace Shared.Serialiser
{
    /// <summary>
    /// Defines various relationships of message types, identifiers and serialisers
    /// </summary>
    public static class SerialiserRegistry
    {
        /// <summary>
        /// A read only version of an <see cref="IMessageSerialiser"/> by Message Identifier dictionary. No one can alter this dictionary after compiling.
        /// </summary>
        public static readonly IReadOnlyDictionary<MessageIdentifier, IMessageSerialiser> SerialisersByMessageIdentifier =
            new Dictionary<MessageIdentifier, IMessageSerialiser>
            {
                // Entity Snapshot Request
                {MessageIdentifier.UserSnapshotRequest, new EntitySnapshotRequestSerialiser<User>()},
                {MessageIdentifier.JamSnapshotRequest, new EntitySnapshotRequestSerialiser<Jam>()},
                {MessageIdentifier.ParticipationSnapshotRequest, new EntitySnapshotRequestSerialiser<Participation>()},
                {MessageIdentifier.BandSnapshotRequest, new EntitySnapshotRequestSerialiser<Band>()},
                {MessageIdentifier.TaskSnapshotRequest, new EntitySnapshotRequestSerialiser<Task>()},

                // Entity Snapshot
                {MessageIdentifier.ParticipationSnapshot, new EntitySnapshotSerialiser<Participation>()},
                {MessageIdentifier.UserSnapshot, new EntitySnapshotSerialiser<User>()},
                {MessageIdentifier.BandSnapshot, new EntitySnapshotSerialiser<Band>()},
                {MessageIdentifier.TaskSnapshot, new EntitySnapshotSerialiser<Task>()},
                {MessageIdentifier.JamSnapshot, new EntitySnapshotSerialiser<Jam>()},

                // Request
                {MessageIdentifier.LoginRequest, new LoginRequestSerialiser()},
                {MessageIdentifier.JamRequest, new JamRequestSerialiser()},
                {MessageIdentifier.BandRequest, new BandRequestSerialiser()},
                {MessageIdentifier.TaskRequest, new TaskRequestSerialiser()},
                {MessageIdentifier.ParticipationRequest, new ParticipationRequestSerialiser()},
                {MessageIdentifier.TaskUpdateRequest, new TaskUpdateRequestSerialiser()},

                // Notification 
                {MessageIdentifier.UserNotification, new EntityNotificationSerialiser<User>()},
                {MessageIdentifier.JamNotification, new EntityNotificationSerialiser<Jam>()},
                {MessageIdentifier.ParticipationNotification, new EntityNotificationSerialiser<Participation>()},
                {MessageIdentifier.BandNotification, new EntityNotificationSerialiser<Band>()},
                {MessageIdentifier.TaskNotification, new EntityNotificationSerialiser<Task>()},
                {MessageIdentifier.ConnectionStatusNotification, new ConnectionStatusNotificationSerialiser()},

                // Response
                {MessageIdentifier.LoginResponse, new LoginResponseSerialiser()},

                //Disconnection
                {MessageIdentifier.ClientDisconnection, new ClientDisconnectionSerialiser()}
            };

        /// <summary>
        /// A read only version of an <see cref="IMessageSerialiser" /> by Message
        /// <see cref="Type" /> dictionary. No one can alter this dictionary
        /// after compiling.
        /// </summary>
        public static readonly IReadOnlyDictionary<Type, IMessageSerialiser> SerialisersByMessageType =
            new Dictionary<Type, IMessageSerialiser>
            {
                // Entity Snapshot Request
                {typeof (EntitySnapshotRequest<User>), new EntitySnapshotRequestSerialiser<User>()},
                {typeof (EntitySnapshotRequest<Jam>), new EntitySnapshotRequestSerialiser<Jam>()},
                {typeof (EntitySnapshotRequest<Participation>), new EntitySnapshotRequestSerialiser<Participation>()},
                {typeof (EntitySnapshotRequest<Band>), new EntitySnapshotRequestSerialiser<Band>()},
                {typeof (EntitySnapshotRequest<Task>), new EntitySnapshotRequestSerialiser<Task>()},

                // Entity Snapshot
                {typeof (EntitySnapshot<User>), new EntitySnapshotSerialiser<User>()},
                {typeof (EntitySnapshot<Jam>), new EntitySnapshotSerialiser<Jam>()},
                {typeof (EntitySnapshot<Participation>), new EntitySnapshotSerialiser<Participation>()},
                {typeof (EntitySnapshot<Band>), new EntitySnapshotSerialiser<Band>()},
                {typeof (EntitySnapshot<Task>), new EntitySnapshotSerialiser<Task>()},

                // Request
                {typeof (LoginRequest), new LoginRequestSerialiser()},
                {typeof (BandRequest), new BandRequestSerialiser()},
                {typeof (ParticipationRequest), new ParticipationRequestSerialiser()},
                {typeof (TaskRequest), new TaskRequestSerialiser()},
                {typeof (JamRequest), new JamRequestSerialiser()},
                {typeof (TaskUpdateRequest), new TaskUpdateRequestSerialiser()},

                // Notification
                {typeof (EntityNotification<User>), new EntityNotificationSerialiser<User>()},
                {typeof (EntityNotification<Jam>), new EntityNotificationSerialiser<Jam>()},
                {typeof (EntityNotification<Participation>), new EntityNotificationSerialiser<Participation>()},
                {typeof (EntityNotification<Band>), new EntityNotificationSerialiser<Band>()},
                {typeof (EntityNotification<Task>), new EntityNotificationSerialiser<Task>()},
                {typeof (ConnectionStatusNotification), new ConnectionStatusNotificationSerialiser()},

                // Response
                {typeof (LoginResponse), new LoginResponseSerialiser()},

                // Disconnection
                {typeof (ClientDisconnection), new ClientDisconnectionSerialiser()}
            };
    }
}