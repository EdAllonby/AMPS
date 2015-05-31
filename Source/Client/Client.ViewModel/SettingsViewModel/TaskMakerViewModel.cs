using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Client.Model.SettingsModel;
using Client.Service;
using Client.ViewModel.Commands;
using Shared;
using Shared.Domain;
using Shared.Repository;
using Utility;

namespace Client.ViewModel.SettingsViewModel
{
    /// <summary>
    /// Displays the <see cref="Task" /> Maker.
    /// </summary>
    public sealed class TaskMakerViewModel : ViewModel
    {
        private readonly int bandId;
        private readonly IClientService clientService;
        private TaskMakerModel taskMakerMakerModel;

        /// <summary>
        /// Creates a new <see cref="Task" /> Maker View Model.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="bandId">The <see cref="Band" /> Id in which the <see cref="Task" /> will be created for.</param>
        public TaskMakerViewModel(IServiceRegistry serviceRegistry, int bandId) : base(serviceRegistry)
        {
            this.bandId = bandId;

            IReadOnlyEntityRepository<User> userRepository = serviceRegistry.GetService<RepositoryManager>().GetRepository<User>();
            var participationRepository = (ParticipationRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Participation>();
            clientService = serviceRegistry.GetService<IClientService>();

            List<Participation> bandParticipants = participationRepository.GetParticipationsByBandId(bandId);

            List<User> bandMembers = bandParticipants.Select(bandParticipant => userRepository.FindEntityById(bandParticipant.UserId)).ToList();

            TaskMakerModel = new TaskMakerModel(bandMembers);
        }

        /// <summary>
        /// The underlying model.
        /// </summary>
        public TaskMakerModel TaskMakerModel
        {
            get { return taskMakerMakerModel; }
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
        public ICommand ApplyCreateNewTaskAndClose
        {
            get { return new RelayCommand(SendTaskRequestAndClose, CanCreateNewTask); }
        }

        /// <summary>
        /// Cancels creating a new <see cref="Task" /> for the backlog.
        /// </summary>
        public ICommand CancelCreateNewTask
        {
            get { return new RelayCommand(OnCloseTaskMakerViewRequest); }
        }

        /// <summary>
        /// Fires when the <see cref="Task" /> maker view model requests a close.
        /// </summary>
        public event EventHandler CloseTaskMakerViewRequest;

        private void SendTaskRequestAndClose()
        {
            var assignedMemberId = TaskMakerModel.AssignedMember != null ? TaskMakerModel.AssignedMember.Id : 0;

            clientService.AddTaskToBacklog(bandId, TaskMakerModel.TaskTitle, TaskMakerModel.TaskDescription, TaskMakerModel.TaskPoints, assignedMemberId, TaskMakerModel.TaskCategory);

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