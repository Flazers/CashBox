using Cashbox.Core;
using Microsoft.EntityFrameworkCore;

namespace Cashbox.MVVM.Models
{
    public partial class MoneyBox
    {
        public MoneyBox() { }


        public async static Task<bool> CreateBox()
        {
            try
            {
                MoneyBox moneyBox = new() { Id = 1, Money = 0 };
                CashBoxDataContext.Context.Add(moneyBox);
                await CashBoxDataContext.Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }

        public async static Task<bool> UpdateMoney(double money, int operation)
        {
            try
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
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }

        }

        public static double GetMoney => CashBoxDataContext.Context.MoneyBoxes.FirstOrDefault(x => x.Id == 1).Money;
    }
}
