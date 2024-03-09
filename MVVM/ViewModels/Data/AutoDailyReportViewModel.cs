using Cashbox.Core;
using Cashbox.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class AutoDailyReportViewModel(AutoDreport AutoDreports) : ViewModelBase
    {
        private readonly AutoDreport _autoDreport = AutoDreports;

        public static AutoDailyReportViewModel GenEndShiftAuto(DailyReport dailyReport) => AutoDreport.GenReport(dailyReport);
        public int DailyReportId => _autoDreport.DailyReportId;

        public int? Salary
        {
            get => _autoDreport.Salary;
            set
            {
                _autoDreport.Salary = value;
                OnPropertyChanged();
            }
        }

        public double AutoProceeds
        {
            get => _autoDreport.AutoProceeds;
            set
            {
                _autoDreport.AutoProceeds = value;
                OnPropertyChanged();
            }
        }

        public double Forfeit
        {
            get => _autoDreport.Forfeit;
            set
            {
                _autoDreport.Forfeit = value;
                OnPropertyChanged();
            }
        }

        public double Award
        {
            get => _autoDreport.Award;
            set
            {
                _autoDreport.Award = value;
                OnPropertyChanged();
            }
        }

        public virtual DailyReport DailyReport { get; set; } = null!;
    }
}
