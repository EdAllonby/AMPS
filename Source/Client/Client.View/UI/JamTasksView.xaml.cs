using Client.ViewModel;
using Client.ViewModel.MainViewModel;
using Shared;
using Shared.Domain;

namespace Client.View.UI
{
    /// <summary>
    /// Interaction logic for JamTasksView.xaml
    /// </summary>
    public partial class JamTasksView
    {
        private readonly IServiceRegistry serviceRegistry;

        /// <summary>
        /// Creates a new <see cref="Jam" /> <see cref="Task" />s view.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="band">The <see cref="Band" /> context.</param>
        public JamTasksView(IServiceRegistry serviceRegistry, Band band)
        {
            this.serviceRegistry = serviceRegistry;

            InitializeComponent();

            var viewModel = new JamTasksViewModel(serviceRegistry, band);

            viewModel.OpenAdminViewRequested += OnOpenAdminViewRequested;

            DataContext = viewModel;
        }

        private void OnOpenAdminViewRequested(object sender, WindowRequestedEventArgs e)
        {
            var adminView = new AdminView(serviceRegistry, e.Band);
            adminView.ShowDialog();
        }
    }
}