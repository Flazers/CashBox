using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cashbox.Core.Commands.Base
{
    public abstract class Command : ICommand
    {
#pragma warning disable CS8612
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
#pragma warning restore CS8612

#pragma warning disable CS8767
        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);
#pragma warning restore CS8767
    }
}
