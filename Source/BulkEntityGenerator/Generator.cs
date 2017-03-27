using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Client.Service;
using Shared;
using Shared.Domain;
using Shared.Repository;
using Utility;

namespace BulkEntityGenerator
{
    internal class Generator
    {
        private static readonly IPAddress IpAddress = IPAddress.Parse("127.0.0.1");
        private readonly IRepositoryManager repositoryManager = new RepositoryManager(PersistenceStrategy.InMemory);
        private readonly IServiceRegistry serviceRegistry = new ServiceRegistry();
        private readonly string username;
        private ClientService clientService;

        public Generator(string username)
        {
            this.username = username;
        }

        public event EventHandler BootstrapCompleted
        {
            add { clientService.BootstrapCompleted += value; }
            remove { clientService.BootstrapCompleted -= value; }
        }

        public void LoginUser()
        {
            repositoryManager.RepositoryEntityTypes = new List<Type> { typeof(User), typeof(Participation), typeof(Band), typeof(Jam), typeof(Task), typeof(TaskComment) };

            repositoryManager.CreateRepositories();

            serviceRegistry.RegisterService<IRepositoryManager>(repositoryManager);
            serviceRegistry.RegisterService<ToastNotificationManager>(new ToastNotificationManager());

            clientService = new ClientService(serviceRegistry);

            clientService.LogOn(new LoginDetails(username, "Password1!", IpAddress, 5004));
        }

        public void CreateBand()
        {
            IReadOnlyEntityRepository<User> userRepository = repositoryManager.GetRepository<User>();
            List<int> bandMembers = userRepository.GetAllEntities().Select(user => user.Id).ToList();
            int leader = bandMembers.PickRandom();

            clientService.CreateBand(bandMembers, $"$GodBand", leader);
        }

        public void CreateTask()
        {
            IReadOnlyEntityRepository<Band> bandRepository = repositoryManager.GetRepository<Band>();
            Band randomBand = clientService.ClientUser.Bands.PickRandom();
            User randomAssignedMember = randomBand.Members.PickRandom();
            int randomFibonacciNumber = NumbersGenerator.CreateFibonacciSequence(50, true).PickRandom();
            const string taskDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." +
                                           " Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor " +
                                           "in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, " +
                                           "sunt in culpa qui officia deserunt mollit anim id est laborum.";

            TaskCategory randomCategory = EnumUtility.EnumToEnumerable<TaskCategory>().PickRandom();

            clientService.AddTaskToBacklog(randomBand.Id, "Task", taskDescription, randomFibonacciNumber, randomAssignedMember.Id, randomCategory);
        }
    }
}