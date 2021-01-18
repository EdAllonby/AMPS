using System;
using System.Globalization;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Client.Service;
using Client.ViewModel.Commands;
using Shared;
using Shared.Domain;
using Utility;

namespace Client.ViewModel.MainViewModel
{
    public sealed class TaskCommentViewModel : ViewModel, IEquatable<TaskCommentViewModel>
    {
        private const int IndentationFactor = 30;
        public readonly TaskComment TaskComment;
        private string relativeTime;
        private bool reply;
        private string replyComment;

        public TaskCommentViewModel(IServiceRegistry serviceRegistry, TaskComment comment, int child) : base(serviceRegistry)
        {
            Username = comment.Commenter.Username;
            RelativeTime = comment.TimePosted.TimeAgo();
            LeftMargin = new Thickness(child * IndentationFactor, 0, 0, 0);
            TaskComment = comment;
            TimePosted = comment.TimePosted.ToString(CultureInfo.InvariantCulture);
            UpdateTimePosted();

            var myTimer = new Timer(30 * 1000);
            myTimer.Start();
            myTimer.Elapsed += (sender, args) => UpdateTimePosted();
        }

        public Thickness LeftMargin { get; }

        public bool Reply
        {
            get => reply;
            set
            {
                reply = value;
                OnPropertyChanged();
            }
        }

        public string Username { get; }

        public string RelativeTime
        {
            get => relativeTime;
            set
            {
                relativeTime = value;
                OnPropertyChanged();
            }
        }

        public string TimePosted { get; }

        public string Comment => TaskComment.Comment;

        public string ReplyComment
        {
            get => replyComment;
            set
            {
                replyComment = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddReply => new RelayCommand(AddReplyToComment, CanAddReplyToComment);

        public bool Equals(TaskCommentViewModel other)
        {
            return TaskComment.Equals(other.TaskComment);
        }

        private void UpdateTimePosted()
        {
            RelativeTime = TaskComment.TimePosted.TimeAgo();
        }

        private void AddReplyToComment()
        {
            var clientService = ServiceRegistry.GetService<IClientService>();

            clientService.AddTaskComment(TaskComment.Task, ReplyComment, TaskComment);

            ReplyComment = string.Empty;
            Reply = false;
        }

        private bool CanAddReplyToComment()
        {
            return !string.IsNullOrWhiteSpace(ReplyComment);
        }
    }
}