using Cashbox.Core;

namespace Cashbox.MVVM.Models
{
    public partial class AppSettings
    {
        public AppSettings() { }

        private static AppSettings _settings = CashBoxDataContext.Context.AppSettings.FirstOrDefault(x => x.Id == 1)!;
        public static AppSettings Settings => _settings;

        private static async Task<bool> CreateSettings()
        {
            try
            {
                AppSettings appsetting = new()
                {
                    Salary = 1000,
                };
                CashBoxDataContext.Context.AppSettings.Add(appsetting);
                await CashBoxDataContext.Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }

        private static async Task<bool> EditSettings(int salary, int award)
        {
            try
            {
                AppSettings appsetting = CashBoxDataContext.Context.AppSettings.FirstOrDefault(x => x.Id == 1)!;
                if (salary != 0)
                    appsetting.Salary = salary;
                await CashBoxDataContext.Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }

        public static async Task<bool> CreateSetting() => await CreateSettings();
        public static async Task<bool> EditSetting(int salary, int award) => await EditSettings(salary, award);
    }
}
