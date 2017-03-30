using System;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using log4net;

namespace Server
{
    /// <summary>
    /// Entry point of the service.
    /// </summary>
    internal static class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));
        private static bool serviceRunningInteractive;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            Thread mainThread = Thread.CurrentThread;
            mainThread.Name = "Main Thread";

            var servicesToRun = new ServiceBase[]
            {
                new ServerService()
            };

            if (Environment.UserInteractive)
            {
                serviceRunningInteractive = true;
                RunInteractive(servicesToRun);
            }

            ServiceBase.Run(servicesToRun);
        }

        private static void RunInteractive(ServiceBase[] servicesToRun)
        {
            Console.WriteLine("Enter Persistence Strategy:");
            Console.WriteLine("1 - Database Persistence (Warning: you need to have a database set up in app.config. Do not use for demonstration purposes.)");
            Console.WriteLine("2 - In Memory Persistence (Use this strategy without needing a database. Warning: All data lost after each server session.)");

            ConsoleKeyInfo keyPress = Console.ReadKey();

            int persistenceType;

            if (int.TryParse(keyPress.KeyChar.ToString(), out persistenceType))
            {
                if (persistenceType == 1 || persistenceType == 2)
                {
                    bool useDatabasePersistence = persistenceType == 1;

                    RunServer(servicesToRun, useDatabasePersistence);
                }
            }
        }

        private static void RunServer(ServiceBase[] servicesToRun, bool useDatabasePersistence)
        {
            Log.Debug("Services running in interactive mode.");

            MethodInfo onStartMethod = typeof(ServiceBase).GetMethod("OnStart", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                Log.DebugFormat("Starting {0}...", service.ServiceName);
                onStartMethod.Invoke(service, new object[] { new[] { useDatabasePersistence.ToString() } });
                Log.Debug("Started");
            }

            var runService = true;

            Log.Debug("Press Q key to stop the services and end the process...");
            while (runService && !serviceRunningInteractive)
            {
                ConsoleKey keyPressed = Console.ReadKey().Key;

                if (keyPressed.Equals(ConsoleKey.Q))
                {
                    runService = false;
                }
            }

            Console.ReadKey();

            MethodInfo onStopMethod = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (ServiceBase service in servicesToRun)
            {
                Log.DebugFormat("Stopping {0}...", service.ServiceName);
                onStopMethod.Invoke(service, null);
                Log.Info("Server Stopped");
            }

            Log.Info("All services stopped.");
        }
    }
}