using System;
using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;
using Utility;

namespace Client.Service.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="EntityNotification{TEntity}" /> the Client received.
    /// </summary>
    internal sealed class JamNotificationHandler : MessageHandler<EntityNotification<Jam>>
    {
        public JamNotificationHandler(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
        }

        protected override void HandleMessage(EntityNotification<Jam> message)
        {
            var toastNotifier = ServiceRegistry.GetService<ToastNotificationManager>();

            var jamRepository = (JamRepository) ServiceRegistry.GetService<IRepositoryManager>().GetRepository<Jam>();

            switch (message.NotificationType)
            {
                case NotificationType.Create:
                    jamRepository.AddEntity(message.Entity);
                    CreateNotification(toastNotifier, message);
                    break;

                case NotificationType.Update:
                    jamRepository.UpdateEntity(message.Entity);
                    break;
            }
        }

        private static void CreateNotification(ToastNotificationManager toastNotifier, EntityNotification<Jam> jamNotification)
        {
            Jam jam = jamNotification.Entity;
            TimeSpan jamLength = jam.JamEndDate - DateTime.Now;

            toastNotifier.Notify(new Notification("A Jam has been created", $"A jam has been created ending in {jamLength.ToDays(false)}."));
        }
    }
}