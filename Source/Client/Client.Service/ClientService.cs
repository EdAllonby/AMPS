using System;
using System.Collections.Generic;
using System.Net;
using Client.Service.MessageHandler;
using log4net;
using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Message.BandMessage;
using Shared.Message.JamMessage;
using Shared.Message.LoginMessage;
using Shared.Message.ParticipationMessage;
using Shared.Message.TaskMessage;
using Utility;

namespace Client.Service
{
    /// <summary>
    /// Handles the logic for <see cref="IMessage" />.
    /// Delegates Server specific communications to the <see cref="connectionHandler" />.
    /// </summary>
    public class ClientService : IClientService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ClientService));

        private readonly MessageHandlerRegistry messageHandlerRegistry;
        private readonly IRepositoryManager repositoryManager;

        /// <summary>
        /// The client's service registry
        /// </summary>
        protected readonly IServiceRegistry ServiceRegistry;

        private ConnectionHandler connectionHandler;
        private ServerLoginHandler serverLoginHandler;

        /// <summary>
        /// Passes the service the reference to the <see cref="IServiceRegistry" />.
        /// </summary>
        /// <param name="serviceRegistry">Contains a housing for client services.</param>
        public ClientService(IServiceRegistry serviceRegistry)
        {
            messageHandlerRegistry = new MessageHandlerRegistry(serviceRegistry);
            ServiceRegistry = serviceRegistry;
            repositoryManager = ServiceRegistry.GetService<IRepositoryManager>();
        }

        public User ClientUser => repositoryManager.GetRepository<User>().FindEntityById(ClientUserId);

        /// <summary>
        /// Gets fired when bootstrapping the repository is complete.
        /// </summary>
        public event EventHandler BootstrapCompleted;

        /// <summary>
        /// Fires when the client has been disconnected.
        /// </summary>
        public event EventHandler ClientDisconnected;

        /// <summary>
        /// Information of the Server.
        /// </summary>
        public IPEndPoint ServerEndPoint => connectionHandler.RemoteEndPoint;

        /// <summary>
        /// This Client's unique User Id.
        /// </summary>
        public int ClientUserId { get; private set; }

        /// <summary>
        /// Connects the Client to the server using the parameters as connection details
        /// and gets the state of <see cref="ClientService" /> up to date with the user status'.
        /// </summary>
        /// <param name="loginDetails">The details used to log in to the server.</param>
        public LoginResult LogOn(LoginDetails loginDetails)
        {
            serverLoginHandler = new ServerLoginHandler(messageHandlerRegistry);

            serverLoginHandler.BootstrapCompleted += OnBootstrapCompleted;

            LoginResponse response = serverLoginHandler.ConnectToServer(loginDetails, out connectionHandler);

            switch (response.LoginResult)
            {
                case LoginResult.Success:
                    ClientUserId = response.User.Id;
                    connectionHandler.MessageReceived += OnNewMessageReceived;
                    break;
                case LoginResult.AlreadyConnected:
                    Log.WarnFormat("User {0} already connected.", loginDetails.Username);
                    break;
                case LoginResult.ServerNotFound:
                    Log.WarnFormat("Cannot find server.");
                    break;
            }

            return response.LoginResult;
        }

        /// <summary>
        /// Log off the currently connected client.
        /// </summary>
        public void LogOff()
        {
            RemoveSubscriptions();

            connectionHandler.SendMessage(new ClientDisconnection(ClientUserId));

            ClientUserId = 0;

            repositoryManager.FlushAll();

            EventUtility.SafeFireEvent(ClientDisconnected, this);
        }

        /// <summary>
        /// Create a new Band.
        /// </summary>
        public void CreateBand(List<int> userIds, string name, int leaderId)
        {
            connectionHandler.SendMessage(new BandRequest(userIds, name, leaderId));
        }

        /// <summary>
        /// Sends a <see cref="TaskUpdateRequest" /> message to the server.
        /// </summary>
        /// <param name="task">The <see cref="Shared.Domain.Task" /> to update.</param>
        public void UpdateTask(Task task)
        {
            connectionHandler.SendMessage(new TaskUpdateRequest(task));
        }

        /// <summary>
        /// Sends a <see cref="ParticipationRequest" /> message to the server to add a user to an existing <see cref="Band" />.
        /// </summary>
        /// <param name="userId">The participant that will be added to the <see cref="Band" />.</param>
        /// <param name="bandId">The targetted <see cref="Band" /> the participant will be added to.</param>
        /// <param name="isLeader">Is the user to add a leader of the <see cref="Band" />?</param>
        public void AddUserToBand(int userId, int bandId, bool isLeader)
        {
            connectionHandler.SendMessage(new ParticipationRequest(new Participation(userId, bandId, isLeader)));
        }

        /// <summary>
        /// Sends a new <see cref="JamRequest" /> to the Server.
        /// </summary>
        /// <param name="bandId">The <see cref="Band" /> whose <see cref="Jam" /> to create.</param>
        /// <param name="taskIds">The <see cref="Task" />s to enter the <see cref="Jam" />.</param>
        /// <param name="jamEndDate">The requested end date of the <see cref="Jam" />.</param>
        public void CreateJam(int bandId, List<int> taskIds, DateTime jamEndDate)
        {
            connectionHandler.SendMessage(new JamRequest(bandId, taskIds, jamEndDate));
        }

        public void AddTaskComment(Task task, string comment, TaskComment parent)
        {
            connectionHandler.SendMessage(new TaskCommentRequest(new TaskComment(comment, task.Id, ClientUserId, parent)));
        }

        /// <summary>
        /// Sends a <see cref="TaskRequest" /> message to the server.
        /// </summary>
        /// <param name="bandId">The Id of the <see cref="Task" /> the Client wants to send the message to.</param>
        /// <param name="taskTitle">The title of the <see cref="Task" />.</param>
        /// <param name="taskDescription">The description of the <see cref="Task" />.</param>
        /// <param name="taskPoints">How many points the <see cref="Task" /> is estimated as.</param>
        /// <param name="assignedUserId">The member who will complete the <see cref="Task" />.</param>
        /// <param name="taskCategory">The <see cref="Task" />'s category.</param>
        public void AddTaskToBacklog(int bandId, string taskTitle, string taskDescription, int taskPoints, int assignedUserId, TaskCategory taskCategory)
        {
            connectionHandler.SendMessage(new TaskRequest(new Task(taskTitle, taskDescription, taskPoints, bandId, assignedUserId, taskCategory)));
        }

        private void OnNewMessageReceived(object sender, MessageEventArgs e)
        {
            IMessage message = e.Message;

            try
            {
                IMessageHandler handler = messageHandlerRegistry.MessageHandlersIndexedByMessageIdentifier[message.MessageIdentifier];

                handler.HandleMessage(message);
            }
            catch (KeyNotFoundException keyNotFoundException)
            {
                Log.Error("ClientService is not supposed to handle message with identifier: " + e.Message.MessageIdentifier, keyNotFoundException);
            }
        }

        private void OnBootstrapCompleted(object sender, EventArgs e)
        {
            EventUtility.SafeFireEvent(BootstrapCompleted, this);
        }

        private void RemoveSubscriptions()
        {
            serverLoginHandler.BootstrapCompleted -= OnBootstrapCompleted;
            connectionHandler.MessageReceived -= OnNewMessageReceived;
        }
    }
}