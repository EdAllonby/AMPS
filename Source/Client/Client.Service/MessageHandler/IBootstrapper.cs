using System;

namespace Client.Service.MessageHandler
{
    public interface IBootstrapper
    {
        event EventHandler<EntityBootstrapEventArgs> EntityBootstrapCompleted;
    }
}