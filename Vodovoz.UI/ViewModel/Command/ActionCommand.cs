using System;
using System.Windows.Input;

namespace Vodovoz.UI.ViewModel.Command {
    public class ActionCommand : ICommand {
        private readonly Action<object> _action;
        private readonly Func<object, bool> _canExecute = p => true;

        public ActionCommand(Action<object> action, Func<object, bool> canExecute = null) {
            _action = action;
            if (canExecute != null) _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute(parameter);

        public void Execute(object parameter) => _action(parameter);

        public event EventHandler CanExecuteChanged;
    }
}
