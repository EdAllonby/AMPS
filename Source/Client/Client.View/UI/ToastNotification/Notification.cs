namespace Client.View.UI.ToastNotification
{
    public sealed class Notification
    {
        public Notification(string message, string imageUrl, string title)
        {
            Message = message;
            ImageUrl = imageUrl;
            Title = title;
        }

        public Notification(int id, Notification incompleNotification)
            : this(incompleNotification.Message, incompleNotification.ImageUrl, incompleNotification.Title)
        {
            Id = id;
        }

        public string Message { get; private set; }

        public int Id { get; private set; }

        public string ImageUrl { get; private set; }
        public string Title { get; private set; }
    }
}