using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.ViewModels.Admin;
using Cashbox.MVVM.ViewModels.Data;
using Cashbox.MVVM.ViewModels.Employee;
using Cashbox.Service;
using System.Windows;

namespace Cashbox.MVVM.ViewModels
{
    public class AuthViewModel : ViewModelBase
    {
        #region Props

        private int countEnter = 0;

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
            if (user == null) 
            { 
                AppCommand.WarningMessage("Пользователь не найден.");
                countEnter++;
                return; 
            }
            if (countEnter >= 2)
                if (user.UserInfo.RoleId == 1)
                {
                    AppCommand.WarningMessage("Пользователь не найден.");
                    await AdminMoneyLogViewModel.CreateTransitMB($"Попытка зайти в аккаунт администратора ({user.UserInfo.FullName}) {DateTime.Now.ToString("dd.MM.yyyy HH.mm.ss")}", 0);
                    return;
                }
            List<DailyReportViewModel> list = await DailyReportViewModel.GetNotCloseReports();

            if (list.Count != 0)
            {
                string ListNotClose = string.Empty;
                int[] SuccessEnterId = [];

                static async void closereport(DailyReportViewModel report)
                {
                    double DayOrdersProccesedSum;
                    List<OrderViewModel> DayOrdersProccesed = await OrderViewModel.GetDayOrdersToMethod((DateOnly)report.Data!, 2);
                    if (DayOrdersProccesed.Count > 0)
                        DayOrdersProccesedSum = (double)DayOrdersProccesed.Sum(x => x.SellCost)! + report.CashOnStart;
                    else
                        DayOrdersProccesedSum = report.CashOnStart;
                    DailyReportViewModel drvm = await DailyReportViewModel.EndShift((DateOnly)report.Data!, new(23, 59, 59), DayOrdersProccesedSum, report.UserId);
                    await AutoDailyReportViewModel.GenEndShiftAuto(drvm!);
                }

                foreach (DailyReportViewModel report in list)
                    ListNotClose += $"{report.UserInfoVM.FullName} ";
                foreach (DailyReportViewModel report in list)
                {
                    if (report.UserId == user.Id)
                    {
                        if (report.Data == DateOnly.FromDateTime(DateTime.Today))
                            break;
                        if (AppCommand.QuestionMessage($"У вас не закрыта предыдущая смена.\nЗакрыть для продолжения работы?") == MessageBoxResult.Yes)
                        {
                            closereport(report);
                            break;
                        }
                    }
                    if (user.UserInfo.RoleId == 2)
                    {
                        AppCommand.WarningMessage($"Открыта смена у {ListNotClose}");
                        return;
                    }
                    if (AppCommand.QuestionMessage($"Открыта смена у {ListNotClose}\nЗакрыть для продолжения работы?") == MessageBoxResult.Yes)
                        closereport(report);
                    else
                        return;
                }
            }
            if (!await OrderViewModel.RemoveNullReferenceOrder())
                return;
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

        public override void OnLoad()
        {
            Pin = 0;
            StringPin = string.Empty;
            countEnter = 0;
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
