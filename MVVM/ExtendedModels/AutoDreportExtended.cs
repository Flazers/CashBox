using Cashbox.Core;
using Cashbox.MVVM.ViewModels.Data;

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
                    AutoProceeds = 0,
                    Award = 0,
                    FullTransit = 0,
                    Salary = setting.Salary,
                };
                List<OrderViewModel> AutoPro = await OrderViewModel.GetDayOrdersToMethod((DateOnly)dailyReport.Data!, 2);
                if (AutoPro.Count != 0)
                    autoDreport.AutoProceeds = AutoPro.Sum(x => (double)x.SellCost!);
                autoDreport.FullTransit = await OrderViewModel.GetSumInDay(dailyReport.Data);
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
                dailyReport.UserInfoVM.Salary += autoDreport.Salary;
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new(autoDreport);
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return null!;
            }
        }

        public static async Task<bool> GiveAward(DailyReportViewModel dailyReport, int award)
        {
            try
            {
                AutoDreport report = CashBoxDataContext.Context.AutoDreports.FirstOrDefault(x => x.DailyReportId == dailyReport.Id);
                if (report == null)
                    return false;
                report.Award += award;
                report.DailyReport.User.UserInfo.Salary += award;
                await CashBoxDataContext.Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }
    }
}
