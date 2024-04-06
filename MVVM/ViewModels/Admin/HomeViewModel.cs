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

        #endregion

        #region Commands

        #endregion

        public override void OnLoad()
        {
            //var data = Order.GetSellDetail(DateOnly.Parse("2.04.2024"), DateOnly.Parse("5.04.2024"));
            AuthHistory = AuthHistoryViewModel.GetAuthHistories().Result.TakeLast(3).OrderByDescending(x => x.Datetime).ToList();
        }

        public HomeViewModel()
        {

        }
    }
}
