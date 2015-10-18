using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using log4net;
using Server.EntityChangedHandler;
using Server.MessageHandler;
using Shared;
using Shared.Configuration;
using Shared.Domain;
using Shared.Message;
using Shared.Message.LoginMessage;
using Shared.Repository;

namespace Server
{
    /// <summary>
    /// Handles the logic for incoming <see cref="IMessage" /> passed from ConnectionHandler.
    /// </summary>
    public sealed class Server
    {
        private readonly int PortNumber;
        private const int FallbackPortNumber = 3141;
        private static readonly ILog Log = LogManager.GetLogger(typeof (Server));
        private readonly EntityChangedHandlerRegistry entityChangedHandlerRegistry;
        private readonly MessageHandlerRegistry messageHandlerRegistry;
        private readonly IServiceRegistry serviceRegistry;
        private TcpListener clientListener;
        private bool isServerRunning;
            
        /// <summary>
        /// Creates a new <see cref="Server" /> instance, and sets up the Repository Change Handlers.
        /// </summary>
        /// <param name="serviceRegistry">The <see cref="Server" />'s services.</param>
        public Server(IServiceRegistry serviceRegistry)
        {
            this.serviceRegistry = serviceRegistry;

            var configManager = serviceRegistry.GetService<AppConfigManager>();

            string storedServerPort = configManager.FindStoredValue("ServerPort");

            bool didParseServerPort = int.TryParse(storedServerPort, out PortNumber);

            if (!didParseServerPort)
            {
                PortNumber = FallbackPortNumber;
            }

            messageHandlerRegistry = new MessageHandlerRegistry(serviceRegistry);
            entityChangedHandlerRegistry = new EntityChangedHandlerRegistry(serviceRegistry);

            JamManager jamManager = serviceRegistry.GetService<JamManager>();
            jamManager.JamEndDateSurpassed += OnJamDateSurpassed;

            jamManager.CheckDates();
        }

        /// <summary>
        /// Starts the <see cref="Server" />, where it begins to listen to new clients.
        /// </summary>
        public void Start()
        {
            isServerRunning = true;

            Log.Info("Server instance started");

            ListenForNewClients();
        }

        /// <summary>
        /// Stops the <see cref="Server" /> which stops it listening for new clients, and removes the Repository change handling.
        /// </summary>
        public void Shutdown()
        {
            Log.Debug("Starting server shutdown.");
            isServerRunning = false;

            entityChangedHandlerRegistry.StopListening();

            JamManager jamManager = serviceRegistry.GetService<JamManager>();
            jamManager.StopCheckingForDates();

            clientListener.Stop();
            Log.Debug("Server shutdown process finished.");
        }

        private void OnJamDateSurpassed(object sender, JamEndedEventArgs e)
        {
            var jamRepository = (IEntityRepository<Jam>) serviceRegistry.GetService<RepositoryManager>().GetRepository<Jam>();

            Jam jamClone = Entity.DeepClone(e.FinishedJam);

            jamClone.IsActive = false;

            jamRepository.UpdateEntity(jamClone);
        }

        private void ListenForNewClients()
        {
            clientListener = new TcpListener(IPAddress.Any, PortNumber);

            clientListener.Start();

            Log.Info("Server started listening for clients to connect");

            while (isServerRunning)
            {
                TcpClient client = clientListener.AcceptTcpClient();

                Log.Info("New client connection found. Starting login initialisation process.");

                client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                InitialiseNewClient(client);
            }
        }

        private void InitialiseNewClient(TcpClient tcpClient)
        {
            IClientHandler clientHandler = new ClientHandler();

            LoginResponse loginResponse = clientHandler.InitialiseClient(tcpClient, serviceRegistry);

            if (loginResponse.LoginResult == LoginResult.Success)
            {
                clientHandler.MessageReceived += OnMessageReceived;
            }
        }

        private void OnMessageReceived(object sender, MessageEventArgs e)
        {
            IMessage message = e.Message;

            try
            {
                IMessageHandler handler = messageHandlerRegistry.MessageHandlersIndexedByMessageIdentifier[message.MessageIdentifier];

                handler.HandleMessage(message);
            }
            catch (KeyNotFoundException keyNotFoundException)
            {
                Log.Error("Server is not supposed to handle message with identifier: " + e.Message.MessageIdentifier, keyNotFoundException);
            }
        }
    }
}