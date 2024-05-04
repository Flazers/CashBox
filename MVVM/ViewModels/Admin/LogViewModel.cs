using Cashbox.Core;
using Cashbox.Core.Commands;
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
            await AppSettingsViewModel.EditSetting(int.Parse(MoneySet));
            await AdminMoneyLogViewModel.CreateTransitMB($"Администратор {UserViewModel.GetCurrentUser().UserInfo.ShortName} отредактировал зарплату за выход ( {MoneySetDef} ₽ => {MoneySet} ₽)", int.Parse(MoneySet));
            AppCommand.InfoMessage("Новая зарплата за выход установлена");
            Update();
        }

        public RelayCommand UpdateLogCommand { get; set; }
        private bool CanUpdateLogCommandExecute(object p) => true;
        private void OnUpdateLogCommandExecuted(object p) => Update();

        #endregion

        public async void Update()
        {
            List<AdminMoneyLogViewModel> list = await AdminMoneyLogViewModel.GetAllLog();
            if (string.IsNullOrEmpty(SearchStr))
                CollectionsLogs = new(list.Take(LogCount).ToList());
            else
                CollectionsLogs = new(list.Where(x => x.DataString.ToString().Contains(SearchStr.Trim()) || 
                                                      x.Action.Trim().Contains(SearchStr.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                                      ).Take(LogCount));
            MoneySetDef = AppSettingsViewModel.Settings.Salary;
        }

        public LogViewModel()
        {
            Update();
            SaveSettingsCommand = new RelayCommand(OnSaveSettingsCommandExecuted, CanSaveSettingsCommandExecute);
            UpdateLogCommand = new RelayCommand(OnUpdateLogCommandExecuted, CanUpdateLogCommandExecute);
        }
    }
}
