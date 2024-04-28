using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.Models;
using Cashbox.MVVM.ViewModels.Data;
using System.Collections.ObjectModel;
using System.Windows;

namespace Cashbox.MVVM.ViewModels.Employee
{
    public class ShiftViewModel : ViewModelBase
    {
        #region Props
        public static UserViewModel? User { get => Models.User.CurrentUser; }

        #region Visibility
        private Visibility _startShiftVisibility = Visibility.Visible;
        public Visibility StartShiftVisibility
        {
            get => _startShiftVisibility;
            set => Set(ref _startShiftVisibility, value);
        }

        private Visibility _processShiftVisibility = Visibility.Collapsed;
        public Visibility ProcessShiftVisibility
        {
            get => _processShiftVisibility;
            set => Set(ref _processShiftVisibility, value);
        }

        private Visibility _processDoShiftVisibility = Visibility.Collapsed;
        public Visibility ProcessDoShiftVisibility
        {
            get => _processDoShiftVisibility;
            set => Set(ref _processDoShiftVisibility, value);
        }

        private Visibility _endShiftVisibility = Visibility.Collapsed;
        public Visibility EndShiftVisibility
        {
            get => _endShiftVisibility;
            set => Set(ref _endShiftVisibility, value);
        }

        private Visibility _checkVisibility = Visibility.Collapsed;
        public Visibility CheckVisibility
        {
            get => _checkVisibility;
            set => Set(ref _checkVisibility, value);
        }

        private Visibility _shiftVisibility = Visibility.Visible;
        public Visibility ShiftVisibility
        {
            get => _shiftVisibility;
            set => Set(ref _shiftVisibility, value);
        }

        private Visibility _ordersListVisibility = Visibility.Visible;
        public Visibility OrdersListVisibility
        {
            get => _ordersListVisibility;
            set => Set(ref _ordersListVisibility, value);
        }

        private Visibility _checkListVisibility = Visibility.Visible;
        public Visibility CheckListVisibility
        {
            get => _checkListVisibility;
            set => Set(ref _checkListVisibility, value);
        }

        private Visibility _checkListOneObjVisibility = Visibility.Collapsed;
        public Visibility CheckListOneObjVisibility
        {
            get => _checkListOneObjVisibility;
            set => Set(ref _checkListOneObjVisibility, value);
        }

        #endregion

        private double _startCash;
        public double StartCash
        {
            get => _startCash;
            set => Set(ref _startCash, value);
        }

        private string? _currentCash;
        public string? CurrentCash
        {
            get => _currentCash;
            set => Set(ref _currentCash, value);
        }

        private double _nalTransit;
        public double NalTransit
        {
            get => _nalTransit;
            set => Set(ref _nalTransit, value);
        }

        private double _sendTransit;
        public double SendTransit
        {
            get => _sendTransit;
            set => Set(ref _sendTransit, value);
        }

        private double _cardTransit;
        public double CardTransit
        {
            get => _cardTransit;
            set => Set(ref _cardTransit, value);
        }

        private double _fullTransit;
        public double FullTransit
        {
            get => _fullTransit;
            set => Set(ref _fullTransit, value);
        }

        private AutoDailyReportViewModel? _autoShift;
        public AutoDailyReportViewModel? AutoShift
        {
            get => _autoShift;
            set => Set(ref _autoShift, value);
        }

        private DailyReportViewModel? _dailyReport;
        public DailyReportViewModel? DailyReport
        {
            get => _dailyReport;
            set => Set(ref _dailyReport, value);
        }

        private OrderViewModel? _selectedOrder;
        public OrderViewModel? SelectedOrder
        {
            get => _selectedOrder;
            set => Set(ref _selectedOrder, value);
        }

        private ObservableCollection<ProductViewModel> _productCollection = [];
        public ObservableCollection<ProductViewModel> ProductCollection
        {
            get => _productCollection;
            set => Set(ref _productCollection, value);
        }

        private ObservableCollection<OrderViewModel> _orderCollection = [];
        public ObservableCollection<OrderViewModel> OrderCollection
        {
            get => _orderCollection;
            set => Set(ref _orderCollection, value);
        }

        private TimeOnly? _startShiftTime;
        public TimeOnly? StartShiftTime
        {
            get => _startShiftTime;
            set => Set(ref _startShiftTime, value);
        }

        private TimeOnly? _endShiftTime;
        public TimeOnly? EndShiftTime
        {
            get => _endShiftTime;
            set => Set(ref _endShiftTime, value);
        }

        private DateOnly _currentDate = DateOnly.FromDateTime(DateTime.Today);
        public DateOnly CurrentDate
        {
            get => _currentDate;
            set => Set(ref _currentDate, value);
        }
        #endregion

        #region Command

        public RelayCommand StartShiftCommand { get; set; }
        private bool CanStartShiftCommandExecute(object p) => true;
        private async void OnStartShiftCommandExecuted(object p)
        {
            StartShiftTime = TimeOnly.FromDateTime(DateTime.Now);
            DailyReportViewModel drvm = await DailyReportViewModel.StartShift(CurrentDate, (TimeOnly)StartShiftTime);
            StartShiftVisibility = Visibility.Collapsed;
            ProcessShiftVisibility = Visibility.Visible;
            ProcessDoShiftVisibility = Visibility.Visible;
            EndShiftVisibility = Visibility.Collapsed;
            AppCommand.InfoMessage($"Смена {drvm.Id} открыта");
        }

