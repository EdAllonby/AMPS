﻿using System;
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
            var addableTaskModels = new List<AddableTaskModel>();

            IEnumerable<User> bandMembers = band.Members;

            foreach (Task backlogTask in band.Backlog)
            {
                var addableTaskModel = new AddableTaskModel(backlogTask, bandMembers.ToList());

                addableTaskModels.Add(addableTaskModel);
            }

            jamMakerModel = new JamMakerModel(addableTaskModels);
        }

        /// <summary>
        /// Models a <see cref="Jam" /> Maker.
        /// </summary>
        public JamMakerModel JamMakerModel
        {
            get => jamMakerModel;
            set
            {
                if (Equals(value, jamMakerModel)) return;
                jamMakerModel = value;
                OnPropertyChanged();
            }
        }

        public bool AddAllTasks
        {
            get => addAllTasks;
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
            var clientService = ServiceRegistry.GetService<IClientService>();

            List<int> taskIdsToAdd = jamMakerModel.AddableTasks.Where(taskModel => taskModel.Add).Select(taskModel => taskModel.TaskId).ToList();

            IReadOnlyEntityRepository<Task> taskRepository = ServiceRegistry.GetService<IRepositoryManager>().GetRepository<Task>();

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