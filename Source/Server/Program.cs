using System;
using System.IO;
using System.Reflection;
using System.Threading;
using log4net;
using log4net.Config;
using Server;

ILog log = LogManager.GetLogger(Assembly.GetEntryAssembly()?.EntryPoint?.DeclaringType);
SetupLogging("log4net.config");
var mainThread = Thread.CurrentThread;
mainThread.Name = "Main Thread";
var servicesToRun = new ServerService();
if (Environment.UserInteractive)
{
    RunInteractive(servicesToRun);
}

static void SetupLogging(string logConfigName)
{
    string assemblyPath = Assembly.GetEntryAssembly()?.Location;
    string assemblyDirectory = Path.GetDirectoryName(assemblyPath);

    if (assemblyDirectory != null)
    {
        var uri = new Uri(Path.Combine(assemblyDirectory, logConfigName));

        XmlConfigurator.Configure(uri);
    }
}

void RunInteractive(ServerService server)
{
    Console.WriteLine("Enter Persistence Strategy:");
    Console.WriteLine(
        "1 - Database Persistence (Warning: you need to have a database set up in app.config. Do not use for demonstration purposes.)");
    Console.WriteLine(
        "2 - In Memory Persistence (Use this strategy without needing a database. Warning: All data lost after each server session.)");

    ConsoleKeyInfo keyPress = Console.ReadKey();

    if (int.TryParse(keyPress.KeyChar.ToString(), out int persistenceType))
        if (persistenceType == 1 || persistenceType == 2)
        {
            bool useDatabasePersistence = persistenceType == 1;

            RunServer(server, useDatabasePersistence);
        }
}

void RunServer(ServerService service, bool useDatabasePersistence)
{
    log.Debug("Services running in interactive mode.");

    log.DebugFormat("Starting {0}...", service);
    service.OnStart(new[] {useDatabasePersistence.ToString()});
    log.Debug("Started");

    var runService = true;

    log.Debug("Press Q key to stop the services and end the process...");
    while (runService)
    {
        ConsoleKey keyPressed = Console.ReadKey().Key;

        if (keyPressed.Equals(ConsoleKey.Q)) runService = false;
    }

    Console.ReadKey();

    log.DebugFormat("Stopping {0}...", service);
    service.OnStop();
    log.Info("Server Stopped");

    log.Info("All services stopped.");
}