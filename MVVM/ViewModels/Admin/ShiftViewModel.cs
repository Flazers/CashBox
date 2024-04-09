using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.Models;
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

        private Visibility _notUserSelectedPanel;
        public Visibility NotUserSelectedPanel
        {
            get 
            {
                if (SelectedOrder != null)
                    return _notUserSelectedPanel = Visibility.Visible;
                return _notUserSelectedPanel = Visibility.Collapsed;
            }
        }

        private Visibility _checkListVisibility = Visibility.Collapsed;
        public Visibility CheckListVisibility
        {
            get => _checkListVisibility;
            set => Set(ref  _checkListVisibility, value);
        }

        private Visibility _checkListOneObjVisibility = Visibility.Collapsed;
        public Visibility CheckListOneObjVisibility
        {
            get => _checkListOneObjVisibility;
            set => Set(ref _checkListOneObjVisibility, value);
        }

        private ObservableCollection<OrderViewModel> _selectedOrderList = [];
        public ObservableCollection<OrderViewModel> SelectedOrderList
        {
            get => _selectedOrderList;
            set => Set(ref _selectedOrderList, value);
        }

        private OrderViewModel? _selectedOrder;
        public OrderViewModel? SelectedOrder
        {
            get => _selectedOrder;
            set => Set(ref _selectedOrder, value);
        }

        private ObservableCollection<ProductViewModel?> _selectedOrderProduct;
        public ObservableCollection<ProductViewModel?> SelectedOrderProduct
        {
            get => _selectedOrderProduct;
            set => Set(ref _selectedOrderProduct, value);
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

        public string EndDateString 
        {
            get => _endDate.ToString("dd/MM/yyyy");
            set => Set(ref _endDate, DateOnly.ParseExact(value, @"dd/MM/yyyy", null));
        }

        public string StartDateString
        {
            get => _startDate.ToString("dd/MM/yyyy");
            set => Set(ref _startDate, DateOnly.ParseExact(value, @"dd/MM/yyyy", null));
        }
        

        private ObservableCollection<DailyReportViewModel> _dailyReportCollection = [];
        public ObservableCollection<DailyReportViewModel> DailyReportCollection
        {
            get => _dailyReportCollection;
            set => Set(ref _dailyReportCollection, value);
        }

        private DailyReportViewModel? _selectedDReport;
        public DailyReportViewModel? SelectedDReport
        {
            get => _selectedDReport;
            set => Set(ref _selectedDReport, value);
        }
        #endregion

        #region Commands

        public RelayCommand SearchDataCommand { get; set; }
        private bool CanSearchDataCommandExecute(object p) => true;
        private async void OnSearchDataCommandExecuted(object p)
        {
            DailyReportCollection = new(DailyReportViewModel.GetPeriodReports(StartDate, EndDate).Result);
        }

        public RelayCommand SeeOrderListCommand { get; set; }
        private bool CanSeeOrderListCommandExecute(object p) => true;
        private async void OnSeeOrderListCommandExecuted(object p)
        {
            if (p == null) return;
            SelectedOrder = SelectedOrderList.FirstOrDefault(x => x.Id == (int)p)!;
            List<OrderProductViewModel> temp = await OrderProductViewModel.GetInOrderProduct(SelectedOrder.Id);
            foreach (OrderProductViewModel item in temp)
                SelectedOrderProduct.Add(item.ProductVM);
            CheckListVisibility = Visibility.Collapsed;
            CheckListOneObjVisibility = Visibility.Visible;
        }

        public RelayCommand SelectShiftCommand { get; set; }
        private bool CanSelectShiftCommandExecute(object p) => true;
        private async void OnSelectShiftCommandExecuted(object p)
        {
            if (p == null) return;
            SelectedDReport = DailyReportCollection.FirstOrDefault(x => x.Id == (int)p)!;
            SelectedOrderList = new(await OrderViewModel.GetAllDayOrders((DateOnly)SelectedDReport.Data!));
            CheckListOneObjVisibility = Visibility.Collapsed;
            CheckListVisibility = Visibility.Visible;
        }
        #endregion

        public ShiftViewModel()
        {
            DailyReportCollection = new(DailyReportViewModel.GetPeriodReports(StartDate, EndDate).Result);
            SearchDataCommand = new RelayCommand(OnSearchDataCommandExecuted, CanSearchDataCommandExecute);
            SeeOrderListCommand = new RelayCommand(OnSeeOrderListCommandExecuted, CanSeeOrderListCommandExecute);
            SelectShiftCommand = new RelayCommand(OnSelectShiftCommandExecuted, CanSelectShiftCommandExecute);
        }
    }
}
