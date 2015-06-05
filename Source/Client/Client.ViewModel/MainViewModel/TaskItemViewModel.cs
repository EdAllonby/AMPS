﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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
    /// The View Model for the a <see cref="Shared.Domain.Task" /> Item.
    /// </summary>
    public sealed class TaskItemViewModel : TaskInformationViewModel, IEquatable<TaskItemViewModel>
    {
        private readonly IReadOnlyEntityRepository<Task> taskRepository;
        private string comment = string.Empty;
        private readonly int taskId;

        private Task Task
        {
            get { return taskRepository.FindEntityById(taskId); }
        }

        /// <summary>
        /// Create a new View Model for the Jam Tasks View.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="task">The <see cref="Shared.Domain.Task" /> to create a view model for.</param>
        public TaskItemViewModel(IServiceRegistry serviceRegistry, Task task) : base(serviceRegistry)
        {
            taskId = task.Id;

            var userRepository = (IEntityRepository<User>) ServiceRegistry.GetService<RepositoryManager>().GetRepository<User>();
            taskRepository = ServiceRegistry.GetService<RepositoryManager>().GetRepository<Task>();

            taskRepository.EntityUpdated += TaskUpdated;

            var assignedMember = userRepository.FindEntityById(task.AssignedUserId);

            TaskModel = new TaskModel(task, assignedMember);
            TaskCommentViewModels = new ObservableCollection<TaskCommentViewModel>();
            UpdateComments();
        }

        /// <summary>
        /// The client's service registry.
        /// </summary>
        public IServiceRegistry ClientServiceRegistry
        {
            get { return ServiceRegistry; }
        }

        /// <summary>
        /// Executed when this <see cref="Shared.Domain.Task" /> is wanting to be completed.
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

        public ObservableCollection<TaskCommentViewModel> TaskCommentViewModels { get; private set; }

        /// <summary>
        /// Checks if two task items are equal based on their underlying <see cref="Shared.Domain.Task" /> Id.
        /// </summary>
        /// <param name="other">The other view model to compare.</param>
        /// <returns></returns>
        public bool Equals(TaskItemViewModel other)
        {
            return TaskModel.TaskId == other.TaskModel.TaskId;
        }

        public void UnsubscribeEvents()
        {
            taskRepository.EntityUpdated -= TaskUpdated;
        }

        private void TaskUpdated(object sender, EntityChangedEventArgs<Task> e)
        {
            if (e.Entity.Id.Equals(TaskModel.TaskId))
            {
                TaskModel.IsCompleted = Task.IsCompleted;

                Application.Current.Dispatcher.Invoke(UpdateComments);
            }
        }

        private void UpdateComments()
        {
            foreach (TaskComment taskComment in Task.Comments)
            {
                AddCommentTree(taskComment, 0);
            }
        }

        private void AddCommentTree(TaskComment taskComment, int level)
        {
            var viewModel = new TaskCommentViewModel(ServiceRegistry, taskComment, level);

            if (!TaskCommentViewModels.Contains(viewModel))
            {
                TaskCommentViewModel parentViewmodel = TaskCommentViewModels.FirstOrDefault(x => x.TaskComment.Equals(taskComment.ParentComment));

                if (parentViewmodel != null)
                {
                    int indexOfParent = TaskCommentViewModels.IndexOf(parentViewmodel);

                    TaskCommentViewModels.Insert(indexOfParent + 1, viewModel);
                }
                else
                {
                    TaskCommentViewModels.Add(viewModel);
                }
            }

            foreach (TaskComment reply in taskComment.Replies)
            {
                AddCommentTree(reply, level + 1);
            }
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

            var clientService = ServiceRegistry.GetService<IClientService>();

            var clientUserId = clientService.ClientUserId;

            return clientUserId.Equals(TaskModel.AssignedMember.Id);
        }

        private void SendTaskCompletion()
        {
            EventUtility.SafeFireEvent(OpenUploadTaskViewRequested, this, new WindowRequestedEventArgs(Task));
        }

        private bool CanAddCommentToTask()
        {
            return !string.IsNullOrWhiteSpace(Comment);
        }

        private void AddCommentToTask()
        {
            var clientService = ServiceRegistry.GetService<IClientService>();

            clientService.AddTaskComment(TaskModel.TaskId, Comment, null);

            Comment = String.Empty;
        }
    }
}