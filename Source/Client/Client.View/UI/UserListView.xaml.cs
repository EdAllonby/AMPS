using Client.ViewModel.MainViewModel;
using Shared;
using Shared.Domain;

namespace Client.View.UI
{
    /// <summary>
    /// Interaction logic for UserListView.xaml
    /// </summary>
    public partial class UserListView
    {
        /// <summary>
        /// Create a list of connected <see cref="User" />s view.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="bandId">The <see cref="Band"/> context.</param>
        public UserListView(IServiceRegistry serviceRegistry, int bandId)
        {
            InitializeComponent();
            DataContext = new UserListViewModel(serviceRegistry, bandId);
        }
    }
}