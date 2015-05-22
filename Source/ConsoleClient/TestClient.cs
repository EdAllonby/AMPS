using Client.Service;
using Shared;

namespace ConsoleClient
{
    public sealed class TestClient : ClientService
    {
        public TestClient(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
        }

        public IServiceRegistry ClientServiceRegistry
        {
            get { return ServiceRegistry; }
        }
    }
}