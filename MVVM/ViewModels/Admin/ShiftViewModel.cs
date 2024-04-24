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
using System.Windows.Documents;

namespace Cashbox.MVVM.ViewModels.Admin
{
    public class ShiftViewModel : ViewModelBase
    {
        #region Props

        private Visibility _orderSelectedPanel = Visibility.Collapsed;
        public Visibility OrderSelectedPanel
        {
            get => _orderSelectedPanel;
            set => Set(ref _orderSelectedPanel, value);
        }

        private Visibility _refundSelectedPanel = Visibility.Collapsed;
        public Visibility RefundSelectedPanel
        {
            get => _refundSelectedPanel;
            set => Set(ref _refundSelectedPanel, value);
        }

        private Visibility _refundSuccessSelectedPanel = Visibility.Collapsed;
        public Visibility RefundSuccessSelectedPanel
        {
            get => _refundSuccessSelectedPanel;
            set => Set(ref _refundSuccessSelectedPanel, value);
        }

        private Visibility _refundListVisibilityPanel = Visibility.Collapsed;
        public Visibility RefundListVisibilityPanel
        {
            get => _refundListVisibilityPanel;
            set => Set(ref _refundListVisibilityPanel, value);
        }

        private Visibility _ordersListVisibility = Visibility.Visible;
        public Visibility OrdersListVisibility
        {
            get => _ordersListVisibility;
            set => Set(ref _ordersListVisibility, value);
        }

        private bool _checkBool = true;
        public bool CheckBool
        {
            get => _checkBool;
            set 
            {
                _checkBool = value;
                OrdersListVisibility = Visibility.Collapsed;
                if (value)
                    OrdersListVisibility = Visibility.Visible;
                OnPropertyChanged();
            }
        }

