using System.Collections.Generic;
using System.Linq;
using log4net;
using Shared;
using Shared.Domain;
using Shared.Message.JamMessage;
using Shared.Repository;

namespace Server.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="JamRequest" /> the Server received.
    /// </summary>
    internal sealed class JamRequestHandler : MessageHandler<JamRequest>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (JamRequestHandler));

        /// <summary>
        /// Handles the incoming <see cref="JamRequest" />.
        /// </summary>
        /// <param name="message">The <see cref="JamRequest" /> that has been received and needs to be handled.</param>
        /// <param name="serviceRegistry">The services needed to handle the message correctly.</param>
        public override void HandleMessage(JamRequest message, IServiceRegistry serviceRegistry)
        {
            BandRepository bandRepository = (BandRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Band>();

            if (JamValidator.IsJamEndDateValid(message.JamEndDate))
            {
                if (JamValidator.IsBandValid(message, bandRepository))
                {
                    var entityIdAllocatorFactory = serviceRegistry.GetService<EntityIdAllocatorFactory>();

                    var jam = new Jam(entityIdAllocatorFactory.AllocateEntityId<Jam>(), message.BandId, message.JamEndDate);

                    IEntityRepository<Jam> jamRepository = (IEntityRepository<Jam>) serviceRegistry.GetService<RepositoryManager>().GetRepository<Jam>();

                    TaskRepository taskRepository = (TaskRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Task>();

                    List<Task> jamTasks = message.TaskIds.Select(taskId => taskRepository.FindEntityById(taskId)).ToList();

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