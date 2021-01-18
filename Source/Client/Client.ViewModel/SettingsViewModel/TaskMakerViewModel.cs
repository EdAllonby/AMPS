using System;
using System.Linq;
using System.Windows.Input;
using Client.Model.SettingsModel;
using Client.Service;
using Client.ViewModel.Commands;
using Shared;
using Shared.Domain;
using Utility;

namespace Client.ViewModel.SettingsViewModel
{
    /// <summary>
    /// Displays the <see cref="Task" /> Maker.
    /// </summary>
    public sealed class TaskMakerViewModel : ViewModel
    {
        private readonly Band band;
        private readonly IClientService clientService;
        private TaskMakerModel taskMakerMakerModel;

        /// <summary>
        /// Creates a new <see cref="Task" /> Maker View Model.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="band">The <see cref="Band" /> in which the <see cref="Task" /> will be created for.</param>
        public TaskMakerViewModel(IServiceRegistry serviceRegistry, Band band) : base(serviceRegistry)
        {
            this.band = band;

            clientService = serviceRegistry.GetService<IClientService>();

            TaskMakerModel = new TaskMakerModel(band.Members.ToList());
        }

        /// <summary>
        /// The underlying model.
        /// </summary>
        public TaskMakerModel TaskMakerModel
        {
            get => taskMakerMakerModel;
            set
            {
                if (Equals(value, taskMakerMakerModel)) return;
                taskMakerMakerModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Create a new <see cref="Task" /> for the <see cref="Band" /> backlog.
        /// </summary>
        public ICommand ApplyCreateNewTaskAndClose => new RelayCommand(SendTaskRequestAndClose, CanCreateNewTask);

        /// <summary>
        /// Cancels creating a new <see cref="Task" /> for the backlog.
        /// </summary>
        public ICommand CancelCreateNewTask => new RelayCommand(OnCloseTaskMakerViewRequest);

        /// <summary>
        /// Fires when the <see cref="Task" /> maker view model requests a close.
        /// </summary>
        public event EventHandler CloseTaskMakerViewRequest;

        private void SendTaskRequestAndClose()
        {
            int assignedMemberId = TaskMakerModel.AssignedMember?.Id ?? 0;

            clientService.AddTaskToBacklog(band.Id, TaskMakerModel.TaskTitle, TaskMakerModel.TaskDescription, TaskMakerModel.TaskPoints, assignedMemberId, TaskMakerModel.TaskCategory);

            OnCloseTaskMakerViewRequest();
        }

        private bool CanCreateNewTask()
        {
            return !string.IsNullOrWhiteSpace(TaskMakerModel.TaskTitle)
                   && !string.IsNullOrWhiteSpace(TaskMakerModel.TaskDescription);
        }

        private void OnCloseTaskMakerViewRequest()
        {
            EventUtility.SafeFireEvent(CloseTaskMakerViewRequest, this);
        }
    }
}