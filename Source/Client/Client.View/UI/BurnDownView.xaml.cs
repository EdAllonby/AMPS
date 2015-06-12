using System.Windows;
using Client.ViewModel.SettingsViewModel;
using Shared;
using Shared.Domain;

namespace Client.View.UI
{
    /// <summary>
    /// Interaction logic for BurnDownView.xaml
    /// </summary>
    public partial class BurnDownView
    {
        public BurnDownView(IServiceRegistry serviceRegistry, Band band)
        {
            InitializeComponent();
            var viewModel = new BurnDownViewModel(band, serviceRegistry);
            DataContext = viewModel;

        }
    }
}
