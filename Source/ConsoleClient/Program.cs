using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Client.Service;
using log4net;
using Shared;
using Shared.Domain;
using Shared.Repository;

namespace ConsoleClient
{
    /// <summary>
    /// Bombard the Server with generated clients. Mainly for concurrency and stress testing.
    /// </summary>
    internal static class Program
    {
        private const int TotalClients = 50;
        private static readonly ILog Log = LogManager.GetLogger(typeof (Program));
        private static readonly List<TestClient> Clients = new List<TestClient>();

        private static void Main()
        {
            Log.Debug("Start");

            for (int i = 0; i < TotalClients; i++)
            {
                var serviceRegistry = new ServiceRegistry();
                var repositoryManager = new RepositoryManager();
                repositoryManager.AddRepository<User>(new UserRepository(new InMemoryEntityPersister<User>()));
                repositoryManager.AddRepository<Jam>(new JamRepository(new InMemoryEntityPersister<Jam>()));
                repositoryManager.AddRepository<Participation>(new ParticipationRepository(new InMemoryEntityPersister<Participation>()));
                repositoryManager.AddRepository<Task>(new TaskRepository(new InMemoryEntityPersister<Task>()));
                repositoryManager.AddRepository<Band>(new BandRepository(new InMemoryEntityPersister<Band>()));

                serviceRegistry.RegisterService<RepositoryManager>(repositoryManager);
                var client = new TestClient(serviceRegistry);
                Clients.Add(client);
            }

            LoginClients();

            // Wait for Bootstrap Complete events...
            Thread.Sleep(5000);

            Log.Info("-----------Beginning Client User Repository Validation Tests-----------");

            foreach (TestClient client in Clients)
            {
                Log.InfoFormat("Testing Client {0}:", client.ClientUserId);
                Validator.ValidateUserRepository(client.ClientServiceRegistry.GetService<RepositoryManager>().GetRepository<User>(), 55);
            }

            // SimulatorUtilities.CreateTenFiveMemberBands(Clients);

            // SimulatorUtilities.CreateTwentyTasksPerBand(Clients);

            Console.ReadKey();
        }

        private static void LoginClients()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

            for (int clientNumber = 0; clientNumber < TotalClients; clientNumber++)
            {
                Clients[clientNumber].LogOn(new LoginDetails(string.Format("$TestUser{0}", clientNumber), "Password1!", ipAddress, 5004));
            }

            // Make sure all clients have finished running their initialisation threads.
            Thread.Sleep(1000);
        }
    }
}