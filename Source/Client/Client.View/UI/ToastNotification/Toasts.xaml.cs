using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Client.Service;

namespace Client.View.UI.ToastNotification
{
    public partial class Toasts
    {
        private readonly ObservableCollection<Notification> buffer = new ObservableCollection<Notification>();
        private readonly ObservableCollection<Notification> notifications = new ObservableCollection<Notification>();
        private int count;
        private readonly ToastNotificationManager notificationManager;

        public Toasts(ToastNotificationManager notificationManager)
        {
            this.notificationManager = notificationManager;
  
            InitializeComponent();
            
            NotificationsControl.DataContext = notifications;
        }

        public void AddNotification(Notification newNotification)
        {
            Notification notification = new Notification(count++, newNotification);

            if (notifications.Count + 1 > notificationManager.TotalToastsToDisplay)
            {
                buffer.Add(notification);
            }
            else
            {
                notifications.Add(notification);
            }

            //Show window if there're notifications
            if (notifications.Count > 0 && !IsActive)
            {
                Show();
            }
        }

        private void RemoveNotification(Notification notification)
        {
            if (notifications.Contains(notification))
            {
                notifications.Remove(notification);
            }

            if (buffer.Count > 0)
            {
                notifications.Add(buffer[0]);
                buffer.RemoveAt(0);
            }

            //Close window if there's nothing to show
            if (notifications.Count < 1)
            {
                Hide();
            }
        }

        private void NotificationWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Math.Abs(e.NewSize.Height) > double.Epsilon)
            {
                return;
            }

            var element = sender as Grid;
            RemoveNotification(notifications.First(n => element != null && n.Id == int.Parse(element.Tag.ToString())));
        }
    }
}