using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Client.Service.MessageHandler;
using log4net;
using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Message.LoginMessage;
using Shared.Serialiser;
using Shared.Serialiser.MessageSerialiser;
using Utility;

namespace Client.Service
{
    /// <summary>
    /// Logs a user in to the Server.
    /// </summary>
    public sealed class ServerLoginHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ServerLoginHandler));

        private readonly MessageHandlerRegistry messageHandlerRegistry;
        private readonly TcpClient serverConnection = new();

        private bool hasReceivedBandSnapshot;
        private bool hasReceivedJamSnapshot;
        private bool hasReceivedParticipationSnapshot;
        private bool hasReceivedTaskSnapshot;
        private bool hasReceivedUserSnapshot;

        /// <summary>
        /// Initialises a server login helper.
        /// </summary>
        public ServerLoginHandler(MessageHandlerRegistry messageHandlerRegistry)
        {
            this.messageHandlerRegistry = messageHandlerRegistry;

            foreach (IBootstrapper bootstrapper in this.messageHandlerRegistry.Bootstrappers)
            {
                bootstrapper.EntityBootstrapCompleted += EntityBootstrapCompleted;
            }
        }

        /// <summary>
        /// Fires when bootstrapping to all repositories has completed.
        /// </summary>
        public event EventHandler BootstrapCompleted;

        /// <summary>
        /// Handles connecting a client to the server, and creates a <see cref="ConnectionHandler" /> on success.
        /// </summary>
        /// <param name="loginDetails">The connection details of the trying-to-connect user.</param>
        /// <param name="connectionHandler">An initialised connection handler on login success.</param>
        /// <returns></returns>
        public LoginResponse ConnectToServer(LoginDetails loginDetails, out ConnectionHandler connectionHandler)
        {
            bool isServerFound = CreateConnection(loginDetails.Address, loginDetails.Port);

            if (!isServerFound)
            {
                connectionHandler = null;

                return new LoginResponse(null, LoginResult.ServerNotFound);
            }

            IMessage userRequest = new LoginRequest(loginDetails.Username, loginDetails.Password);
            SendConnectionMessage(userRequest);
            var loginResponse = (LoginResponse) GetConnectionMessage();

            if (loginResponse.LoginResult == LoginResult.Success)
            {
                BootstrapRepositories(loginResponse.User.Id);

                connectionHandler = new ConnectionHandler(loginResponse.User.Id, serverConnection);

                Log.DebugFormat("Connection process to the server has finished");
            }
            else
            {
                connectionHandler = null;
            }

            return loginResponse;
        }

        /// <summary>
        /// Remove any bootstrap subscriptions to this object.
        /// </summary>
        public void RemoveBootstrapEventSubscriptions()
        {
            foreach (IBootstrapper bootstrapper in messageHandlerRegistry.Bootstrappers)
            {
                bootstrapper.EntityBootstrapCompleted -= EntityBootstrapCompleted;
            }
        }

        private void EntityBootstrapCompleted(object sender, EntityBootstrapEventArgs e)
        {
            if (e.EntityType == typeof(User))
            {
                hasReceivedUserSnapshot = true;
            }
            else if (e.EntityType == typeof(Jam))
            {
                hasReceivedJamSnapshot = true;
            }
            else if (e.EntityType == typeof(Participation))
            {
                hasReceivedParticipationSnapshot = true;
            }
            else if (e.EntityType == typeof(Band))
            {
                hasReceivedBandSnapshot = true;
            }
            else if (e.EntityType == typeof(Task))
            {
                hasReceivedTaskSnapshot = true;
            }
            else
            {
                string errorMessage = $"{typeof(ServerLoginHandler).Name} class should not be bootstrapping for an entity of type {e.EntityType.Name}";
                Log.ErrorFormat(errorMessage);

                throw new ArgumentException(errorMessage);
            }

            TrySendBootstrapCompleteEvent();
        }

        private void BootstrapRepositories(int userId)
        {
            SendConnectionMessage(new EntitySnapshotRequest<User>(userId));
            SendConnectionMessage(new EntitySnapshotRequest<Jam>(userId));
            SendConnectionMessage(new EntitySnapshotRequest<Participation>(userId));
            SendConnectionMessage(new EntitySnapshotRequest<Band>(userId));
            SendConnectionMessage(new EntitySnapshotRequest<Task>(userId));
        }

        private bool CreateConnection(IPAddress targetAddress, int targetPort)
        {
            const int timeoutSeconds = 5;

            Log.Info("ClientService looking for server with address: " + targetAddress + ":" + targetPort);

            serverConnection.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            IAsyncResult asyncResult = serverConnection.BeginConnect(targetAddress.ToString(), targetPort, null, null);
            WaitHandle waitHandle = asyncResult.AsyncWaitHandle;
            try
            {
                if (!asyncResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(timeoutSeconds), false))
                {
                    serverConnection.Close();
                    Log.Warn("Timed out trying to find server.");
                    return false;
                }

                serverConnection.EndConnect(asyncResult);
                Log.Info("ClientService found server, connection created");

                return true;
            }
            catch (SocketException socketException)
            {
                Log.WarnFormat("Port value is incorrect. {0}", socketException);
                return false;
            }
            finally
            {
                waitHandle.Close();
            }
        }

        private void TrySendBootstrapCompleteEvent()
        {
            if (HasReceivedAllBootstraps())
            {
                Log.Debug("Client bootstrap complete. Sending Bootstrap Completed event.");
                OnBootstrapCompleted();
            }
        }

        private bool HasReceivedAllBootstraps()
        {
            return hasReceivedUserSnapshot &&
                   hasReceivedJamSnapshot &&
                   hasReceivedParticipationSnapshot &&
                   hasReceivedBandSnapshot &&
                   hasReceivedTaskSnapshot;
        }

        private IMessage GetConnectionMessage()
        {
            MessageIdentifier messageIdentifier = MessageIdentifierSerialiser.DeserialiseMessageIdentifier(serverConnection.GetStream());

            IMessageSerialiser messageSerialiser = SerialiserFactory.GetSerialiser(messageIdentifier);

            return messageSerialiser.Deserialise(serverConnection.GetStream());
        }

        private void SendConnectionMessage(IMessage message)
        {
            IMessageSerialiser messageSerialiser = SerialiserFactory.GetSerialiser(message.MessageIdentifier);
            messageSerialiser.Serialise(serverConnection.GetStream(), message);
        }

        private void OnBootstrapCompleted()
        {
            EventUtility.SafeFireEvent(BootstrapCompleted, this);
        }
    }
}