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

        public static async Task<AppSettingsViewModel> EditSetting(int salary = 0, double moneybox = 0, string email = "", int award = 0) => await AppSettings.EditSetting(salary, moneybox, email, award);
        public static async Task<AppSettingsViewModel> CreateSetting() => await AppSettings.CreateSetting();
        public static async Task<AppSettingsViewModel?> GetSetting() => await AppSettings.GetSetting();

        public int Id => _appSettings.Id;

        public double? MoneyBox
        {
            get => _appSettings.MoneyBox;
            set
            {
                _appSettings.MoneyBox = value;
                OnPropertyChanged();
            }
        }

        public string? MainEmail
        {
            get => _appSettings.MainEmail;
            set
            {
                _appSettings.MainEmail = value;
                OnPropertyChanged();
            }
        }

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
