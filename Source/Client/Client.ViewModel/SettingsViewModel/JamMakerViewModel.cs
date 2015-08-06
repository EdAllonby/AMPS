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
    /// A View Model for the <see cref="Jam" /> Maker.
    /// </summary>
    public sealed class JamMakerViewModel : TaskInformationViewModel
    {
        private readonly int bandId;
        private bool addAllTasks;
        private DateTime jamEndDate;
        private JamMakerModel jamMakerModel;

        /// <summary>
        /// Fires when requested to close the Jam maker.
        /// </summary>
        public EventHandler RequestJamMakerClose;

        /// <summary>
        /// Creates a new View Model for the <see cref="Jam" /> Maker.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="band">The <see cref="Band" /> to create a <see cref="Jam" /> for.</param>
        public JamMakerViewModel(IServiceRegistry serviceRegistry, Band band) : base(serviceRegistry)
        {
            bandId = band.Id;
            var userRepository = serviceRegistry.GetService<RepositoryManager>().GetRepository<User>();

            List<AddableTaskModel> addableTaskModels = new List<AddableTaskModel>();
            var taskRepository = (TaskRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Task>();

            ParticipationRepository participationRepository = (ParticipationRepository) ServiceRegistry.GetService<RepositoryManager>().GetRepository<Participation>();

            List<User> bandMembers = participationRepository.GetParticipationsByBandId(band.Id).Select(participant => userRepository.FindEntityById(participant.UserId)).ToList();

            foreach (Task backlogTask in taskRepository.GetTasksInBandBacklog(bandId))
            {
                User assignedUser = userRepository.FindEntityById(backlogTask.AssignedUserId);
                AddableTaskModel addableTaskModel = new AddableTaskModel(backlogTask, assignedUser, bandMembers);

                addableTaskModels.Add(addableTaskModel);
            }

            jamMakerModel = new JamMakerModel(addableTaskModels);
        }

        /// <summary>
        /// Models a <see cref="Jam" /> Maker.
        /// </summary>
        public JamMakerModel JamMakerModel
        {
            get { return jamMakerModel; }
            set
            {
                if (Equals(value, jamMakerModel)) return;
                jamMakerModel = value;
                OnPropertyChanged();
            }
        }

        public bool AddAllTasks
        {
            get { return addAllTasks; }
            set
            {
                addAllTasks = value;
                foreach (AddableTaskModel addableTaskModel in JamMakerModel.AddableTasks)
                {
                    addableTaskModel.Add = value;
                }

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Submits a new <see cref="Jam" /> with the Jam Maker's current state.
        /// </summary>
        public ICommand CreateJam => new RelayCommand(SubmitJamRequest, CanSubmitJamRequest);

        private bool CanSubmitJamRequest()
        {
            if (jamMakerModel.JamDayLength <= 0)
            {
                return false;
            }

            foreach (AddableTaskModel addableTask in jamMakerModel.AddableTasks)
            {
                if (addableTask.Add && !DoesAddedTaskHaveAssignedUserAndPoints(addableTask))
                {
                    return false;
                }
            }

            return jamMakerModel.AddableTasks.Any(addableTaskModel => addableTaskModel.Add);
        }

        private static bool DoesAddedTaskHaveAssignedUserAndPoints(TaskModel addableTask)
        {
            return addableTask.AssignedMember != null && addableTask.Points > 0;
        }

        private void SubmitJamRequest()
        {
            IClientService clientService = ServiceRegistry.GetService<IClientService>();

            List<int> taskIdsToAdd = jamMakerModel.AddableTasks.Where(taskModel => taskModel.Add).Select(taskModel => taskModel.TaskId).ToList();

            IReadOnlyEntityRepository<Task> taskRepository = ServiceRegistry.GetService<RepositoryManager>().GetRepository<Task>();

            foreach (AddableTaskModel taskToUpdate in jamMakerModel.AddableTasks.Where(taskModel => taskModel.Add))
            {
                // TODO: Only update Tasks that have changed.

                Task updatedTask = taskRepository.FindEntityById(taskToUpdate.TaskId);

                updatedTask.AssignedUserId = taskToUpdate.AssignedMember.Id;
                updatedTask.Points = taskToUpdate.Points;

                clientService.UpdateTask(updatedTask);
            }

            jamEndDate = DateTime.UtcNow.AddDays(JamMakerModel.JamDayLength);
            clientService.CreateJam(bandId, taskIdsToAdd, jamEndDate);

            EventUtility.SafeFireEvent(RequestJamMakerClose, this);
        }
    }
}