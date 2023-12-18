using Cashbox.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.Service
{
    public interface ISubNavigationService
    {
        ViewModelBase? CurrentView { get; }
        void NavigateTo<T>() where T : ViewModelBase;

    }
}
