using System;
using System.Windows.Input;

namespace ExpensePrediction.Frontend.ViewModel
{
    public class CommandHandler : ICommand
    {
        public delegate void CommandOnExecute(object parameter);

        public delegate bool CommandOnCanExecute(object parameter);

        private readonly CommandOnExecute _execute;
        private readonly CommandOnCanExecute _canExecute;

        public CommandHandler(CommandOnExecute onExecute, CommandOnCanExecute canExecute)
        {
            _execute = onExecute;
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            return _canExecute.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            _execute.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}