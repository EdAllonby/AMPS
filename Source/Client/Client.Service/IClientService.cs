using System;
using System.Collections.Generic;
using System.Net;
using Shared;
using Shared.Domain;
using Shared.Message.JamMessage;
using Shared.Message.LoginMessage;
using Shared.Message.ParticipationMessage;
using Shared.Message.TaskMessage;

namespace Client.Service
{
    /// <summary>
    /// API to create a Client <see cref="IService" /> to communicate with a Server.
    /// </summary>
    public interface IClientService : IService
    {
        /// <summary>
        /// Information of the Server.
        /// </summary>
        IPEndPoint ServerEndPoint { get; }

        /// <summary>
        /// This Client's unique <see cref="User" /> Id.
        /// </summary>
        int ClientUserId { get; }

        /// <summary>
        /// Fires when the client has finished successfully bootstrapping.
        /// </summary>
        event EventHandler BootstrapCompleted;

        /// <summary>
        /// Fires when the client has been disconnected.
        /// </summary>
        event EventHandler ClientDisconnected;

        /// <summary>
        /// Connects the Client to the server using the parameters as connection details
        /// and gets the state of <see cref="ClientService" /> up to date with the user status'.
        /// </summary>
        /// <param name="loginDetails">The details used to log in to the server.</param>
        LoginResult LogOn(LoginDetails loginDetails);

        /// <summary>
        /// Log off the currently connected client.
        /// </summary>
        void LogOff();

        /// <summary>
        /// Create a new <see cref="Band" />.
        /// </summary>
        /// <param name="userIds">The users to include in the <see cref="Band" />.</param>
        /// <param name="name">The name of the <see cref="Band" />.</param>
        /// <param name="leaderId">The leader of the <see cref="Band" />.</param>
        void CreateBand(List<int> userIds, string name, int leaderId);

        /// <summary>
        /// Sends a <see cref="TaskRequest" /> message to the server.
        /// </summary>
        /// <param name="bandId">The Id of the <see cref="Shared.Domain.Task" /> the Client wants to send the message to.</param>
        /// <param name="taskTitle">The title of the <see cref="Shared.Domain.Task" />.</param>
        /// <param name="taskDescription">The description of the <see cref="Shared.Domain.Task" />.</param>
        /// <param name="taskPoints">How many points the <see cref="Shared.Domain.Task" /> is estimated as.</param>
        /// <param name="assignedUserId">The member who will complete the <see cref="Shared.Domain.Task" />.</param>
        /// <param name="taskCategory">The <see cref="Shared.Domain.Task" />'s category.</param>
        void AddTaskToBacklog(int bandId, string taskTitle, string taskDescription, int taskPoints, int assignedUserId, TaskCategory taskCategory);

        /// <summary>
        /// Sends a <see cref="TaskUpdateRequest" /> message to the server.
        /// </summary>
        /// <param name="task">The <see cref="Shared.Domain.Task" /> to update.</param>
        void UpdateTask(Task task);

        /// <summary>
        /// Sends a <see cref="ParticipationRequest" /> message to the server to add a user to an existing <see cref="Band" />.
        /// </summary>
        /// <param name="userId">The participant that will be added to the <see cref="Band" />.</param>
        /// <param name="bandId">The targetted <see cref="Band" /> the participant will be added to.</param>
        /// <param name="isLeader">Is this <see cref="User" /> a leader of the <see cref="Band" />?</param>
        void AddUserToBand(int userId, int bandId, bool isLeader);

        /// <summary>
        /// Sends a new <see cref="JamRequest" /> to the Server.
        /// </summary>
        /// <param name="bandId">The <see cref="Band" /> whose <see cref="Jam" /> to create.</param>
        /// <param name="taskIds">The <see cref="Task" />s to enter the <see cref="Jam" />.</param>
        /// <param name="jamEndDate">The requested end date of the <see cref="Jam" />.</param>
        void CreateJam(int bandId, List<int> taskIds, DateTime jamEndDate);
    }
}