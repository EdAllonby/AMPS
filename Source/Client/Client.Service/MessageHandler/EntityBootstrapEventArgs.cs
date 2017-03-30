using System;

namespace Client.Service.MessageHandler
{
    public class EntityBootstrapEventArgs : EventArgs
    {
        public EntityBootstrapEventArgs(Type entityType)
        {
            EntityType = entityType;
        }

        public Type EntityType { get; }
    }
}