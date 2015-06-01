using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using Client.Service;
using Client.Service.FTPService;
using Client.View.UI;
using log4net;
using log4net.Config;
using Shared;
using Shared.Domain;
using Shared.Repository;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (App));

        /// <summary>
        /// When the application has started, give it an appropriate logging mechanism, and show the log in view.
        /// </summary>
        /// <param name="e">Startup event args.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            // Console must be started before configuring log4net.
            ConsoleManager.Show();
            SetupLogging("log4netDebug.config");
#else
            SetupLogging("log4netRelease.config");
#endif
            Thread.CurrentThread.Name = "Main Thread";

            IServiceRegistry serviceRegistry = CreateLoadedServiceRegistry();

            var loginWindow = new LoginView(serviceRegistry);
            loginWindow.Show();

            base.OnStartup(e);
        }

        private static void SetupLogging(string logConfigName)
        {
            string assemblyPath = Assembly.GetAssembly(typeof (App)).Location;
            string assemblyDirectory = Path.GetDirectoryName(assemblyPath);

            if (assemblyDirectory != null)
            {
                var uri = new Uri(Path.Combine(assemblyDirectory, logConfigName));

                XmlConfigurator.Configure(uri);
            }
        }

        private static IServiceRegistry CreateLoadedServiceRegistry()
        {
            IServiceRegistry serviceRegistry = new ServiceRegistry();

            var repositoryManager = new RepositoryManager();

            repositoryManager.AddRepository<User>(new UserRepository(new InMemoryEntityPersister<User>()));
            repositoryManager.AddRepository<Participation>(new ParticipationRepository(new InMemoryEntityPersister<Participation>()));
            repositoryManager.AddRepository<Band>(new BandRepository(new InMemoryEntityPersister<Band>()));
            repositoryManager.AddRepository<Jam>(new JamRepository(new InMemoryEntityPersister<Jam>()));
            repositoryManager.AddRepository<Task>(new TaskRepository(new InMemoryEntityPersister<Task>()));

            serviceRegistry.RegisterService<RepositoryManager>(repositoryManager);
            serviceRegistry.RegisterService<IClientService>(new ClientService(serviceRegistry));
            serviceRegistry.RegisterService<IFtpManager>(new FtpManager());
            serviceRegistry.RegisterService<ToastNotificationManager>(new ToastNotificationManager());

            return serviceRegistry;
        }

        /// <summary>
        /// Close the console if shown.
        /// </summary>
        /// <param name="e">Exit event args.</param>
        protected override void OnExit(ExitEventArgs e)
        {
#if DEBUG
            ConsoleManager.Hide();
#endif
            base.OnExit(e);
        }
    }
}