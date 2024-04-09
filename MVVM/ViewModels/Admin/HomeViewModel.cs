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

        public RelayCommand InEditMoneyBoxCommand { get; set; }
        private bool CanInEditMoneyBoxCommandExecute(object p) => true;
        private async void OnInEditMoneyBoxCommandExecuted(object p)
        {
            await MoneyBoxViewModel.UpdateMoney(NewCashInBox, 1);
            MessageBox.Show($"{NewCashInBox} ₽ внесено в кассу", "Успех");
            CashInBox = MoneyBoxViewModel.GetMoney;
        }

        public RelayCommand OutEditMoneyBoxCommand { get; set; }
        private bool CanOutEditMoneyBoxCommandExecute(object p) => true;
        private async void OnOutEditMoneyBoxCommandExecuted(object p)
        {
            await MoneyBoxViewModel.UpdateMoney(NewCashInBox, 2);
            MessageBox.Show($"{NewCashInBox} ₽ вычтено из кассы", "Успех");
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
            InEditMoneyBoxCommand = new RelayCommand(OnInEditMoneyBoxCommandExecuted, CanInEditMoneyBoxCommandExecute);
            OutEditMoneyBoxCommand = new RelayCommand(OnOutEditMoneyBoxCommandExecuted, CanOutEditMoneyBoxCommandExecute);
        }
    }
}
