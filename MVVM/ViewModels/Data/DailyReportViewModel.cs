using Cashbox.Core;
using Cashbox.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class DailyReportViewModel(DailyReport DailyReports) : ViewModelBase
    {
        private readonly DailyReport _dailyReport = DailyReports;

        public static async Task<DailyReportViewModel?> StartShift(DateTime dateTime) => await DailyReport.StartShift(dateTime);
        public static async Task<DailyReportViewModel?> EndShift(DateTime dateTime, double processed) => await DailyReport.EndShift(dateTime, processed);
        public static DailyReportViewModel? GetCurrentShift() => DailyReport.CurrentShift;
        public int Id => _dailyReport.Id;

        public DateOnly? Data 
        {
            get => _dailyReport.Data;
            set
            {
                _dailyReport.Data = value;
                OnPropertyChanged();
            }
        }

        public TimeOnly? OpenTime 
        {
            get => _dailyReport.OpenTime;
            set
            {
                _dailyReport.OpenTime = value;
                OnPropertyChanged();
            }
        }

        public TimeOnly? CloseTime 
        {
            get => _dailyReport.CloseTime;
            set
            {
                _dailyReport.CloseTime = value;
                OnPropertyChanged();
            }
        }

        public int UserId 
        {
            get => _dailyReport.UserId;
            set
            {
                _dailyReport.UserId = value;
                OnPropertyChanged();
            }
        }

        public double? Proceeds 
        {
            get => _dailyReport.Proceeds;
            set
            {
                _dailyReport.Proceeds = value;
                OnPropertyChanged();
            }
        }

        public virtual AutoDreport? AutoDreport { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
