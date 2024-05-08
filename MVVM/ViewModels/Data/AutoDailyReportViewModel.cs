using Cashbox.Core;
using Cashbox.MVVM.Models;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class AutoDailyReportViewModel(AutoDreport AutoDreports) : ViewModelBase
    {
        private readonly AutoDreport _autoDreport = AutoDreports;

        public static async Task<bool> StartShiftAuto(DailyReportViewModel dailyReport) => await AutoDreport.CreateAutoReport(dailyReport);
        public static async Task<AutoDailyReportViewModel> GenEndShiftAuto(DailyReportViewModel dailyReport) => await AutoDreport.GenReport(dailyReport);
        public static async Task<bool> GiveAward(DailyReportViewModel dailyReport, int award) => await AutoDreport.GiveAward(dailyReport, award);
        public int DailyReportId => _autoDreport.DailyReportId;

        public int Salary
        {
            get => _autoDreport.Salary;
            set
            {
                _autoDreport.Salary = value;
                OnPropertyChanged();
            }
        }

        public double? AutoProceeds
        {
            get => _autoDreport.AutoProceeds;
            set
            {
                _autoDreport.AutoProceeds = value;
                OnPropertyChanged();
            }
        }

        public double? FullTransit
        {
            get => _autoDreport.FullTransit;
            set
            {
                _autoDreport.FullTransit = value;
                OnPropertyChanged();
            }
        }

        public double? Forfeit
        {
            get => _autoDreport.Forfeit;
            set
            {
                _autoDreport.Forfeit = value;
                OnPropertyChanged();
            }
        }

        public double? Award
        {
            get => _autoDreport.Award;
            set
            {
                _autoDreport.Award = value;
                OnPropertyChanged();
            }
        }

        public virtual DailyReport DailyReport => _autoDreport.DailyReport;
        public DailyReportViewModel DailyReportVM => new(_autoDreport.DailyReport);
    }
}
