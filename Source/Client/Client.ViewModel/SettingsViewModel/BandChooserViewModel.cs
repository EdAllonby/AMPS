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
    /// Holds the logic for the view. Accesses the Service manager to receive and send messages. 
    /// </summary>
    public class BandChooserViewModel : ViewModel
    {
        private readonly BandRepository bandRepository;
        private readonly int clientUserId;
        private readonly ParticipationRepository participationRepository;
        private BandChooserModel bandChooserModel;

        /// <summary>
        /// Creates a new view model for choosing a band to enter.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry"/>.</param>
        public BandChooserViewModel(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
            clientUserId = serviceRegistry.GetService<IClientService>().ClientUserId;

            bandRepository = (BandRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Band>();
            participationRepository = (ParticipationRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Participation>();

            bandRepository.EntityAdded += OnBandsChanged;
            bandRepository.EntityUpdated += OnBandsChanged;

            participationRepository.EntityAdded += OnParticipationsChanged;
            participationRepository.EntityUpdated += OnParticipationsChanged;

            BandChooserModel = new BandChooserModel(GetBandsParticipatingIn());
        }

        /// <summary>
        /// The underlying model.
        /// </summary>
        public BandChooserModel BandChooserModel
        {
            get { return bandChooserModel; }
            set
            {
                if (Equals(value, bandChooserModel)) return;
                bandChooserModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Try  to open band management screen.
        /// </summary>
        public ICommand OpenBand
        {
            get { return new RelayCommand(OpenNewBand, CanOpenBand); }
        }

        /// <summary>
        /// Fires when requesting to open the main band management view.
        /// </summary>
        public event EventHandler<WindowRequestedEventArgs> OpenMainViewRequested;

        private void OnParticipationsChanged(object sender, EntityChangedEventArgs<Participation> e)
        {
            BandChooserModel = new BandChooserModel(GetBandsParticipatingIn());
        }

        private void OnBandsChanged(object sender, EntityChangedEventArgs<Band> e)
        {
            BandChooserModel = new BandChooserModel(GetBandsParticipatingIn());
        }

        private IEnumerable<Band> GetBandsParticipatingIn()
        {
            var bandIdsUserIsIn = participationRepository.GetAllBandIdsByUserId(clientUserId);

            return bandIdsUserIsIn.Select(bandId => bandRepository.FindEntityById(bandId));
        }

        private void OpenNewBand()
        {
            if (BandChooserModel.SelectedBand != null)
            {
                Band band = ServiceRegistry.GetService<RepositoryManager>().GetRepository<Band>().FindEntityById(BandChooserModel.SelectedBand.Id);
                EventUtility.SafeFireEvent(OpenMainViewRequested, this, new WindowRequestedEventArgs(band));
            }
        }

        private bool CanOpenBand()
        {
            return bandChooserModel.SelectedBand != null;
        }
    }
}