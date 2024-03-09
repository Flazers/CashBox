using Cashbox.MVVM.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.Models
{
    public partial class AutoDreport
    {
        public AutoDreport() { }

        public static AutoDailyReportViewModel GenReport(DailyReport dailyReport)
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
    }
}
