using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.Models;
using Cashbox.MVVM.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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

        private ObservableCollection<ProductViewModel> _selectedOrderProduct = [];
        public ObservableCollection<ProductViewModel> SelectedOrderProduct
        {
            get => _selectedOrderProduct;
            set => Set(ref _selectedOrderProduct, value);
        }

        private DateTime _endDate = DateTime.Today;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (value < StartDate)
                {
                    AppCommand.WarningMessage("Дата конца не может быть меньше даты начала");
                    return;
                }
                _endDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime _startDate = DateTime.Today.AddDays(-30);
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (value > EndDate)
                {
                    AppCommand.WarningMessage("Дата начала не может быть больше даты конца");
                    return;
                }
                _startDate = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ProductViewModel> _productCollection = [];
        public ObservableCollection<ProductViewModel> ProductCollection
        {
            get => _productCollection;
            set => Set(ref _productCollection, value);
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
            dataload();
        }

        public RelayCommand SeeOrderListCommand { get; set; }
        private bool CanSeeOrderListCommandExecute(object p) => true;
        private void OnSeeOrderListCommandExecuted(object p)
        {
            if (p == null) return;
            SelectedOrderProduct = [];
            SelectedOrder = SelectedOrderList.FirstOrDefault(x => x.Id == (int)p)!;
            foreach (OrderProduct item in SelectedOrder.OrderProducts)
            {
                ProductViewModel product = new(item.Product);
                SelectedOrderProduct.Add(product);
            }
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

        public override async void OnLoad()
        {
            ProductCollection = new(await ProductViewModel.GetProducts(true));
            dataload();
        }

        public async void dataload()
        {
            List<DailyReportViewModel> data = await DailyReportViewModel.GetPeriodReports(DateOnly.FromDateTime(StartDate), DateOnly.FromDateTime(EndDate));
            DailyReportCollection = new(data.OrderByDescending(x => x.Data));
        }

        public ShiftViewModel()
        {
            SearchDataCommand = new RelayCommand(OnSearchDataCommandExecuted, CanSearchDataCommandExecute);
            SeeOrderListCommand = new RelayCommand(OnSeeOrderListCommandExecuted, CanSeeOrderListCommandExecute);
            SelectShiftCommand = new RelayCommand(OnSelectShiftCommandExecuted, CanSelectShiftCommandExecute);
        }
    }
}
