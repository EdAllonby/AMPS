using System;
using System.Windows.Input;

namespace Client.ViewModel.Commands
{
    internal sealed class DownloadTaskCommand : ICommand
    {
        private readonly TaskInformationViewModel viewModel;

        public DownloadTaskCommand(TaskInformationViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            viewModel.DownloadTask((int) parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}