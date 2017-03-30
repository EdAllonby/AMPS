using System;
using System.Collections.Generic;
using log4net;
using Shared.Domain;
using Shared.Message.JamMessage;
using Shared.Repository;

namespace Server.MessageHandler
{
    /// <summary>
    /// Checks for when a <see cref="Jam" /> is added.
    /// </summary>
    internal static class JamValidator
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(JamValidator));

        /// <summary>
        /// Checks if <see cref="Task" />s can join a <see cref="Jam" />.
        /// </summary>
        /// <param name="tasks">The <see cref="Task" />s to check.</param>
        /// <returns>Whether the tasks can enter a <see cref="Jam" />.</returns>
        public static bool AreJamTasksValid(IEnumerable<Task> tasks)
        {
            foreach (Task task in tasks)
            {
                if (task.IsInJam)
                {
                    Log.WarnFormat("Task with Id {0} is already in a Jam.", task.Id);
                    return false;
                }
                if (!task.HasAssignedUser)
                {
                    Log.WarnFormat("Task with Id {0} does not have an assigned user.", task.Id);
                    return false;
                }
                if (!task.HasPoints)
                {
                    Log.WarnFormat("Task with Id {0} has not been assigned any points.", task.Id);
                    return false;
                }
                if (string.IsNullOrWhiteSpace(task.Title))
                {
                    Log.WarnFormat("Task with Id {0} does not have a title.", task.Id);
                    return false;
                }
                if (string.IsNullOrWhiteSpace(task.Description))
                {
                    Log.WarnFormat("Task with Id {0} does not have a description", task.Id);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if the <see cref="JamRequest" /> band is available.
        /// </summary>
        /// <param name="jamRequest">The request for a new <see cref="Jam" />.</param>
        /// <param name="bandRepository">Where <see cref="Band" /> entities live.</param>
        /// <returns></returns>
        public static bool IsBandValid(JamRequest jamRequest, IReadOnlyEntityRepository<Band> bandRepository)
        {
            // If a Band exists, the Jam is allowed to be created.
            if (bandRepository.FindEntityById(jamRequest.BandId) == null)
            {
                Log.WarnFormat("Jam wasn't created because the Server could not find a Band with id {0}.", jamRequest.BandId);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if a <see cref="Jam" />s end date is valid.
        /// </summary>
        /// <param name="jamEndDate">The <see cref="Jam" />s end date.</param>
        /// <returns>Whether the date is valid for a <see cref="Jam" />s end date.</returns>
        public static bool IsJamEndDateValid(DateTime jamEndDate)
        {
            if (jamEndDate > DateTime.UtcNow.AddHours(23))
            {
                return true;
            }

            Log.Warn("Jam could not be created because its end date is too close to the threshold date.");

            return false;
        }
    }
}