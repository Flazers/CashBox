using Cashbox.Core;
using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.Models
{
    public partial class DailyReport
    {
        public DailyReport() { }
        public static DailyReportViewModel? CurrentShift => CashBoxDataContext.Context.DailyReports.Select(s => new DailyReportViewModel(s)).FirstOrDefault(x => x.Data == DateOnly.FromDateTime(DateTime.Now) && x.UserId == UserViewModel.GetCurrentUser().Id); 

        public static async Task<DailyReportViewModel?> StartShift(DateTime dateTime)
        {
            UserViewModel user = UserViewModel.GetCurrentUser();
            DailyReport dailyReport = new()
            {
                Data = DateOnly.FromDateTime(dateTime),
                OpenTime = TimeOnly.FromDateTime(dateTime),
                UserId = user.Id,
            };
            CashBoxDataContext.Context.Add(dailyReport);
            await CashBoxDataContext.Context.SaveChangesAsync();
            return new DailyReportViewModel(dailyReport);
        }
        public static async Task<DailyReportViewModel?> EndShift(DateTime dateTime, double Proceeds)
        {
            UserViewModel user = UserViewModel.GetCurrentUser();
            DailyReport DR = CashBoxDataContext.Context.DailyReports.FirstOrDefault(x => x.Data == DateOnly.FromDateTime(dateTime) && x.UserId == user.Id);
            DR.CloseTime = TimeOnly.FromDateTime(dateTime);
            DR.Proceeds = Proceeds;
            AutoDailyReportViewModel adreport = await AutoDailyReportViewModel.GenEndShiftAuto(DR);
            await CashBoxDataContext.Context.SaveChangesAsync();
            return new DailyReportViewModel(DR);
        }
    }
}
