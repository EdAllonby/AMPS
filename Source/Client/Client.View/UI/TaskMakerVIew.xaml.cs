using System;
using Client.ViewModel.SettingsViewModel;
using Shared;
using Shared.Domain;

namespace Client.View.UI
{
    /// <summary>
    /// Interaction logic for TaskMakerView.xaml
    /// </summary>
    public partial class TaskMakerView
    {
        /// <summary>
        /// Create a new Task maker view to create new tasks.
        /// </summary>
        /// <param name="serviceRegistry">The client's <see cref="IServiceRegistry" />.</param>
        /// <param name="band">The <see cref="Band" /> to add a <see cref="Task" /> to.</param>
        public TaskMakerView(IServiceRegistry serviceRegistry, Band band)
        {
            var viewModel = new TaskMakerViewModel(serviceRegistry, band);

            viewModel.CloseTaskMakerViewRequest += OnCloseTaskMakerViewRequest;

            DataContext = viewModel;

            InitializeComponent();
        }

        private void OnCloseTaskMakerViewRequest(object sender, EventArgs e)
        {
            Close();
        }
    }
}