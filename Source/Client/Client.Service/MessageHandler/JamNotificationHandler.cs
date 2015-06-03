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
    internal sealed class JamNotificationHandler : IMessageHandler
    {
        public void HandleMessage(IMessage message, IServiceRegistry serviceRegistry)
        {
            var jamNotification = (EntityNotification<Jam>) message;
            var toastNotifier = serviceRegistry.GetService<ToastNotificationManager>();

            var jamRepository = (JamRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Jam>();

            switch (jamNotification.NotificationType)
            {
                case NotificationType.Create:
                    jamRepository.AddEntity(jamNotification.Entity);
                    CreateNotification(toastNotifier, jamNotification);
                    break;

                case NotificationType.Update:
                    jamRepository.UpdateEntity(jamNotification.Entity);
                    break;
            }
        }

        private static void CreateNotification(ToastNotificationManager toastNotifier, EntityNotification<Jam> jamNotification)
        {
            Jam jam = jamNotification.Entity;
            TimeSpan jamLength = jam.JamEndDate - DateTime.Now;

            toastNotifier.Notify(new Notification("A Jam has been created", string.Format("A jam has been created ending in {0}.", jamLength.ToDays(false))));
        }
    }
}