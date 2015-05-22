using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using log4net;
using Shared;
using Shared.Domain;
using Shared.Repository;
using Utility;

namespace Server
{
    /// <summary>
    /// Checks the end dates in the Server's <see cref="Jam" /> repository periodically.
    /// </summary>
    internal sealed class JamManager : IService
    {
        private const int DateCheckerRateInMilliseconds = 30000;
        private static readonly ILog Log = LogManager.GetLogger(typeof (JamManager));
        private readonly IReadOnlyEntityRepository<Jam> serverJamRepository;
        private bool canCheckForDates;

        /// <summary>
        /// Creates a <see cref="Jam" /> end date manager and enabled Date Checking.
        /// </summary>
        /// <param name="serverJamRepository">The server's repository for <see cref="Jam" />s.</param>
        public JamManager(IReadOnlyEntityRepository<Jam> serverJamRepository)
        {
            this.serverJamRepository = serverJamRepository;
            canCheckForDates = true;
        }

        /// <summary>
        /// Fires when a <see cref="Jam" /> has ended in the <see cref="JamRepository" />.
        /// </summary>
        public event EventHandler<JamEndedEventArgs> JamEndDateSurpassed;

        /// <summary>
        /// Stop checking <see cref="Jam" /> end dates.
        /// </summary>
        public void StopCheckingForDates()
        {
            canCheckForDates = false;
        }

        /// <summary>
        /// Start checking for dates periodically.
        /// </summary>
        public void CheckDates()
        {
            Thread jamDateCheckerThread = new Thread(CheckDatesWorker);
            jamDateCheckerThread.Start();
        }

        private void CheckDatesWorker()
        {
            while (canCheckForDates)
            {
                IEnumerable<Jam> activeJams = serverJamRepository.GetAllEntities().Where(jam => jam.IsActive);

                foreach (Jam activeJam in activeJams)
                {
                    if (DateTime.UtcNow > activeJam.JamEndDate)
                    {
                        JamEndedEventArgs jamEndedEventArgs = new JamEndedEventArgs(activeJam);

                        EventUtility.SafeFireEvent(JamEndDateSurpassed, this, jamEndedEventArgs);

                        Log.InfoFormat("Jam with id {0} has ended", activeJam.Id);
                    }
                    else
                    {
                        Log.DebugFormat("Active Jam with Id {0} has not yet ended.", activeJam.Id);
                    }
                }

                Thread.Sleep(DateCheckerRateInMilliseconds);
            }

            Log.Info("Jam Manager stopped checking for Jam end dates.");
        }
    }
}