﻿using System;
using System.Net.Sockets;
using JetBrains.Annotations;
using log4net;
using Shared.Message;

namespace Shared.Serialiser.MessageSerialiser
{
    /// <summary>
    /// Defines what message gets what identifier, and used to serialise and deserialise Message Identifiers to their related
    /// Type.
    /// </summary>
    public static class MessageIdentifierSerialiser
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MessageIdentifierSerialiser));

        /// <summary>
        /// Serialises a <see cref="MessageIdentifier" /> through the <see cref="NetworkStream" />.
        /// </summary>
        /// <param name="networkStream">The <see cref="NetworkStream" /> to serialise the <see cref="MessageIdentifier" /> across.</param>
        /// <param name="messageIdentifier">The <see cref="MessageIdentifier" /> to serialise.</param>
        public static void Serialise([NotNull] NetworkStream networkStream, MessageIdentifier messageIdentifier)
        {
            networkStream.Write(BitConverter.GetBytes((int) messageIdentifier), 0, 4);
            Log.DebugFormat("Sent Message Identifier: {0} to networkStream.", messageIdentifier);
        }

        /// <summary>
        /// Deserialises a <see cref="MessageIdentifier" /> from the <see cref="NetworkStream" />.
        /// </summary>
        /// <param name="networkStream">
        /// The <see cref="NetworkStream" /> containing the serialised <see cref="MessageIdentifier" />
        /// </param>
        /// <returns>The deserialised <see cref="MessageIdentifier" />.</returns>
        public static MessageIdentifier DeserialiseMessageIdentifier([NotNull] NetworkStream networkStream)
        {
            var messageTypeBuffer = new byte[4];

            networkStream.Read(messageTypeBuffer, 0, 4);

            int messageIdentifierNumber = BitConverter.ToInt32(messageTypeBuffer, 0);

            var messageIdentifier = (MessageIdentifier) messageIdentifierNumber;

            Log.DebugFormat("Message Identifier {0} received from networkStream.", messageIdentifier);

            return messageIdentifier;
        }
    }
}