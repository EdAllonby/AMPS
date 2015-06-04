using System;
using System.Windows;
using Shared;
using Shared.Domain;

namespace Client.ViewModel.MainViewModel
{
    public sealed class TaskCommentViewModel : ViewModel, IEquatable<TaskCommentViewModel>
    {
        private readonly TaskComment comment;
        private const int IndentationFactor = 30;

        public TaskCommentViewModel(IServiceRegistry serviceRegistry, TaskComment comment, int child) : base(serviceRegistry)
        {
            LeftMargin = new Thickness(child*IndentationFactor, 0, 0, 0);
            this.comment = comment;
        }

        public Thickness LeftMargin { get; private set; }

        public string Comment
        {
            get { return comment.Comment; }
        }

        public bool Equals(TaskCommentViewModel other)
        {
            return Comment.Equals(other.Comment);
        }
    }
}