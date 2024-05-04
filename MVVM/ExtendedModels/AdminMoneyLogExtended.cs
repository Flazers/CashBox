using Cashbox.Core;
using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;

namespace Cashbox.MVVM.Models
{
    public partial class AdminMoneyLog
    {
        public AdminMoneyLog() { }

        public static async Task<bool> CreateTransitMB(string action, double money)
        {
            try
            {
                AdminMoneyLog adminMoneyLog = new()
                {
                    Datetime = DateTime.Now,
                    UserId = UserViewModel.GetCurrentUser().Id,
                    Action = action,
                    Money = money,
                };
                CashBoxDataContext.Context.AdminMoneyLog.Add(adminMoneyLog);
                await CashBoxDataContext.Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }

        public static async Task<bool> CreateTransitAward(string action, double money, int subuserid)
        {
            try
            {
                AdminMoneyLog adminMoneyLog = new()
                {
                    Datetime = DateTime.Now,
                    UserId = UserViewModel.GetCurrentUser().Id,
                    Action = action,
                    Money = money,
                };
                CashBoxDataContext.Context.AdminMoneyLog.Add(adminMoneyLog);
                await CashBoxDataContext.Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }

        public static async Task<bool> CreateTransitSalary(string action, double money, int subuserid)
        {
            try
            {
                AdminMoneyLog adminMoneyLog = new()
                {
                    Datetime = DateTime.Now,
                    UserId = UserViewModel.GetCurrentUser().Id,
                    Action = action,
                    Money = money,
                };
                CashBoxDataContext.Context.AdminMoneyLog.Add(adminMoneyLog);
                await CashBoxDataContext.Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }

        public static async Task<List<AdminMoneyLogViewModel>> GetAllLog() => new(await CashBoxDataContext.Context.AdminMoneyLog.OrderByDescending(x => x.Datetime).Select(s => new AdminMoneyLogViewModel(s)).ToListAsync());
    }
}
