using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.Models;
using Cashbox.MVVM.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Cashbox.MVVM.ViewModels.Admin
{
    public class HomeViewModel : ViewModelBase
    {

        #region Props

        private List<AuthHistoryViewModel>? _authHistory;
        public List<AuthHistoryViewModel>? AuthHistory
        {
            get => _authHistory;
            set => Set(ref _authHistory, value);
        }

        private double _cashInBox = MoneyBoxViewModel.GetMoney;
        public double CashInBox
        {
            get => _cashInBox;
            set => Set(ref _cashInBox, value);
        }

        private double _newCashInBox = MoneyBoxViewModel.GetMoney;
        public double NewCashInBox
        {
            get => _newCashInBox;
            set => Set(ref _newCashInBox, value);
        }

        #endregion

        #region Commands

        public RelayCommand EditMoneyBoxCommand { get; set; }
        private bool CanEditMoneyBoxCommandExecute(object p) => true;
        private async void OnEditMoneyBoxCommandExecuted(object p)
        {
            await MoneyBoxViewModel.UpdateMoney(NewCashInBox);
            MessageBox.Show("Данные обновлены", "Успех");
            CashInBox = MoneyBoxViewModel.GetMoney;
        }

        #endregion

        public override void OnLoad()
        {
            //var data = Order.GetSellDetail(DateOnly.Parse("2.04.2024"), DateOnly.Parse("5.04.2024"));
            AuthHistory = AuthHistoryViewModel.GetAuthHistories().Result.TakeLast(3).OrderByDescending(x => x.Datetime).ToList();
        }

        public HomeViewModel()
        {
            EditMoneyBoxCommand = new RelayCommand(OnEditMoneyBoxCommandExecuted, CanEditMoneyBoxCommandExecute);
        }
    }
}
