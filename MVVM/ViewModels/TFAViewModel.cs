using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.Models;
using Cashbox.MVVM.ViewModels.Data;
using Cashbox.Service;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        private BitmapImage? _QR = null;
        public BitmapImage? QR
        {
            get => _QR;
            set
            {
                _QR = value;
                OnPropertyChanged();
            }
        }

        private Visibility _bindTFAVisibility = Visibility.Visible;
        public Visibility BindTFAVisibility
        {
            get => _bindTFAVisibility;
            set => Set(ref _bindTFAVisibility, value);
        }

        private Visibility _getTFAVisibility = Visibility.Collapsed;
        public Visibility GetTFAVisibility
        {
            get => _getTFAVisibility;
            set => Set(ref _getTFAVisibility, value);
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

        public RelayCommand SendNewTFACommand { get; set; }
        private bool CanSendNewTFACommandExecute(object p)
        {
            if (StringPin.Length > 0)
                return true;
            return false;
        }
        private void OnSendNewTFACommandExecuted(object p)
        {
            
        }

        public RelayCommand CheckTFACommand { get; set; }
        private bool CanCheckTFACommandExecute(object p)
        {
            if (StringPin.Length > 0)
                return true;
            return false;
        }
        private void OnCheckTFACommandExecuted(object p)
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


        #endregion
        private async Task<string> GetAsyncPathImg()
        {
            try
            {
                HttpResponseMessage response = await new HttpClient()
                    .GetAsync($"https://www.authenticatorApi.com/pair.aspx?AppName=CashBox&AppInfo={UserViewModel.GetCurrentUser()}&SecretCode=CashBoxTFAGenerationCode");
                if (response.IsSuccessStatusCode)
                {
                    MatchCollection ImgPath = new Regex(@"src='.*?'").Matches(await response.Content.ReadAsStringAsync());
                    string ShortVal = ImgPath[0].Value.Remove(0, 5);
                    return ShortVal.Remove(ShortVal.Length - 1, 1);
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void GetQRCode() => QR = new BitmapImage(new Uri(await GetAsyncPathImg(), UriKind.Absolute));

        public TFAViewModel(INavigationService? navService)
        {
            NavigationService = navService;
            GetQRCode();
            EnterPinCommand = new RelayCommand(OnEnterPinCommandExecuted, CanEnterPinCommandExecute);
            ErasePinCommand = new RelayCommand(OnErasePinCommandExecuted, CanErasePinCommandExecute);
            SendNewTFACommand = new RelayCommand(OnSendNewTFACommandExecuted, CanSendNewTFACommandExecute);
            CheckTFACommand = new RelayCommand(OnCheckTFACommandExecuted, CanCheckTFACommandExecute);
        }
    }
}
