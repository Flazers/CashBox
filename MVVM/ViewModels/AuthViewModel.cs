using Cashbox.Core.Commands;
using Cashbox.Service;
using System.Windows;
using System.ComponentModel.DataAnnotations;
using Cashbox.Core;
using Cashbox.MVVM.Models;
using Cashbox.MVVM.ViewModels.Data;
using System.Security;
using Cashbox.MVVM.ViewModels.Admin;
using Cashbox.MVVM.ViewModels.Employee;

namespace Cashbox.MVVM.ViewModels
{
    public class AuthViewModel : ViewModelBase
    {
        #region Props

        #region SwipeAuthMethod
        private Visibility _authMethodPinVisibility = Visibility.Visible;
        public Visibility AuthMethodPinVisibility
        {
            get => _authMethodPinVisibility;
            set => Set(ref _authMethodPinVisibility, value);
        }

        private Visibility _authMethodLPVisibility = Visibility.Collapsed;
        public Visibility AuthMethodLPVisibility
        {
            get => _authMethodLPVisibility;
            set => Set(ref _authMethodLPVisibility, value);
        }

        private bool _authMethodPinEnabled = true;
        public bool IsAuthMethodPinEnabled
        {
            get => _authMethodPinEnabled;
            set => Set(ref _authMethodPinEnabled, value);
        }

        private bool _authMethodLPEnabled = false;
        public bool IsAuthMethodLPEnabled
        {
            get => _authMethodLPEnabled;
            set => Set(ref _authMethodLPEnabled, value);
        }

        #endregion

        #region UserData

        private string _login = string.Empty;
        [Required(AllowEmptyStrings = false)]
        public string Login
        {
            get => _login;
            set => Set(ref _login, value);
        }

        private string _password = string.Empty;
        [Required(AllowEmptyStrings = false)]
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        private int _pin;
        [Required(AllowEmptyStrings = false)]
        public int Pin
        {
            get => _pin;
            set => Set(ref _pin, value);
        }

        private string _stringPin = string.Empty;
        [Required(AllowEmptyStrings = false)]
        public string StringPin
        {
            get => _stringPin;
            set
            {
                _stringPin = value;
                if (StringPin.Length != 0)
                    Pin = int.Parse(value);
                OnPropertyChanged();
            }
        }

        private bool _tfa = false;
        [Required(AllowEmptyStrings = false)]
        public bool TFA
        {
            get => _tfa;
            set => Set(ref _tfa, value);
        }

        #endregion

        #endregion


        #region Commands

        public RelayCommand SwipeAuthMethodVisibilityCommand { get; set; }
        private bool CanSwipeAuthMethodVisibilityCommandExecute(object p) => true;
        private void OnSwipeAuthMethodVisibilityCommandExecuted(object p)
        {
            switch (IsAuthMethodPinEnabled)
            {
                case true:
                    AuthMethodPinVisibility = Visibility.Visible;
                    AuthMethodLPVisibility = Visibility.Collapsed;
                    break;
                case false:
                    AuthMethodPinVisibility = Visibility.Collapsed;
                    AuthMethodLPVisibility = Visibility.Visible;
                    break;
            }
        }

        public RelayCommand EnterPinCommand { get; set; }
        private bool CanEnterPinCommandExecute(object p) 
        {
            if (StringPin.Length == 6)
                return false;
            if (StringPin.Length == 0 && (string)p == "0")
                return false;
            return true;
        }
        private void OnEnterPinCommandExecuted(object p)
        {
            StringPin += (string)p;
        }

        public RelayCommand ErasePinCommand { get; set; }
        private bool CanErasePinCommandExecute(object p)
        {
            if (StringPin.Length > 0)
                return true;
            return false;
        }
        private void OnErasePinCommandExecuted(object p)
        {
            StringPin = StringPin.Remove(StringPin.Length - 1);
        }

        public RelayCommand AuthByLogPassCommand { get; set; }
        private bool CanAuthByLogPassCommandExecute(object p) => true;
        private async void OnAuthByLogPassCommandExecuted(object p)
        {
            UserViewModel? user = await UserViewModel.GetUserByLogPass(Login, Password);
            if (user == null) { MessageBox.Show("lox"); return; }
            switch (user.UserInfo?.Role.Id)
            {
                case 1:
                    NavigationService?.NavigateTo<AMainViewModel>();
                    break;
                case 2:
                    NavigationService?.NavigateTo<EMainViewModel>();
                    break;
            }
        }

        public RelayCommand AuthByPinCommand { get; set; }
        private bool CanAuthByPinCommandExecute(object p) => true;
        private async void OnAuthByPinCommandExecuted(object p)
        {
            UserViewModel? user = await UserViewModel.GetUserByPin(Pin);
            if (user == null) { MessageBox.Show("lox"); return; }
            switch (user.UserInfo?.Role.Id)
            {
                case 1:
                    NavigationService?.NavigateTo<AMainViewModel>();
                    break;
                case 2:
                    NavigationService?.NavigateTo<EMainViewModel>();
                    break;
            }
        }

        #endregion


        #region Navigation
        private INavigationService? _navigationService;
        public INavigationService? NavigationService
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }
        public RelayCommand NavigateViewCommand { get; set; }
        private bool CanNavigateViewCommandExecute(object p) => true;
        private void OnNavigateViewCommandExecuted(object p)
        {
            NavigationService?.NavigateTo<LoadingViewModel>();
        }
        #endregion

        public AuthViewModel(INavigationService? navService)
        {
            NavigationService = navService;
            NavigateViewCommand = new RelayCommand(OnNavigateViewCommandExecuted, CanNavigateViewCommandExecute);
            ErasePinCommand = new RelayCommand(OnErasePinCommandExecuted, CanErasePinCommandExecute);
            SwipeAuthMethodVisibilityCommand = new RelayCommand(OnSwipeAuthMethodVisibilityCommandExecuted, CanSwipeAuthMethodVisibilityCommandExecute);
            EnterPinCommand = new RelayCommand(OnEnterPinCommandExecuted, CanEnterPinCommandExecute);
            AuthByLogPassCommand = new RelayCommand(OnAuthByLogPassCommandExecuted, CanAuthByLogPassCommandExecute);
            AuthByPinCommand = new RelayCommand(OnAuthByPinCommandExecuted, CanAuthByPinCommandExecute);
        }
    }
}
