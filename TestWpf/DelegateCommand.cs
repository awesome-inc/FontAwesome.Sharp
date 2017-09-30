using System;
using System.Diagnostics;
using System.Windows.Input;

namespace TestWpf
{
    public class DelegateCommand : ICommand
    {
        private readonly Func<object, bool> _canExecute;
        private readonly Action<object> _doExecute;

        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _canExecute = canExecute ?? (parameter => true);
            _doExecute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public DelegateCommand(Action execute, Func<bool> canExecute = null)
            : this(execute == null ? (Action<object>) (context => { }) : o => execute(),
                canExecute == null ? (Func<object, bool>) (context => true) : o => canExecute())
        {
        }

        #region ICommand

        // cf.: http://stackoverflow.com/questions/2587916/wpf-viewmodel-commands-canexecute-issue
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter)) return;

            _doExecute(parameter);
        }

        #endregion
    }
}