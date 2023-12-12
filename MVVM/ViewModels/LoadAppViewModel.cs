using Cashbox.Core.Commands;
using Cashbox.MVVM.ViewModels.Base;
using Cashbox.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.ViewModels
{
    public class LoadAppViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        public INavigationService NavigationService
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }

        public LoadAppViewModel(INavigationService navService)
        {
            NavigationService = navService;
            Task.Run(() => { 
                Thread.Sleep(2500);
                NavigationService.NavigateTo<AuthViewModel>();
            });
        }
    }
}
