using System;
using System.Windows;
using System.Windows.Input;
using Client.Service;
using Client.ViewModel.Commands;
using Shared;
using Shared.Domain;

namespace Client.ViewModel.MainViewModel
{
    public sealed class TaskCommentViewModel : ViewModel, IEquatable<TaskCommentViewModel>
    {
        private const int IndentationFactor = 30;
        public readonly TaskComment TaskComment;
        private bool reply;
        private string replyComment;

        public TaskCommentViewModel(IServiceRegistry serviceRegistry, TaskComment comment, int child) : base(serviceRegistry)
        {
            LeftMargin = new Thickness(child*IndentationFactor, 0, 0, 0);
            TaskComment = comment;
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