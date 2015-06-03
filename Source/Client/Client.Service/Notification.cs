﻿namespace Client.Service
{
    public sealed class Notification
    {
        public Notification(string title, string message)
        {
            Message = message;
            Title = title;
        }

        public Notification(int id, Notification incompleNotification)
            : this(incompleNotification.Title, incompleNotification.Message)
        {
            Id = id;
        }

        public string Message { get; private set; }
        public int Id { get; private set; }

        public static string ImageUrl
        {
            get { return "pack://application:,,,/Resources/notification-icon.png"; }
        }

        public string Title { get; private set; }
    }
}