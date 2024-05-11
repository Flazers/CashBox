using Cashbox.Core;
using Cashbox.MVVM.ViewModels.Data;

namespace Cashbox.MVVM.Models
{
    public partial class AutoDreport
    {
        public AutoDreport() { }

        public static async Task<bool> CreateAutoReport(DailyReportViewModel dailyReport)
        {
            try
            {
                AutoDreport autoDreport = new()
                {
                    DailyReportId = dailyReport.Id,
                    AutoProceeds = 0,
                    Award = 0,
                    Forfeit = 0,
                    FullTransit = 0,
                    Salary = 0,
                };
                await CashBoxDataContext.Context.AutoDreports.AddAsync(autoDreport);
                await CashBoxDataContext.Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }

        public static async Task<AutoDailyReportViewModel> GenReport(DailyReportViewModel dailyReport)
        {
            try
            {
                AppSettings setting = AppSettingsViewModel.Settings;
                AutoDreport autoDreport = CashBoxDataContext.Context.AutoDreports.FirstOrDefault(x => x.DailyReportId == dailyReport.Id);
                if (autoDreport == null)
                    return null!;
                List<OrderViewModel> AutoPro = await OrderViewModel.GetDayOrdersToMethod((DateOnly)dailyReport.Data!, 2);
                autoDreport.Salary = setting.Salary;
                if (AutoPro.Count != 0)
                    autoDreport.AutoProceeds = AutoPro.Sum(x => (double)x.SellCostWithDiscount!);
                autoDreport.FullTransit = await OrderViewModel.GetSumInDay(dailyReport.Data);
                double forfeit = (double)autoDreport.AutoProceeds! - (double)dailyReport.Proceeds!;
                autoDreport.Forfeit = forfeit;
                if (forfeit < 0)
                    autoDreport.Salary -= Convert.ToInt32(forfeit);
                await MoneyBoxViewModel.UpdateMoney((int)dailyReport.Proceeds, 1);
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
