using Cashbox.Core;
using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;

namespace Cashbox.MVVM.Models
{
    public partial class DailyReport : ViewModelBase
    {
        public DailyReport() { }

        private static DailyReportViewModel? _currentShift = null;
        public static DailyReportViewModel? CurrentShift
        {
            get
            {
                DailyReport dr = CashBoxDataContext.Context.DailyReports.FirstOrDefault(x => x.Data == DateOnly.FromDateTime(DateTime.Today) && x.UserId == UserViewModel.GetCurrentUser().Id);
                if (dr != null)
                    CurrentShift = new(dr);
                return _currentShift;
            }
            private set => _currentShift = value;
        }

        public static async Task<DailyReportViewModel?> StartShift(DateOnly date, TimeOnly time)
        {
            if (CashBoxDataContext.Context.DailyReports.FirstOrDefault(x => x.Data == date && x.CloseTime != null) != null)
            {
                AppCommand.WarningMessage("Смена закрыта, открыть ее можно будет на следующий день");
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
                    TakedSalary = false
                };
                CashBoxDataContext.Context.Add(dailyReport);
                await CashBoxDataContext.Context.SaveChangesAsync();
                CurrentShift = new(dailyReport);
                await AutoDreport.CreateAutoReport(CurrentShift);
                return new(dailyReport);
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return null;
            }

        }

        public static async Task<DailyReportViewModel?> EndShift(DateOnly date, TimeOnly? time, int Proceeds, int userId)
        {
            try
            {
                DailyReport DR = await CashBoxDataContext.Context.DailyReports.FirstOrDefaultAsync(x => x.Data == date && x.UserId == userId);
                DR.CloseTime = time;
                DR.Proceeds = Proceeds;
                return new(DR);
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return new(null!);
            }
        }

        public static async Task<bool> TakeSalary(DailyReportViewModel drvm, int award, int salary)
        {
            try
            {
                if (drvm == null)
                    return false;
                drvm.TakedSalary = true;
                int allsalary = salary + award;
                int tempsalary = 0;
                drvm.AutoDreportVM.Salary = salary;
                drvm.AutoDreportVM.Award = award;
                int money = MoneyBoxViewModel.GetMoney;
                if (money < allsalary)
                {
                    tempsalary = allsalary - money;
                    AppCommand.InfoMessage($"Денег в кассе недостаточно для выдачи полной зарплаты, напишите администратору для получения остатка зарплаты: \nЗарплата: {salary}\nПремия: {award}\nВзято из кассы: {allsalary - tempsalary}\nОстаток:{tempsalary} ₽");
                }
                await MoneyBoxViewModel.UpdateMoney(allsalary - tempsalary, 2);
                if (allsalary - tempsalary != 0)
                    await AdminMoneyLogViewModel.CreateTransitMB($"Сотрудник (id: {drvm.UserId}) {drvm.UserInfoVM.FullName} забрал из кассы {allsalary - tempsalary} ₽ \nЗарплата: {salary} ₽. \nПремия: {award} ₽ ", allsalary - tempsalary);
                drvm.UserInfoVM.Salary -= allsalary;
                await CashBoxDataContext.Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }

        public static async Task<List<DailyReportViewModel>> GetPeriodReports(DateOnly start, DateOnly end) => await CashBoxDataContext.Context.DailyReports.Where(x => x.Data >= start && x.Data <= end).Select(s => new DailyReportViewModel(s)).ToListAsync();
        public static async Task<DailyReportViewModel?> GetReport(DateOnly date) => await CashBoxDataContext.Context.DailyReports.Where(x => x.Data == date).Select(s => new DailyReportViewModel(s)).FirstOrDefaultAsync();
        public static async Task<List<DailyReportViewModel>> GetNotCloseReports() => await CashBoxDataContext.Context.DailyReports.Where(x => x.CloseTime == null).Select(s => new DailyReportViewModel(s)).ToListAsync();
    }
}
