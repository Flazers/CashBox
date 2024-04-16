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

        #region UserData

        private int _pin;
        public int Pin
        {
            get => _pin;
            set => Set(ref _pin, value);
        }

        private string _stringPin = string.Empty;
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

        #endregion

        #endregion


        #region Commands

        public RelayCommand CloseApplicationCommand { get; set; }
        private bool CanCloseApplicationCommandExecute(object p) => true;
        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
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

        
        public RelayCommand AuthByPinCommand { get; set; }
        private bool CanAuthByPinCommandExecute(object p) => true;
        private async void OnAuthByPinCommandExecuted(object p)
        {
            UserViewModel? user = await UserViewModel.GetUserByPin(Pin);
            if (user == null) { MessageBox.Show("Пользователь не найден.", "Ошибка"); return; }
            switch (user.UserInfo?.Role.Id)
            {
                case 1:
                    NavigationService?.NavigateTo<AMainViewModel>();
                    break;
                case 2:
                    NavigationService?.NavigateTo<AMainViewModel>();
                    break;
                case 3:
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

        public override void Clear()
        {
            Pin = 0;
            StringPin = string.Empty;
        }

        public AuthViewModel(INavigationService? navService)
        {
            NavigationService = navService;
            NavigateViewCommand = new RelayCommand(OnNavigateViewCommandExecuted, CanNavigateViewCommandExecute);
            ErasePinCommand = new RelayCommand(OnErasePinCommandExecuted, CanErasePinCommandExecute);
            EnterPinCommand = new RelayCommand(OnEnterPinCommandExecuted, CanEnterPinCommandExecute);
            AuthByPinCommand = new RelayCommand(OnAuthByPinCommandExecuted, CanAuthByPinCommandExecute);
            CloseApplicationCommand = new RelayCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
        }
    }
}