        public RelayCommand EndShiftCommand { get; set; }
        private bool CanEndShiftCommandExecute(object p) => true;
        private async void OnEndShiftCommandExecuted(object p)
        {
            if (string.IsNullOrEmpty(CurrentCash))
            {
                AppCommand.WarningMessage("Пересчитайте деньги в кассе и введите значение в поле \"Денег в кассе\"");
                return;
            }
            if (AppCommand.QuestionMessage("После закрытия смены, новую можно открыть только на следующий день. \nВы уверены, что хотите ее закрыть?") == MessageBoxResult.Yes)
            {
                EndShiftTime = TimeOnly.FromDateTime(DateTime.Now);
                DailyReportViewModel drvm = await DailyReportViewModel.EndShift(CurrentDate, EndShiftTime, double.Parse(CurrentCash), UserViewModel.GetCurrentUser().Id);
                AutoDailyReportViewModel adreport = await AutoDailyReportViewModel.GenEndShiftAuto(drvm!);
                StartShiftVisibility = Visibility.Collapsed;
                ProcessShiftVisibility = Visibility.Visible;
                ProcessDoShiftVisibility = Visibility.Collapsed;
                EndShiftVisibility = Visibility.Visible;
                AutoShift = adreport;
                AppCommand.InfoMessage($"Смена {drvm.Id} закрыта");
            }

        }

        public RelayCommand SeeCheckPanelCommand { get; set; }
        private bool CanSeeCheckPanelCommandExecute(object p) => true;
        private void OnSeeCheckPanelCommandExecuted(object p)
        {
            ShiftVisibility = Visibility.Collapsed;
            CheckVisibility = Visibility.Visible;
        }

        public RelayCommand SeeShiftPanelCommand { get; set; }
        private bool CanSeeShiftPanelCommandExecute(object p) => true;
        private void OnSeeShiftPanelCommandExecuted(object p)
        {
            ShiftVisibility = Visibility.Visible;
            CheckVisibility = Visibility.Collapsed;
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
            ProductCollection = [];
            SelectedOrder = OrderCollection.FirstOrDefault(x => x.Id == (int)p)!;
            foreach (OrderProduct item in SelectedOrder.OrderProducts)
            {
                ProductViewModel product = new(item.Product);
                if (item.SellCost != product.SellCost)
                    product.ReSellCost = item.SellCost.ToString();
                product.AmountRes = item.Amount;
                ProductCollection.Add(product);
            }
            CheckListVisibility = Visibility.Collapsed;
            CheckListOneObjVisibility = Visibility.Visible;
        }

        #endregion

        public override async void OnLoad()
        {
            DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Today);
            CardTransit = (await OrderViewModel.GetDayOrdersToMethod(dateOnly, 1)).Sum(x => (double)x.SellCost!);
            NalTransit = (await OrderViewModel.GetDayOrdersToMethod(dateOnly, 2)).Sum(x => (double)x.SellCost!);
            SendTransit = (await OrderViewModel.GetDayOrdersToMethod(dateOnly, 3)).Sum(x => (double)x.SellCost!);
            DailyReportViewModel drvm = DailyReportViewModel.GetCurrentShift();
            if (drvm != null)
            {
                OrderCollection = new(await OrderViewModel.GetAllDayOrders((DateOnly)drvm.Data));
                StartCash = drvm.CashOnStart;
            }
            else
                StartCash = MoneyBoxViewModel.GetMoney;
            FullTransit = SendTransit + CardTransit + NalTransit;
            if (StartShiftVisibility == Visibility.Collapsed)
                OrderCollection = new(Order.GetAllDayOrders(dateOnly).Result);
        }

        public ShiftViewModel()
        {
            SeeOrderListCommand = new RelayCommand(OnSeeOrderListCommandExecuted, CanSeeOrderListCommandExecute);
            GoBackOrderCommand = new RelayCommand(OnGoBackOrderCommandExecuted, CanGoBackOrderCommandExecute);
            StartShiftCommand = new RelayCommand(OnStartShiftCommandExecuted, CanStartShiftCommandExecute);
            EndShiftCommand = new RelayCommand(OnEndShiftCommandExecuted, CanEndShiftCommandExecute);
            SeeCheckPanelCommand = new RelayCommand(OnSeeCheckPanelCommandExecuted, CanSeeCheckPanelCommandExecute);
            SeeShiftPanelCommand = new RelayCommand(OnSeeShiftPanelCommandExecuted, CanSeeShiftPanelCommandExecute);
            DailyReport = DailyReportViewModel.GetCurrentShift();
            if (DailyReport != null)
            {
                StartShiftTime = DailyReport.OpenTime;
                StartShiftVisibility = Visibility.Collapsed;
                ProcessShiftVisibility = Visibility.Visible;
                if (DailyReport.CloseTime != null)
                {
                    EndShiftTime = DailyReport.CloseTime;
                    ProcessDoShiftVisibility = Visibility.Collapsed;
                    EndShiftVisibility = Visibility.Visible;
                    AutoShift = DailyReport.AutoDreportVM!;
                }
                else
                {
                    ProcessDoShiftVisibility = Visibility.Visible;
                    EndShiftVisibility = Visibility.Collapsed;
                }
            }
        }
    }
}
