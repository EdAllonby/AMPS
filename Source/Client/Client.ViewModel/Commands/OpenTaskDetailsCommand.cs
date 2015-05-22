using System;
using System.Windows.Input;

namespace Client.ViewModel.Commands
{
    internal sealed class OpenTaskDetailsCommand : ICommand
    {
        private readonly TaskInformationViewModel viewModel;

        public OpenTaskDetailsCommand(TaskInformationViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            viewModel.OpenTaskDetails((int) parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}