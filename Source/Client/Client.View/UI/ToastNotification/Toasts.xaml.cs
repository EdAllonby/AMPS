using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Client.View.UI.ToastNotification
{
    public partial class Toasts
    {
        private const byte MaxNotifications = 4;
        private int count;
        private readonly Notifications notifications = new Notifications();
        private readonly Notifications buffer = new Notifications();

        public Toasts()
        {
            InitializeComponent();
            NotificationsControl.DataContext = notifications;
        }

        public void AddNotification(Notification notification)
        {
            notification.Id = count++;
            if (notifications.Count + 1 > MaxNotifications)
                buffer.Add(notification);
            else
                notifications.Add(notification);

            //Show window if there're notifications
            if (notifications.Count > 0 && !IsActive)
                Show();
        }

        private void RemoveNotification(Notification notification)
        {
            if (notifications.Contains(notification))
                notifications.Remove(notification);

            if (buffer.Count > 0)
            {
                notifications.Add(buffer[0]);
                buffer.RemoveAt(0);
            }

            //Close window if there's nothing to show
            if (notifications.Count < 1)
                Hide();
        }

        private void NotificationWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Height != 0.0)
            {
                return;
            }

            var element = sender as Grid;
            RemoveNotification(notifications.First(n => element != null && n.Id == int.Parse(element.Tag.ToString())));
        }
    }
}
