using System;
using System.Windows.Input;
using Client.Model.SettingsModel;
using Client.Service;
using Client.ViewModel.Commands;
using Shared;
using Shared.Domain;
using Shared.Repository;
using Utility;

namespace Client.ViewModel.MainViewModel
{
    /// <summary>
    /// The View Model for the a <see cref="Task" /> Item.
    /// </summary>
    public sealed class TaskItemViewModel : TaskInformationViewModel, IEquatable<TaskItemViewModel>
    {
        private readonly Task task;
        private string comment = string.Empty;

        /// <summary>
        /// Create a new View Model for the Jam Tasks View.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="task">The <see cref="Task" /> to create a view model for.</param>
        public TaskItemViewModel(IServiceRegistry serviceRegistry, Task task) : base(serviceRegistry)
        {
            this.task = task;

            var userRepository = (IEntityRepository<User>) ServiceRegistry.GetService<RepositoryManager>().GetRepository<User>();

            var assignedMember = userRepository.FindEntityById(task.AssignedUserId);

            TaskModel = new TaskModel(task, assignedMember);
        }

        /// <summary>
        /// The client's service registry.
        /// </summary>
        public IServiceRegistry ClientServiceRegistry
        {
            get { return ServiceRegistry; }
        }

        /// <summary>
        /// Executed when this <see cref="Task" /> is wanting to be completed.
        /// </summary>
        public ICommand CompleteTask
        {
            get { return new RelayCommand(SendTaskCompletion, CanCompleteTask); }
        }

        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Underlying model.
        /// </summary>
        public TaskModel TaskModel { get; private set; }

        public ICommand AddComment
        {
            get { return new RelayCommand(AddCommentToTask, CanAddCommentToTask); }
        }

        /// <summary>
        /// Checks if two task items are equal based on their underlying <see cref="Task" /> Id.
        /// </summary>
        /// <param name="other">The other view model to compare.</param>
        /// <returns></returns>
        public bool Equals(TaskItemViewModel other)
        {
            return TaskModel.TaskId == other.TaskModel.TaskId;
        }

        /// <summary>
        /// Fires when requesting to open Task Upload view.
        /// </summary>
        public event EventHandler<WindowRequestedEventArgs> OpenUploadTaskViewRequested;

        private bool CanCompleteTask()
        {
            if (TaskModel.IsCompleted)
            {
                return false;
            }

            IClientService clientService = ServiceRegistry.GetService<IClientService>();

            int clientUserId = clientService.ClientUserId;

            return clientUserId.Equals(TaskModel.AssignedMember.Id);
        }

        private void SendTaskCompletion()
        {
            EventUtility.SafeFireEvent(OpenUploadTaskViewRequested, this, new WindowRequestedEventArgs(task));
        }

        private bool CanAddCommentToTask()
        {
            return !string.IsNullOrWhiteSpace(Comment);
        }

        private void AddCommentToTask()
        {
            IClientService clientService = ServiceRegistry.GetService<IClientService>();

            task.Comments.Add(new TaskComment(string.Empty, Comment, null));

            clientService.UpdateTask(task);
        }
    }
}