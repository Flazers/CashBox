using Cashbox.Core;
using Cashbox.MVVM.Models;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class MoneyBoxViewModel(MoneyBox moneyBox) : ViewModelBase
    {
        private readonly MoneyBox _moneyBox = moneyBox;

        public async static Task<bool?> CreateBox() => await MoneyBox.CreateBox();
        public static int GetMoney => MoneyBox.GetMoney;
        public async static Task<bool> UpdateMoney(int money, int operation = 0) => await MoneyBox.UpdateMoney(money, operation);
        public int Id => _moneyBox.Id;

        public int Money
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
