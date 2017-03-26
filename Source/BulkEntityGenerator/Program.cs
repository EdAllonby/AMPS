using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Client.Service;
using log4net;
using Shared;
using Shared.Domain;
using Shared.Repository;
using Task = Shared.Domain.Task;

namespace BulkEntityGenerator
{
    /// <summary>
    /// Bombard the Server with generated clients. Mainly for concurrency and stress testing.
    /// </summary>
    internal static class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));
        private static readonly IPAddress IpAddress = IPAddress.Parse("127.0.0.1");

        private static void Main()
        {
            //LoginUser(1);

            LoginUsers(1000);

            Log.Debug("Start");

            Console.ReadKey();
        }

        private static void LoginUsers(int count)
        {
            Parallel.For(0, count, LoginUser);
        }

        private static void LoginUser(int i)
        {
            var serviceRegistry = new ServiceRegistry();

            var repositoryManager = new RepositoryManager(PersistenceStrategy.InMemory)
            {
                RepositoryEntityTypes = new List<Type> { typeof(User), typeof(Participation), typeof(Band), typeof(Jam), typeof(Task), typeof(TaskComment) }
            };

            repositoryManager.CreateRepositories();

            serviceRegistry.RegisterService<IRepositoryManager>(repositoryManager);

            var client = new ClientService(serviceRegistry);

            client.LogOn(new LoginDetails($"generatedUser{i}", "Password1!", IpAddress, 5004));
        }
    }
}