using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.Models
{
    public partial class MoneyBox
    {
        public MoneyBox() { }


        public async static Task<MoneyBoxViewModel> CreateBox()
        {
            MoneyBox moneyBox = new() { Id = 1, Money = 0};
            CashBoxDataContext.Context.Add(moneyBox);
            await CashBoxDataContext.Context.SaveChangesAsync();
            return new(moneyBox);
        }

        public async static Task<MoneyBoxViewModel> UpdateMoney(double money, int operation)
        {
            MoneyBox moneyBox = await CashBoxDataContext.Context.MoneyBoxes.FirstOrDefaultAsync(x => x.Id == 1);
            switch (operation)
            {
                case 1:
                    moneyBox.Money += money;
                    break;
                case 2:
                    moneyBox.Money -= money;
                    break;
                default:
                    moneyBox.Money = money;
                    break;
            }
            await CashBoxDataContext.Context.SaveChangesAsync();
            return new(moneyBox);
        }

        public static double GetMoney => CashBoxDataContext.Context.MoneyBoxes.FirstOrDefault(x => x.Id == 1).Money;
    }
}
