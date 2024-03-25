using Cashbox.Core;
using Cashbox.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class MoneyBoxViewModel(MoneyBox moneyBox) : ViewModelBase
    {
        private readonly MoneyBox _moneyBox = moneyBox;

        public async static Task<bool?> CreateBox() => await MoneyBox.CreateBox();
        public static double GetMoney => MoneyBox.GetMoney;
        public async static Task<bool?> UpdateMoney(double money, int operation = 0) => await MoneyBox.UpdateMoney(money, operation);
        public int Id => _moneyBox.Id;

        public double Money 
        { 
            get => _moneyBox.Money;
            set 
            { 
                _moneyBox.Money = value;
                OnPropertyChanged();
            }
        }

    }
}
