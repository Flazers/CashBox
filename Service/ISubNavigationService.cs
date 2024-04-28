using Cashbox.Core;

namespace Cashbox.Service
{
    public interface ISubNavigationService
    {
        ViewModelBase? CurrentView { get; }
        void NavigateTo<T>() where T : ViewModelBase;
    }
}
