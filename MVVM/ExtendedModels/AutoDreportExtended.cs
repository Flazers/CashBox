using Cashbox.Core;
using Cashbox.MVVM.ViewModels.Data;

namespace Cashbox.MVVM.Models
{
    public partial class AutoDreport
    {
        public AutoDreport() { }

        public static AutoDailyReportViewModel? GenReport(DailyReport dailyReport)
        {
            try
            {
                var setting = AppSettingsViewModel.Settings;
                AutoDreport autoDreport = new()
                {
                    DailyReportId = dailyReport.Id,
                    Salary = setting.Salary,
                    Award = OrderViewModel.GetSumInDay(dailyReport.Data),
                    Forfeit = OrderViewModel.GetSumMethodInDay(dailyReport.Data) - (double)dailyReport.Proceeds!,
                };
                return new(autoDreport);

            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return null;
            }
        }
    }
}
