using System.ServiceProcess;
using System.Threading;
using log4net;
using Shared;

namespace Server
{
    /// <summary>
    /// A service to run the <see cref="Server" />.
    /// </summary>
    public class ServerService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ServerService));
        private Server server;

        private void StartServer(bool useDatabasePersistence)
        {
            Log.DebugFormat("Using database persistence: {0}", useDatabasePersistence);

            IServiceRegistry serviceRegistry = ServerRegistryCreator.RegisterServices(useDatabasePersistence);
            server = new Server(serviceRegistry);
            server.Start();
        }

        /// <summary>
        /// Creates a <see cref="Server" /> in a new thread when the windows service has started.
        /// </summary>
        /// <param name="args"></param>
        public void OnStart(string[] args)
        {
            bool useDatabasePersistence = bool.Parse(args[0]);

            var serverThread = new Thread(() => StartServer(useDatabasePersistence)) { Name = "Server Thread" };
            serverThread.Start();
        }

        /// <summary>
        /// Shuts down the <see cref="Server" /> when the windows service has stopped.
        /// </summary>
        public void OnStop()
        {
            server?.Shutdown();
        }
    }
}