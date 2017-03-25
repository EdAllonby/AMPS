using Client.Model.SettingsModel;
using Shared;
using Shared.Domain;

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
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="band">The context.</param>
        public BandDetailsViewModel(IServiceRegistry serviceRegistry, Band band)
            : base(serviceRegistry)
        {
            bandDetailsModel = new BandDetailsModel(band);
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
        public string DisplayBandLeader => $"Leader is {bandDetailsModel.BandLeader.Username}";

        /// <summary>
        /// The name of the <see cref="Band" />.
        /// </summary>
        public string DisplayBandName => $"Band name is: {BandMakerModel.Band.Name}";
    }
}