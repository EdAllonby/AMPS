using System.Collections.Generic;
using System.Linq;
using Client.Model.SettingsModel;
using Shared;
using Shared.Domain;
using Shared.Repository;

namespace Client.ViewModel.SettingsViewModel
{
    /// <summary>
    /// Holds the logic for the view. Accesses the Service manager to receive and send messages. 
    /// </summary>
    public sealed class BandDetailsViewModel : ViewModel
    {
        private BandDetailsModel bandDetailsModel;

        /// <summary>
        /// Creates a new view model for band details.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry"/>.</param>
        /// <param name="band">The context.</param>
        public BandDetailsViewModel(IServiceRegistry serviceRegistry, Band band)
            : base(serviceRegistry)
        {
            ParticipationRepository participationRepository = (ParticipationRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Participation>();

            List<Participation> bandParticipants = participationRepository.GetParticipationsByBandId(band.Id);

            IReadOnlyEntityRepository<User> userRepository = serviceRegistry.GetService<RepositoryManager>().GetRepository<User>();

            User bandLeader = bandParticipants.Where(bandParticipant => bandParticipant.IsLeader).Select(bandParticipant => userRepository.FindEntityById(bandParticipant.UserId)).FirstOrDefault();

            var bandMembers = bandParticipants.Select(bandParticipant => userRepository.FindEntityById(bandParticipant.UserId));

            if (bandLeader != null)
            {
                bandDetailsModel = new BandDetailsModel(bandMembers.ToList(), bandLeader, band);
            }
            else
            {
                Log.ErrorFormat("Leader not found for Band with Id {0}", band.Id);
            }
        }

        /// <summary>
        /// The underlying model.
        /// </summary>
        public BandDetailsModel BandMakerModel
        {
            get { return bandDetailsModel; }
            set
            {
                if (Equals(value, bandDetailsModel)) return;
                bandDetailsModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The leader of the <see cref="Band" />.
        /// </summary>
        public string DisplayBandLeader
        {
            get { return string.Format("Leader is {0}", bandDetailsModel.BandLeader.Username); }
        }

        /// <summary>
        /// The name of the <see cref="Band" />.
        /// </summary>
        public string DisplayBandName
        {
            get { return string.Format("Band name is: {0}", BandMakerModel.Band.Name); }
        }
    }
}