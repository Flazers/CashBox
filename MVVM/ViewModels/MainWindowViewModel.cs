using Cashbox.Core;
using Cashbox.Service;

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
            NavigationService?.NavigateTo<LoadingViewModel>();
        }
    }
}
