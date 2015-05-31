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

namespace Shared.Serialiser
{
    /// <summary>
    /// Defines various relationships of message types, identifiers and serialisers
    /// </summary>
    public static class SerialiserRegistry
    {
        /// <summary>
        /// A read only version of an <see cref="IMessageSerialiser" /> by Message Identifier dictionary. No one can alter this
        /// dictionary after compiling.
        /// </summary>
        public static readonly IReadOnlyDictionary<MessageIdentifier, IMessageSerialiser> SerialisersByMessageIdentifier =
            new Dictionary<MessageIdentifier, IMessageSerialiser>
            {
                // Entity Snapshot Request
                {MessageIdentifier.UserSnapshotRequest, new MessageSerialiser<EntitySnapshotRequest<User>>()},
                {MessageIdentifier.JamSnapshotRequest, new MessageSerialiser<EntitySnapshotRequest<Jam>>()},
                {MessageIdentifier.ParticipationSnapshotRequest, new MessageSerialiser<EntitySnapshotRequest<Participation>>()},
                {MessageIdentifier.BandSnapshotRequest, new MessageSerialiser<EntitySnapshotRequest<Band>>()},
                {MessageIdentifier.TaskSnapshotRequest, new MessageSerialiser<EntitySnapshotRequest<Task>>()},

                // Entity Snapshot
                {MessageIdentifier.ParticipationSnapshot, new MessageSerialiser<EntitySnapshot<Participation>>()},
                {MessageIdentifier.UserSnapshot, new MessageSerialiser<EntitySnapshot<User>>()},
                {MessageIdentifier.BandSnapshot, new MessageSerialiser<EntitySnapshot<Band>>()},
                {MessageIdentifier.TaskSnapshot, new MessageSerialiser<EntitySnapshot<Task>>()},
                {MessageIdentifier.JamSnapshot, new MessageSerialiser<EntitySnapshot<Jam>>()},

                // Request
                {MessageIdentifier.LoginRequest, new MessageSerialiser<LoginRequest>()},
                {MessageIdentifier.JamRequest, new MessageSerialiser<JamRequest>()},
                {MessageIdentifier.BandRequest, new MessageSerialiser<BandRequest>()},
                {MessageIdentifier.TaskRequest, new MessageSerialiser<TaskRequest>()},
                {MessageIdentifier.ParticipationRequest, new MessageSerialiser<ParticipationRequest>()},
                {MessageIdentifier.TaskUpdateRequest, new MessageSerialiser<TaskUpdateRequest>()},
                {MessageIdentifier.TaskCommentRequest, new MessageSerialiser<TaskCommentRequest>()},

                // Notification 
                {MessageIdentifier.UserNotification, new EntityNotificationSerialiser<User>()},
                {MessageIdentifier.JamNotification, new EntityNotificationSerialiser<Jam>()},
                {MessageIdentifier.ParticipationNotification, new EntityNotificationSerialiser<Participation>()},
                {MessageIdentifier.BandNotification, new EntityNotificationSerialiser<Band>()},
                {MessageIdentifier.TaskNotification, new EntityNotificationSerialiser<Task>()},
                {MessageIdentifier.ConnectionStatusNotification, new ConnectionStatusNotificationSerialiser()},

                // Response
                {MessageIdentifier.LoginResponse, new MessageSerialiser<LoginResponse>()},

                //Disconnection
                {MessageIdentifier.ClientDisconnection, new MessageSerialiser<ClientDisconnection>()}
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
                {typeof (EntitySnapshotRequest<User>), new MessageSerialiser<EntitySnapshotRequest<User>>()},
                {typeof (EntitySnapshotRequest<Jam>), new MessageSerialiser<EntitySnapshotRequest<Jam>>()},
                {typeof (EntitySnapshotRequest<Participation>), new MessageSerialiser<EntitySnapshotRequest<Participation>>()},
                {typeof (EntitySnapshotRequest<Band>), new MessageSerialiser<EntitySnapshotRequest<Band>>()},
                {typeof (EntitySnapshotRequest<Task>), new MessageSerialiser<EntitySnapshotRequest<Task>>()},

                // Entity Snapshot
                {typeof (EntitySnapshot<User>), new MessageSerialiser<EntitySnapshotRequest<User>>()},
                {typeof (EntitySnapshot<Jam>), new MessageSerialiser<EntitySnapshotRequest<Jam>>()},
                {typeof (EntitySnapshot<Participation>), new MessageSerialiser<EntitySnapshotRequest<Participation>>()},
                {typeof (EntitySnapshot<Band>), new MessageSerialiser<EntitySnapshotRequest<Band>>()},
                {typeof (EntitySnapshot<Task>), new MessageSerialiser<EntitySnapshotRequest<Task>>()},

                // Request
                {typeof (LoginRequest), new MessageSerialiser<LoginRequest>()},
                {typeof (BandRequest), new MessageSerialiser<BandRequest>()},
                {typeof (ParticipationRequest), new MessageSerialiser<ParticipationRequest>()},
                {typeof (TaskRequest), new MessageSerialiser<TaskRequest>()},
                {typeof (JamRequest), new MessageSerialiser<JamRequest>()},
                {typeof (TaskUpdateRequest), new MessageSerialiser<TaskUpdateRequest>()},
                {typeof (TaskCommentRequest), new MessageSerialiser<TaskCommentRequest>()},

                // Notification
                {typeof (EntityNotification<User>), new EntityNotificationSerialiser<User>()},
                {typeof (EntityNotification<Jam>), new EntityNotificationSerialiser<Jam>()},
                {typeof (EntityNotification<Participation>), new EntityNotificationSerialiser<Participation>()},
                {typeof (EntityNotification<Band>), new EntityNotificationSerialiser<Band>()},
                {typeof (EntityNotification<Task>), new EntityNotificationSerialiser<Task>()},
                {typeof (ConnectionStatusNotification), new ConnectionStatusNotificationSerialiser()},

                // Response
                {typeof (LoginResponse), new MessageSerialiser<LoginResponse>()},

                // Disconnection
                {typeof (ClientDisconnection), new MessageSerialiser<ClientDisconnection>()}
            };
    }
}