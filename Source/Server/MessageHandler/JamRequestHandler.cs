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
        public JamRequestHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
        }

        /// <summary>
        /// Handles the incoming <see cref="JamRequest" />.
        /// </summary>
        /// <param name="message">The <see cref="JamRequest" /> that has been received and needs to be handled.</param>
        protected override void HandleMessage(JamRequest message)
        {
            BandRepository bandRepository = (BandRepository) ServiceRegistry.GetService<RepositoryManager>().GetRepository<Band>();

            if (JamValidator.IsJamEndDateValid(message.JamEndDate))
            {
                if (JamValidator.IsBandValid(message, bandRepository))
                {
                    var entityIdAllocatorFactory = ServiceRegistry.GetService<EntityIdAllocatorFactory>();

                    var jam = new Jam(entityIdAllocatorFactory.AllocateEntityId<Jam>(), message.BandId, message.JamEndDate);

                    var jamRepository = (IEntityRepository<Jam>) ServiceRegistry.GetService<RepositoryManager>().GetRepository<Jam>();

                    TaskRepository taskRepository = (TaskRepository) ServiceRegistry.GetService<RepositoryManager>().GetRepository<Task>();

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