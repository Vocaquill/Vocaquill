using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Vocaquill.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => canExecute == null || canExecute();

        public void Execute(object parameter) => execute();

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