        private bool _refundBool = false;
        public bool RefundBool
        {
            get => _refundBool;
            set
            {
                _refundBool = value;
                RefundListVisibilityPanel = Visibility.Collapsed;
                if (value)
                    RefundListVisibilityPanel = Visibility.Visible;
                OnPropertyChanged();
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

        private string _search = string.Empty;
        public string Search
        {
            get => _search;
            set => Set(ref _search, value);
        }

        private ObservableCollection<RefundViewModel> _unSuccessRefundCollection = [];
        public ObservableCollection<RefundViewModel> UnSuccessRefundCollection
        {
            get => _unSuccessRefundCollection;
            set => Set(ref _unSuccessRefundCollection, value);
        }

        private ObservableCollection<RefundViewModel> _successRefundCollection = [];
        public ObservableCollection<RefundViewModel> SuccessRefundCollection
        {
            get => _successRefundCollection;
            set => Set(ref _successRefundCollection, value);
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

        private ObservableCollection<RefundViewModel> _dailyRefundCollection = [];
        public ObservableCollection<RefundViewModel> DailyRefundCollection
        {
            get => _dailyRefundCollection;
            set => Set(ref _dailyRefundCollection, value);
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
        private void OnSearchDataCommandExecuted(object p)
        {
            Dataload(Search);
        }

        public RelayCommand GoBackOrderCommand { get; set; }
        private bool CanGoBackOrderCommandExecute(object p) => true;
        private void OnGoBackOrderCommandExecuted(object p)
        {
            SelectedOrder = null;
            CheckListOneObjVisibility = Visibility.Collapsed;
            CheckListVisibility = Visibility.Visible;
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
                if (item.SellCost != product.SellCost)
                    product.ReSellCost = item.SellCost.ToString();
                product.AmountRes = item.Amount;
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
            OrderSelectedPanel = Visibility.Visible;
            RefundSelectedPanel = Visibility.Collapsed;
            RefundSuccessSelectedPanel = Visibility.Collapsed;
            SelectedOrderList = new(await OrderViewModel.GetAllDayOrders((DateOnly)SelectedDReport.Data!));
            DailyRefundCollection = new(await RefundViewModel.GetRefundedDailyProduct(SelectedDReport.Id));
            CheckListOneObjVisibility = Visibility.Collapsed;
            CheckListVisibility = Visibility.Visible;
        }

        public RelayCommand SeeRefundsCommand { get; set; }
        private bool CanSeeRefundsCommandExecute(object p) => true;
        private void OnSeeRefundsCommandExecuted(object p)
        {
            OrderSelectedPanel = Visibility.Visible;
            RefundSelectedPanel = Visibility.Collapsed;
            RefundSuccessSelectedPanel = Visibility.Collapsed;
        }

        public RelayCommand OpenUnRefundCommand { get; set; }
        private bool CanOpenUnRefundCommandExecute(object p) => true;
        private void OnOpenUnRefundCommandExecuted(object p)
        {
            OrderSelectedPanel = Visibility.Collapsed;
            RefundSelectedPanel = Visibility.Visible;
            RefundSuccessSelectedPanel = Visibility.Collapsed;
        }

        public RelayCommand OpenRefundCommand { get; set; }
        private bool CanOpenRefundCommandExecute(object p) => true;
        private void OnOpenRefundCommandExecuted(object p)
        {
            OrderSelectedPanel = Visibility.Collapsed;
            RefundSelectedPanel = Visibility.Collapsed;
            RefundSuccessSelectedPanel = Visibility.Visible;
        }

        public RelayCommand ChangeRefundSuccessCommand { get; set; }
        private bool CanChangeRefundSuccessCommandExecute(object p) => true;
        private async void OnChangeRefundSuccessCommandExecuted(object p)
        {
            if (!await RefundViewModel.SuccessRefund())
            {
                AppCommand.ErrorMessage("Не удалось подтвердить возврат");
                return;
            }
            AppCommand.InfoMessage("Успех");
            UpdateRefund();
        }

        #endregion

        public override async void OnLoad()
        {
            ProductCollection = new(await ProductViewModel.GetProducts(true));
            Dataload();
        }

        public async void Dataload(string search = "")
        {
            List<DailyReportViewModel> data = await DailyReportViewModel.GetPeriodReports(DateOnly.FromDateTime(StartDate), DateOnly.FromDateTime(EndDate));
            if (search == "")
                DailyReportCollection = new(data.OrderByDescending(x => x.Data));
            else
                DailyReportCollection = new(data.Where(x => x.UserInfoVM.FullName.Contains(search.ToLower().Trim())).OrderByDescending(x => x.Data));
        }

        public async void UpdateRefund()
        {
            List<RefundViewModel> list = await RefundViewModel.GetRefundedAllProduct();
            UnSuccessRefundCollection = new(list.Where(x => x.IsSuccessRefund == false && x.BuyDate == null).OrderByDescending(x => x.DailyReport.Data).ToList());
            SuccessRefundCollection = new(list.Where(x => x.IsSuccessRefund == true && x.BuyDate == null).OrderByDescending(x => x.DailyReport.Data).ToList());
        }


        public ShiftViewModel()
        {
            Dataload();
            UpdateRefund();
            OpenUnRefundCommand = new RelayCommand(OnOpenUnRefundCommandExecuted, CanOpenUnRefundCommandExecute);
            OpenRefundCommand = new RelayCommand(OnOpenRefundCommandExecuted, CanOpenRefundCommandExecute);
            SeeRefundsCommand = new RelayCommand(OnSeeRefundsCommandExecuted, CanSeeRefundsCommandExecute);
            SearchDataCommand = new RelayCommand(OnSearchDataCommandExecuted, CanSearchDataCommandExecute);
            SeeOrderListCommand = new RelayCommand(OnSeeOrderListCommandExecuted, CanSeeOrderListCommandExecute);
            SelectShiftCommand = new RelayCommand(OnSelectShiftCommandExecuted, CanSelectShiftCommandExecute);
            GoBackOrderCommand = new RelayCommand(OnGoBackOrderCommandExecuted, CanGoBackOrderCommandExecute);
            ChangeRefundSuccessCommand = new RelayCommand(OnChangeRefundSuccessCommandExecuted, CanChangeRefundSuccessCommandExecute);
        }
    }
}
