using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.Models
{
    public partial class AppSettings
    {
        public AppSettings() { }

        private static AppSettings _settings = CashBoxDataContext.Context.AppSettings.FirstOrDefault(x => x.Id == 1)!;
        public static AppSettings Settings => _settings;

        private static async Task<AppSettingsViewModel> CreateSettings()
        {
            AppSettings appsetting = new()
            {
                Salary = 1000,
                MoneyBox = 0,
                AwardProcent = 100,
                MainEmail = string.Empty,
            };
            CashBoxDataContext.Context.AppSettings.Add(appsetting);
            await CashBoxDataContext.Context.SaveChangesAsync();
            return new(appsetting);
        }

        private static async Task<AppSettingsViewModel> EditSettings(int salary, double moneybox, string email, int award)
        {
            AppSettings appsetting = CashBoxDataContext.Context.AppSettings.FirstOrDefault(x => x.Id == 1)!;
            if (salary != 0)
                appsetting.Salary = salary;
            if (award != 0)
                appsetting.AwardProcent = award;
            if (email != "")
                appsetting.MainEmail = email.ToLower().Trim();
            if (moneybox != 0)
                appsetting.MoneyBox = moneybox;
            await CashBoxDataContext.Context.SaveChangesAsync();
            return new(appsetting);
        }

        public static async Task<AppSettingsViewModel> CreateSetting() => await CreateSettings();
        public static async Task<AppSettingsViewModel> EditSetting(int salary, double moneybox, string email, int award) => await EditSettings(salary, moneybox, email, award);
    }
}
