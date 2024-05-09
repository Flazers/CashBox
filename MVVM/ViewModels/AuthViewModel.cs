using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.ViewModels.Admin;
using Cashbox.MVVM.ViewModels.Data;
using Cashbox.MVVM.ViewModels.Employee;
using Cashbox.Service;
using System.Security;
using System.Windows;

namespace Cashbox.MVVM.ViewModels
{
    public class AuthViewModel : ViewModelBase
    {
        #region Props

        private int countEnter = 0;

        #region UserData

        private string _securePassword = string.Empty;
        public string SecurePassword 
        { 
            private get => _securePassword;
            set
            {
                _securePassword = value;
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

        public RelayCommand AuthByPinCommand { get; set; }
        private bool CanAuthByPinCommandExecute(object p) => true;
        private async void OnAuthByPinCommandExecuted(object p)
        {
            int Pin = 0;
            if (!int.TryParse(SecurePassword, out Pin))
                return;
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
                }
            }
            if (!await OrderViewModel.RemoveNullReferenceOrder())
                return;            
            if (!await RefundViewModel.RemoveNullReferenceRefund())
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
            SecurePassword = string.Empty;
            countEnter = 0;
        }

        public AuthViewModel(INavigationService? navService)
        {
            NavigationService = navService;
            NavigateViewCommand = new RelayCommand(OnNavigateViewCommandExecuted, CanNavigateViewCommandExecute);
            AuthByPinCommand = new RelayCommand(OnAuthByPinCommandExecuted, CanAuthByPinCommandExecute);
            CloseApplicationCommand = new RelayCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
        }
    }
}
