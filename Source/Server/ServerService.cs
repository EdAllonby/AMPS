using System.ServiceProcess;
using System.Threading;
using log4net;
using Shared;

namespace Server
{
    /// <summary>
    /// A service to run the <see cref="Server" />.
    /// </summary>
    public partial class ServerService : ServiceBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (ServerService));
        private Server server;

        /// <summary>
        /// Starts the windows service.
        /// </summary>
        public ServerService()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates a <see cref="Server" /> in a new thread when the windows service has started.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            bool useDatabasePersistence = bool.Parse(args[0]);

            var serverThread = new Thread(() => StartServer(useDatabasePersistence)) {Name = "Server Thread"};
            serverThread.Start();
        }

        private void StartServer(bool useDatabasePersistence)
        {
            Log.DebugFormat("Using database persistence: {0}", useDatabasePersistence);

            IServiceRegistry serviceRegistry = ServerRegistryCreator.RegisterServices(useDatabasePersistence);
            server = new Server(serviceRegistry);
            server.Start();
        }

        /// <summary>
        /// Shuts down the <see cref="Server" /> when the windows service has stopped.
        /// </summary>
        protected override void OnStop()
        {
            server?.Shutdown();
        }
    }
}