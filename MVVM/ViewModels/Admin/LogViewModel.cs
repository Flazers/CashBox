using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.Models;
using Cashbox.MVVM.ViewModels.Data;
using System.Collections.ObjectModel;
using System.Windows;

namespace Cashbox.MVVM.ViewModels.Admin
{
    public class LogViewModel : ViewModelBase
    {

        #region Props

        private ObservableCollection<AdminMoneyLogViewModel> _collectionLogs = [];
        public ObservableCollection<AdminMoneyLogViewModel> CollectionsLogs
        {
            get => _collectionLogs;
            set => Set(ref _collectionLogs, value);
        }

        private int _logCount = 20;
        public int LogCount
        {
            get => _logCount;
            set => Set(ref _logCount, value);
        }

        private int _moneySetDef = AppSettingsViewModel.Settings.Salary;
        public int MoneySetDef
        {
            get => _moneySetDef;
            set => Set(ref _moneySetDef, value);
        }

        private string _moneySet = AppSettingsViewModel.Settings.Salary.ToString();
        public string MoneySet
        {
            get => _moneySet;
            set => Set(ref _moneySet, value);
        }

        private int _startCashtDef = AppSettingsViewModel.Settings.StartCash;
        public int StartCashtDef
        {
            get => _startCashtDef;
            set => Set(ref _startCashtDef, value);
        }

        private string _startCash = AppSettingsViewModel.Settings.StartCash.ToString();
        public string StartCash
        {
            get => _startCash;
            set => Set(ref _startCash, value);
        }

        private string _searchStr = string.Empty;
        public string SearchStr
        {
            get => _searchStr;
            set => Set(ref _searchStr, value);
        }

        #endregion

        #region Commands

        public RelayCommand SaveSettingsCommand { get; set; }
        private bool CanSaveSettingsCommandExecute(object p) => true;
        private async void OnSaveSettingsCommandExecuted(object p) 
        {
            if (string.IsNullOrEmpty(MoneySet))
            {
                AppCommand.WarningMessage("Введите зарплату за выход");
                return;
            }
            if (string.IsNullOrEmpty(StartCash))
            {
                AppCommand.WarningMessage("Введите деньги на начало смены");
                return;
            }
            if (StartCash == StartCashtDef.ToString() && MoneySet == MoneySetDef.ToString())
            {
                AppCommand.InfoMessage("Изменения сохранены");
                return;
            }
            UserViewModel user = UserViewModel.GetCurrentUser();
            await AppSettingsViewModel.EditSetting(int.Parse(MoneySet), int.Parse(StartCash));
            if (StartCash != StartCashtDef.ToString())
                await AdminMoneyLogViewModel.CreateTransitMB($"Администратор (id: {user.Id}) {user.UserInfo.ShortName} отредактировал деньги на начало смены ( {StartCashtDef} ₽ => {StartCash} ₽)", int.Parse(StartCash));
            if (MoneySet != MoneySetDef.ToString())
                await AdminMoneyLogViewModel.CreateTransitMB($"Администратор (id: {user.Id}) {user.UserInfo.ShortName} отредактировал зарплату за выход ( {MoneySetDef} ₽ => {MoneySet} ₽)", int.Parse(MoneySet));
            AppCommand.InfoMessage("Изменения сохранены");
            await Update();
        }

        public RelayCommand ClearCountSellCommand { get; set; }
        private bool CanClearCountSellCommandExecute(object p) => true;
        private async void OnClearCountSellCommandExecuted(object p)
        {
            if (AppCommand.QuestionMessage("Вы уверены что хотите очистить колличество проданных товаров за все время?") == MessageBoxResult.Yes)
            {
                await ProductViewModel.ClearCountSell();
                UserViewModel user = UserViewModel.GetCurrentUser();
                await AdminMoneyLogViewModel.CreateTransitMB($"Администратор (id: {user.Id}) {user.UserInfo.ShortName} очистил колличество проданных товаров за все время", 0);
                AppCommand.InfoMessage("Успех");
                await Update();
            }
        }

        public RelayCommand UpdateLogCommand { get; set; }
        private bool CanUpdateLogCommandExecute(object p) => true;
        private async void OnUpdateLogCommandExecuted(object p) => await Update();

        #endregion

        public async Task Update()
        {
            List<AdminMoneyLogViewModel> list = await AdminMoneyLogViewModel.GetAllLog();
            if (string.IsNullOrEmpty(SearchStr))
                CollectionsLogs = new(list.Take(LogCount).ToList());
            else
                CollectionsLogs = new(list.Where(x => x.DataString.ToString().Contains(SearchStr.Trim()) || 
                                                      x.Action.Trim().Contains(SearchStr.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                                      ).Take(LogCount));
            MoneySetDef = AppSettingsViewModel.Settings.Salary;
            StartCashtDef = AppSettingsViewModel.Settings.StartCash;
        }

        public override void OnLoad()
        {
            Task.Run(Update);
        }

        public LogViewModel()
        {
            ClearCountSellCommand = new RelayCommand(OnClearCountSellCommandExecuted, CanClearCountSellCommandExecute);
            SaveSettingsCommand = new RelayCommand(OnSaveSettingsCommandExecuted, CanSaveSettingsCommandExecute);
            UpdateLogCommand = new RelayCommand(OnUpdateLogCommandExecuted, CanUpdateLogCommandExecute);
        }
    }
}
