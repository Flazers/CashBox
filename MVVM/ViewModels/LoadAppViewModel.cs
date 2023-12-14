using Cashbox.Core;
using Cashbox.Service;


namespace Cashbox.MVVM.ViewModels
{
    public class LoadAppViewModel : ViewModelBase
    {
        private INavigationService? _navigationService;
        public INavigationService? NavigationService
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }

        public LoadAppViewModel(INavigationService navService)
        {
            NavigationService = navService;
            Task.Run(() => { 
                Thread.Sleep(1500);
                NavigationService.NavigateTo<AuthViewModel>();
            });
        }
    }
}
