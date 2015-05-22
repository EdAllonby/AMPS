using Client.Service;
using Client.ViewModel.LoginViewModel;
using NUnit.Framework;
using Shared;

namespace ClientViewModelTest
{
    [TestFixture]
    public class LoginViewModelTest
    {
        [Test]
        public void TestMethod1()
        {
            ServiceRegistry serviceRegistry = new ServiceRegistry();
            serviceRegistry.RegisterService<IClientService>(new ClientService(serviceRegistry));

            LoginViewModel loginViewModel = new LoginViewModel(serviceRegistry);

            // TODO: Mock out Client Service.
        }

        [Test]
        public void TestMethod2()
        {
            ServiceRegistry serviceRegistry = new ServiceRegistry();
            serviceRegistry.RegisterService<IClientService>(new ClientService(serviceRegistry));

            LoginViewModel loginViewModel = new LoginViewModel(serviceRegistry);

            // TODO: Mock out Client Service.
        }
    }
}