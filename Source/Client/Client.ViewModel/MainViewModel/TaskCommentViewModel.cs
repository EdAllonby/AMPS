using System;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Client.Service;
using Client.ViewModel.Commands;
using Shared;
using Shared.Domain;
using Shared.Repository;
using Utility;

namespace Client.ViewModel.MainViewModel
{
    public sealed class TaskCommentViewModel : ViewModel, IEquatable<TaskCommentViewModel>
    {
        private const int IndentationFactor = 30;
        public readonly TaskComment TaskComment;
        private bool reply;
        private string replyComment;
        private readonly string username;
        private string relativeTime;
        private readonly string timePosted;

        public TaskCommentViewModel(IServiceRegistry serviceRegistry, TaskComment comment, int child) : base(serviceRegistry)
        {
            IReadOnlyEntityRepository<User> userRepository = serviceRegistry.GetService<RepositoryManager>().GetRepository<User>();

            username = userRepository.FindEntityById(comment.CommenterId).Username;
            RelativeTime = comment.TimePosted.TimeAgo();
            LeftMargin = new Thickness(child*IndentationFactor, 0, 0, 0);
            TaskComment = comment;
            timePosted = comment.TimePosted.ToString();
            UpdateTimePosted();

            Timer myTimer = new Timer(30 * 1000);
            myTimer.Start();
            myTimer.Elapsed += (sender, args) => UpdateTimePosted();

        }

        public Thickness LeftMargin { get; private set; }

        public bool Reply
        {
            get { return reply; }
            set
            {
                reply = value;
                OnPropertyChanged();
            }
        }

        public string Username
        {
            get { return username; }
        }

        public string RelativeTime
        {
            get { return relativeTime; }
            set
            {
                relativeTime = value;
                OnPropertyChanged();
            }
        }

        public string TimePosted
        {
            get { return timePosted; }
        }

        public string Comment
        {
            get { return TaskComment.Comment; }
        }

        public bool Equals(TaskCommentViewModel other)
        {
            return TaskComment.Equals(other.TaskComment);
        }

        public string ReplyComment
        {
            get { return replyComment; }
            set
            {
                replyComment = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddReply
        {
            get { return new RelayCommand(AddReplyToComment, CanAddReplyToComment); }
        }

        private void UpdateTimePosted()
        {
            RelativeTime = TaskComment.TimePosted.TimeAgo();
        }

        private void AddReplyToComment()
        {
            IClientService clientService = ServiceRegistry.GetService<IClientService>();

            clientService.AddTaskComment(TaskComment.TaskId, ReplyComment, TaskComment);
            
            ReplyComment = string.Empty;
            Reply = false;
        }

        private bool CanAddReplyToComment()
        {
            return !string.IsNullOrWhiteSpace(ReplyComment);
        }
    }
}