using System.Collections.Generic;
using System.Linq;
using log4net;
using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Message.JamMessage;
using Shared.Repository;

namespace Server.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="JamRequest" /> the Server received.
    /// </summary>
    internal sealed class JamRequestHandler : IMessageHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (JamRequestHandler));

        /// <summary>
        /// Handles the incoming <see cref="JamRequest" />.
        /// </summary>
        /// <param name="message">The <see cref="JamRequest" /> that has been received and needs to be handled.</param>
        /// <param name="serviceRegistry">The services needed to handle the message correctly.</param>
        public void HandleMessage(IMessage message, IServiceRegistry serviceRegistry)
        {
            JamRequest jamRequest = (JamRequest) message;
            BandRepository bandRepository = (BandRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Band>();

            if (JamValidator.IsJamEndDateValid(jamRequest.JamEndDate))
            {
                if (JamValidator.IsBandValid(jamRequest, bandRepository))
                {
                    var entityIdAllocatorFactory = serviceRegistry.GetService<EntityIdAllocatorFactory>();

                    var jam = new Jam(entityIdAllocatorFactory.AllocateEntityId<Jam>(), jamRequest.BandId, jamRequest.JamEndDate);

                    IEntityRepository<Jam> jamRepository = (IEntityRepository<Jam>) serviceRegistry.GetService<RepositoryManager>().GetRepository<Jam>();

                    TaskRepository taskRepository = (TaskRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Task>();

                    List<Task> jamTasks = jamRequest.TaskIds.Select(taskId => taskRepository.FindEntityById(taskId)).ToList();

                    if (JamValidator.AreJamTasksValid(jamTasks))
                    {
                        jamRepository.AddEntity(jam);

                        foreach (Task jamTask in jamTasks)
                        {
                            jamTask.AssignTaskToJam(jam.Id);
                            taskRepository.UpdateEntity(jamTask);
                        }
                    }
                    else
                    {
                        Log.Warn("Jam could not be created because one of the tasks were invalid.");
                    }
                }
            }
        }
    }
}