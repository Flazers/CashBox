using Cashbox.Core;

namespace Cashbox.Service
{
    public interface INavigationService
    {
        ViewModelBase? CurrentView { get; }
        void NavigateTo<T>() where T : ViewModelBase;
    }

}