using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.Service;
using Cashbox.MVVM.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Extensions.DependencyInjection;

namespace Cashbox.MVVM.ViewModels
{
    public class AuthViewModel : ViewModelBase
    {
        //private INavigationService _navigationService;
        //public INavigationService NavigationService
        //{
        //    get => _navigationService;
        //    set => Set(ref _navigationService, value);
        //}

        //public RelayCommand NavigateMainPageCommand { get; set; }
        //private bool CanNavigateMainPageCommandExecute(object p) => true;
        //private void OnNavigateMainPageCommandExecuted(object p)
        //{
        //    NavigationService.NavigateTo<MainPageViewModel>();
        //}

        //public AuthPageViewModel(INavigationService navService)
        //{
        //    NavigationService = navService;
        //    NavigateMainPageCommand = new RelayCommand(OnNavigateMainPageCommandExecuted, CanNavigateMainPageCommandExecute);
        //}

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

        #region SHpassword
        #endregion

        #region UserData
        private string _login;
        public string Login
        {
            get => _login;
            set => Set(ref _login, value);
        }

        private string _password;
        public string Password
        {
            private get => _password;
            set => Set(ref _password, value);
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

        #endregion

        private INavigationService _navigationService;
        public INavigationService NavigationService
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }

        public RelayCommand NavigateMainPageCommand { get; set; }
        private bool CanNavigateMainPageCommandExecute(object p) => true;
        private void OnNavigateMainPageCommandExecuted(object p)
        {
            NavigationService.NavigateTo<LoadingViewModel>();
        }

#if DEBUG
        public AuthViewModel() {}
#endif
        public AuthViewModel(INavigationService navService)
        {
            NavigationService = navService;
            NavigateMainPageCommand = new RelayCommand(OnNavigateMainPageCommandExecuted, CanNavigateMainPageCommandExecute);
            SwipeAuthMethodVisibilityCommand = new RelayCommand(OnSwipeAuthMethodVisibilityCommandExecuted, CanSwipeAuthMethodVisibilityCommandExecute);
        }
    }
}
