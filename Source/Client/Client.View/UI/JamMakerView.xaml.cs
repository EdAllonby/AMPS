using System;
using Client.ViewModel;
using Client.ViewModel.SettingsViewModel;
using Shared;
using Shared.Domain;

namespace Client.View.UI
{
    /// <summary>
    /// Interaction logic for JamMakerView.xaml
    /// </summary>
    public partial class JamMakerView
    {
        private readonly IServiceRegistry serviceRegistry;

        /// <summary>
        /// Create a new view for creating <see cref="Jam" />s and initialises its View Model.
        /// </summary>
        public JamMakerView(IServiceRegistry serviceRegistry, Band band)
        {
            this.serviceRegistry = serviceRegistry;

            var viewModel = new JamMakerViewModel(serviceRegistry, band);

            viewModel.OpenTaskDetailsViewRequested += OnOpenTaskBacklogViewRequested;
            viewModel.RequestJamMakerClose += OnCloseJamMakerRequested;
            DataContext = viewModel;

            InitializeComponent();
        }

        private void OnCloseJamMakerRequested(object sender, EventArgs e)
        {
            Close();
        }

        private void OnOpenTaskBacklogViewRequested(object sender, WindowRequestedEventArgs windowRequestedEventArgs)
        {
            var taskDetailsView = new TaskDetailsView(serviceRegistry, windowRequestedEventArgs.Task);

            taskDetailsView.ShowDialog();
        }
    }
}