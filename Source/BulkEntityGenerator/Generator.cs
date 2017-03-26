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
        private readonly IRepositoryManager repositoryManager = new EmptyRepositoryManager();
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
    }
}