
using Cashbox.Core;
using System.Windows;
using System.Windows.Threading;

namespace Cashbox.Service
{
    public class NavigationService : ViewModelBase, INavigationService
    {
        private readonly Func<Type, ViewModelBase>? _viewModelFactory;
        public NavigationService(Func<Type, ViewModelBase> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        private ViewModelBase? _currentView;
        public ViewModelBase? CurrentView
        {
            get => _currentView;
            private set => Set(ref _currentView, value);
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            ViewModelBase? viewModel = _viewModelFactory?.Invoke(typeof(TViewModel));
            CurrentView = viewModel;
            CurrentView?.Clear();
            Application.Current.Dispatcher.Invoke(new Action(() => { CurrentView?.OnLoad(); }));
        }
    }
}
