using Cashbox.Core;
using Cashbox.MVVM.Models;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class AppSettingsViewModel(AppSettings appSettings) : ViewModelBase
    {
        private readonly AppSettings _appSettings = appSettings;

        public static async Task<bool> EditSetting(int salary, int startCash) => await AppSettings.EditSetting(salary, startCash);
        public static async Task<bool> CreateSetting() => await AppSettings.CreateSetting();
        public static AppSettings Settings => AppSettings.Settings;

        public int Id => _appSettings.Id;

        public int Salary
        {
            get => _appSettings.Salary;
            set
            {
                _appSettings.Salary = value;
                OnPropertyChanged();
            }
        }

        public int StartCash
        {
            get => _appSettings.StartCash;
            set
            {
                _appSettings.StartCash = value;
                OnPropertyChanged();
            }
        }
    }
}
