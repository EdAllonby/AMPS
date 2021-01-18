﻿using System;
using System.IO;
using System.Reflection;
using System.Threading;
using log4net;
using log4net.Config;

namespace Server
{
    /// <summary>
    /// Entry point of the service.
    /// </summary>
    internal static class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));
        private static bool serviceRunningInteractive;

        private static void SetupLogging(string logConfigName)
        {
            string assemblyPath = Assembly.GetAssembly(typeof(Program)).Location;
            string assemblyDirectory = Path.GetDirectoryName(assemblyPath);

            if (assemblyDirectory != null)
            {
                var uri = new Uri(Path.Combine(assemblyDirectory, logConfigName));

                XmlConfigurator.Configure(uri);
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            SetupLogging("log4net.config");

            Thread mainThread = Thread.CurrentThread;
            mainThread.Name = "Main Thread";

            var servicesToRun = new ServerService();

            if (Environment.UserInteractive)
            {
                serviceRunningInteractive = true;
                RunInteractive(servicesToRun);
            }
        }

        private static void RunInteractive(ServerService servicesToRun)
        {
            Console.WriteLine("Enter Persistence Strategy:");
            Console.WriteLine("1 - Database Persistence (Warning: you need to have a database set up in app.config. Do not use for demonstration purposes.)");
            Console.WriteLine("2 - In Memory Persistence (Use this strategy without needing a database. Warning: All data lost after each server session.)");

            ConsoleKeyInfo keyPress = Console.ReadKey();

            if (int.TryParse(keyPress.KeyChar.ToString(), out int persistenceType))
            {
                if (persistenceType == 1 || persistenceType == 2)
                {
                    bool useDatabasePersistence = persistenceType == 1;

                    RunServer(servicesToRun, useDatabasePersistence);
                }
            }
        }

        private static void RunServer(ServerService service, bool useDatabasePersistence)
        {
            Log.Debug("Services running in interactive mode.");

            Log.DebugFormat("Starting {0}...", service);
            service.OnStart(new[] {useDatabasePersistence.ToString()});
            Log.Debug("Started");

            var runService = true;

            Log.Debug("Press Q key to stop the services and end the process...");
            while (runService)
            {
                ConsoleKey keyPressed = Console.ReadKey().Key;

                if (keyPressed.Equals(ConsoleKey.Q))
                {
                    runService = false;
                }
            }

            Console.ReadKey();

            Log.DebugFormat("Stopping {0}...", service);
            service.OnStop();
            Log.Info("Server Stopped");
        
            Log.Info("All services stopped.");
        }
    }
}