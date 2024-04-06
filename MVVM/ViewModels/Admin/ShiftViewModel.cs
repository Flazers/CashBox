using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cashbox.MVVM.ViewModels.Admin
{
    public class ShiftViewModel : ViewModelBase
    {
        #region Props

        private OrderViewModel? _selectedOrder;
        public OrderViewModel? SelectedOrder 
        {
            get => _selectedOrder;
            set => Set(ref _selectedOrder, value);
        }

        private DateOnly _endDate = DateOnly.FromDateTime(DateTime.Today);
        public DateOnly EndDate
        {
            get => _endDate;
            set => Set(ref _endDate, value);
        }

        private DateOnly _startDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-30));
        public DateOnly StartDate
        {
            get => _startDate;
            set => Set(ref _startDate, value);
        }

        private ObservableCollection<DailyReportViewModel> _dailyReportCollection;
        public ObservableCollection<DailyReportViewModel> DailyReportCollection
        {
            get => _dailyReportCollection;
            set => Set(ref _dailyReportCollection, value);
        }

        private DailyReportViewModel _selectedDReport;
        public DailyReportViewModel SelectedDReport
        {
            get => _selectedDReport;
            set => Set(ref _selectedDReport, value);
        }

        //private OrderViewModel? _selectedOrder;
        //public OrderViewModel? SelectedOrder
        //{
        //    get => _selectedOrder;
        //    set => Set(ref _selectedOrder, value);
        //}
        #endregion

        #region Commands
        public RelayCommand RemoveProductCommand { get; set; }
        private bool CanRemoveProductCommandExecute(object p) => true;
        private async void OnRemoveProductCommandExecuted(object p)
        {
           
        }

        public RelayCommand SearchDataCommand { get; set; }
        private bool CanSearchDataCommandExecute(object p) => true;
        private async void OnSearchDataCommandExecuted(object p)
        {
            DailyReportCollection = new(DailyReportViewModel.GetPeriodReports(StartDate, EndDate).Result);
        }
        #endregion

        public ShiftViewModel()
        {
            DailyReportCollection = new(DailyReportViewModel.GetPeriodReports(StartDate, EndDate).Result);
            SearchDataCommand = new RelayCommand(OnSearchDataCommandExecuted, CanSearchDataCommandExecute);
        }
    }
}
