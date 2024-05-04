using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.ViewModels.Data;
using Cashbox.Service;
using System.Windows;

namespace Cashbox.MVVM.ViewModels.Admin
{
    public class AMainViewModel : ViewModelBase
    {

        #region Props
        public static UserViewModel? User { get => Models.User.CurrentUser; }

        #region isBoolView
        private bool _isHomeView = true;
        public bool IsHomeView
        {
            get => _isHomeView;
            set => Set(ref _isHomeView, value);
        }

        private bool _isEmployeeView = false;
        public bool IsEmployeeView
        {
            get => _isEmployeeView;
            set => Set(ref _isEmployeeView, value);
        }

        private bool _isStockView = false;
        public bool IsStockView
        {
            get => _isStockView;
            set => Set(ref _isStockView, value);
        }

        private bool _isShiftView = false;
        public bool IsShiftView
        {
            get => _isShiftView;
            set => Set(ref _isShiftView, value);
        }

        private bool _isLogView = false;
        public bool IsLogView
        {
            get => _isLogView;
            set => Set(ref _isLogView, value);
        }
        #endregion

        #endregion

        #region Commands

        public RelayCommand NavigateSubViewCommand { get; set; }
        private bool CanNavigateSubViewCommandExecute(object p) => true;
        private void OnNavigateSubViewCommandExecuted(object p)
        {
            if (IsHomeView)
                SubNavigationService?.NavigateTo<HomeViewModel>();
            if (IsEmployeeView)
                SubNavigationService?.NavigateTo<EmployeesViewModel>();
            if (IsStockView)
                SubNavigationService?.NavigateTo<StockViewModel>();
            if (IsShiftView)
                SubNavigationService?.NavigateTo<ShiftViewModel>();
            if (IsLogView)
                SubNavigationService?.NavigateTo<LogViewModel>();
        }

        public RelayCommand LogOutCommand { get; set; }
        private bool CanLogOutCommandExecute(object p) => true;
        private void OnLogOutCommandExecuted(object p)
        {
            if (AppCommand.QuestionMessage("Вы уверены, что хотите выйти?") == MessageBoxResult.No) return;
            UserViewModel.LogOut();
            NavigationService?.NavigateTo<AuthViewModel>();
        }

        #endregion

        #region Navigation

        public override void Clear()
        {
            IsHomeView = true;
            IsEmployeeView = false;
            IsStockView = false;
            IsShiftView = false;
            NavigateSubViewCommand.Execute(this);
        }

        private ISubNavigationService? _subNavigationService;
        public ISubNavigationService? SubNavigationService
        {
            get => _subNavigationService;
            set => Set(ref _subNavigationService, value);
        }

        private INavigationService? _NavigationService;
        public INavigationService? NavigationService
        {
            get => _NavigationService;
            set => Set(ref _NavigationService, value);
        }

        #endregion

        public AMainViewModel(ISubNavigationService subNavService, INavigationService navService)
        {
            SubNavigationService = subNavService;
            NavigationService = navService;
            NavigateSubViewCommand = new RelayCommand(OnNavigateSubViewCommandExecuted, CanNavigateSubViewCommandExecute);
            NavigateSubViewCommand.Execute(this);
            LogOutCommand = new RelayCommand(OnLogOutCommandExecuted, CanLogOutCommandExecute);
        }

    }
}
