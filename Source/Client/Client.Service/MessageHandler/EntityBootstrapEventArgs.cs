using System;

namespace Client.Service.MessageHandler
{
    public class EntityBootstrapEventArgs : EventArgs
    {
        public Type EntityType { get; }

        public EntityBootstrapEventArgs(Type entityType)
        {
            EntityType = entityType;
        }
    }
}