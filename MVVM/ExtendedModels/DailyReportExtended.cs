using Cashbox.Core;
using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Windows;

namespace Cashbox.MVVM.Models
{
    public partial class DailyReport : ViewModelBase
    {
        public DailyReport() { }

        private static DailyReport? _currentShift = CashBoxDataContext.Context.DailyReports.FirstOrDefault(x => x.Data == DateOnly.FromDateTime(DateTime.Today) && x.UserId == UserViewModel.GetCurrentUser().Id);
        public static DailyReport? CurrentShift 
        { 
            get => _currentShift;
            set => _currentShift = value;
        }

        public static async Task<DailyReportViewModel?> StartShift(DateOnly date, TimeOnly time)
        {
            if (CashBoxDataContext.Context.DailyReports.FirstOrDefault(x => x.Data == date && x.UserId == UserViewModel.GetCurrentUser().Id && x.CloseTime != null) != null)
            {
                MessageBox.Show("Смена закрыта, открыть ее можно будет на следующий день", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
                return null;
            }
            try
            {
                UserViewModel user = UserViewModel.GetCurrentUser();
                DailyReport dailyReport = new()
                {
                    Data = date,
                    OpenTime = time,
                    UserId = user.Id,
                    CashOnStart = MoneyBox.GetMoney,
                };
                CashBoxDataContext.Context.Add(dailyReport);
                await CashBoxDataContext.Context.SaveChangesAsync();
                CurrentShift = CashBoxDataContext.Context.DailyReports.FirstOrDefault(x => x.Data == date && x.UserId == UserViewModel.GetCurrentUser().Id);
                return new DailyReportViewModel(dailyReport);
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return null;
            }

        }
        public static async Task<DailyReportViewModel?> EndShift(DateOnly date, TimeOnly? time, double Proceeds, int userId)
        {
            try
            {
                DailyReport DR = await CashBoxDataContext.Context.DailyReports.FirstOrDefaultAsync(x => x.Data == date && x.UserId == userId);
                DR.CloseTime = time;
                DR.Proceeds = Proceeds - DR.CashOnStart;
                return new(DR);
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return new(null!);
            }
        }

        public static async Task<List<DailyReportViewModel>> GetPeriodReports(DateOnly start, DateOnly end) => await CashBoxDataContext.Context.DailyReports.Where(x => x.Data >= start && x.Data <= end).Select(s => new DailyReportViewModel(s)).ToListAsync();
        public static async Task<List<DailyReportViewModel>> GetNotCloseReports() => await CashBoxDataContext.Context.DailyReports.Where(x => x.CloseTime == null).Select(s => new DailyReportViewModel(s)).ToListAsync();
    }
}
