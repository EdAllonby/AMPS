using System;
using System.Windows;
using Shared;
using Shared.Domain;

namespace Client.ViewModel.MainViewModel
{
    public sealed class TaskCommentViewModel : ViewModel, IEquatable<TaskCommentViewModel>
    {
        private readonly TaskComment taskComment;
        private const int IndentationFactor = 30;

        public TaskCommentViewModel(IServiceRegistry serviceRegistry, TaskComment comment, int child) : base(serviceRegistry)
        {
            LeftMargin = new Thickness(child*IndentationFactor, 0, 0, 0);
            this.taskComment = comment;
        }

        public Thickness LeftMargin { get; private set; }

        public string Comment
        {
            get { return taskComment.Comment; }
        }

        public bool Equals(TaskCommentViewModel other)
        {
            return taskComment.Equals(other.taskComment);
        }
    }
}