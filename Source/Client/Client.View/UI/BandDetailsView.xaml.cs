using Client.ViewModel.SettingsViewModel;
using Shared;
using Shared.Domain;

namespace Client.View.UI
{
    /// <summary>
    /// Interaction logic for BandDetailsView.xaml
    /// </summary>
    public partial class BandDetailsView
    {
        /// <summary>
        /// Creates a new <see cref="Band" /> details view to display information about the specified <see cref="Band" />.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="band">The <see cref="Band" /> to display details for.</param>
        public BandDetailsView(IServiceRegistry serviceRegistry, Band band)
        {
            var viewModel = new BandDetailsViewModel(serviceRegistry, band);
            DataContext = viewModel;

            InitializeComponent();
        }
    }
}