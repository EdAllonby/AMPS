using System;
using Client.ViewModel.SettingsViewModel;
using Shared;

namespace Client.View.UI
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView
    {
        public SettingsView(IServiceRegistry serviceRegistry)
        {
            InitializeComponent();

            DataContext = new SettingsViewModel(serviceRegistry);

            var viewModel = (SettingsViewModel) DataContext;

            viewModel.Close += CloseRequested;
        }

        private void CloseRequested(object sender, EventArgs e)
        {
            Close();
        }
    }
}