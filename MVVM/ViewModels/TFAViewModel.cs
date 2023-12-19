using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cashbox.MVVM.ViewModels
{
    public class TFAViewModel : ViewModelBase
    {

        #region Props
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


        #endregion

        #region Commands
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

        #region Navigation
        private INavigationService? _navigationService;
        public INavigationService? NavigationService
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }
        #endregion

        public TFAViewModel(INavigationService? navService)
        {
            NavigationService = navService;
            EnterPinCommand = new RelayCommand(OnEnterPinCommandExecuted, CanEnterPinCommandExecute);
            ErasePinCommand = new RelayCommand(OnErasePinCommandExecuted, CanErasePinCommandExecute);
        }
        #endregion
    }
}
