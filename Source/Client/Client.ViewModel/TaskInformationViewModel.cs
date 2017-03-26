using System;
using System.Windows;
using System.Windows.Input;
using Client.ViewModel.Commands;
using Shared;
using Shared.Domain;
using Shared.Repository;
using Utility;

namespace Client.ViewModel
{
    /// <summary>
    /// A view model that shows task information.
    /// </summary>
    public abstract class TaskInformationViewModel : ViewModel
    {
        private readonly IReadOnlyEntityRepository<Task> taskRepository;

        /// <summary>
        /// Initialise a new Task Information type of view model.
        /// </summary>
        /// <param name="serviceRegistry"></param>
        protected TaskInformationViewModel(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
            taskRepository = serviceRegistry.GetService<IRepositoryManager>().GetRepository<Task>();

            HyperlinkClicked = new OpenTaskDetailsCommand(this);

            DownloadClicked = new DownloadTaskCommand(this);
        }

        /// <summary>
        /// When a more info link is clicked.
        /// </summary>
        public ICommand HyperlinkClicked { get; private set; }

        /// <summary>
        /// When a download link is clicked.
        /// </summary>
        public ICommand DownloadClicked { get; private set; }

        /// <summary>
        /// Attempt to download a <see cref="Task" /> from the FTP.
        /// </summary>
        /// <param name="taskId">The Id of the <see cref="Task" /> selected for more info.</param>
        public void DownloadTask(int taskId)
        {
            // Open Download Task View
            Application.Current.Dispatcher.Invoke(() => OnOpenDownloadTaskViewRequested(taskId));
        }

        /// <summary>
        /// Fires when requested a new window with more <see cref="Task" /> details.
        /// </summary>
        public event EventHandler<WindowRequestedEventArgs> OpenTaskDetailsViewRequested;

        /// <summary>
        /// Fires when requesting a download task view.
        /// </summary>
        public event EventHandler<WindowRequestedEventArgs> OpenDownloadTaskViewRequested;

        /// <summary>
        /// Open a new <see cref="Task" /> details window.
        /// </summary>
        /// <param name="taskId">The Id of the <see cref="Task" /> selected for more info.</param>
        public void OpenTaskDetails(int taskId)
        {
            Application.Current.Dispatcher.Invoke(() => OnOpenTaskDetailsViewRequested(taskId));
        }

        private void OnOpenDownloadTaskViewRequested(int taskId)
        {
            Task taskSelected = taskRepository.FindEntityById(taskId);

            EventUtility.SafeFireEvent(OpenDownloadTaskViewRequested, this, new WindowRequestedEventArgs(taskSelected));
        }

        private void OnOpenTaskDetailsViewRequested(int taskId)
        {
            Task taskSelected = taskRepository.FindEntityById(taskId);

            EventUtility.SafeFireEvent(OpenTaskDetailsViewRequested, this, new WindowRequestedEventArgs(taskSelected));
        }
    }
}