using Cashbox.Core;
using Cashbox.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private INavigationService? _navigationService;
        public INavigationService? NavigationService
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }

        public MainWindowViewModel(INavigationService? navService)
        {
            NavigationService = navService;
            NavigationService?.NavigateTo<LoadAppViewModel>();
        }
    }
}
