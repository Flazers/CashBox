using Cashbox.Core;
using Cashbox.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class AppSettingsViewModel(AppSettings appSettings) : ViewModelBase
    {
        private readonly AppSettings _appSettings = appSettings;

        public static async Task<bool> EditSetting(int salary = 0, int award = 0) => await AppSettings.EditSetting(salary, award);
        public static async Task<bool> CreateSetting() => await AppSettings.CreateSetting();
        public static AppSettings Settings => AppSettings.Settings;

        public int Id => _appSettings.Id;

        public int? Salary
        {
            get => _appSettings.Salary;
            set
            {
                _appSettings.Salary = value;
                OnPropertyChanged();
            }
        }

        public double? AwardProcent
        {
            get => _appSettings.AwardProcent;
            set
            {
                _appSettings.AwardProcent = value;
                OnPropertyChanged();
            }
        }
    }
}
