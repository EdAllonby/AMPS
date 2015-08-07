using System;
using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Repository;
using Utility;

namespace Client.Service.MessageHandler
{
    /// <summary>
    /// Handles a <see cref="EntityNotification{T}" /> the Client received.
    /// </summary>
    internal sealed class JamNotificationHandler : MessageHandler<EntityNotification<Jam>>
    {
        public override void HandleMessage(EntityNotification<Jam> message, IServiceRegistry serviceRegistry)
        {
            var toastNotifier = serviceRegistry.GetService<ToastNotificationManager>();

            var jamRepository = (JamRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Jam>();

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