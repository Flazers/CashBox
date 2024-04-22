using Cashbox.Core;
using Cashbox.MVVM.ViewModels.Data;
using System.Diagnostics;

namespace Cashbox.MVVM.Models
{
    public partial class AutoDreport
    {
        public AutoDreport() { }

        public static async Task<AutoDailyReportViewModel> GenReport(DailyReportViewModel dailyReport)
        {
            try
            {
                var setting = AppSettingsViewModel.Settings;
                AutoDreport autoDreport = new()
                {
                    DailyReportId = dailyReport.Id,
                    AutoProceeds = OrderViewModel.GetDayOrdersToMethod((DateOnly)dailyReport.Data!, 2).Result.Sum(x => (double)x.SellCost!),
                    Award = 0,
                    FullTransit = OrderViewModel.GetSumInDay(dailyReport.Data),
                    Salary = setting.Salary,
                };
                double forfeit = (double)autoDreport.AutoProceeds - (double)dailyReport.Proceeds!;
                if (forfeit <= 0)
                    autoDreport.Forfeit = 0;
                else
                {
                    autoDreport.Forfeit = forfeit;
                    autoDreport.Salary -= Convert.ToInt32(forfeit);
                }
                await CashBoxDataContext.Context.AutoDreports.AddAsync(autoDreport);
                await MoneyBoxViewModel.UpdateMoney((double)dailyReport.Proceeds, 1);
                dailyReport.UserInfoVM.Salary = autoDreport.Salary;
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new(autoDreport);
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return null!;
            }
        }
    }
}
