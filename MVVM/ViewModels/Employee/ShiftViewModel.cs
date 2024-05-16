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

        private double _startCash = AppSettingsViewModel.Settings.StartCash;
        public double StartCash
        {
            get => _startCash;
            set => Set(ref _startCash, value);
        }

        private string? _currentCash = string.Empty;
        public string? CurrentCash
        {
            get => _currentCash;
            set => Set(ref _currentCash, value);
        }

        private double _nalTransit = 0;
        public double NalTransit
        {
            get => _nalTransit;
            set => Set(ref _nalTransit, value);
        }

        private double _sendTransit = 0;
        public double SendTransit
        {
            get => _sendTransit;
            set => Set(ref _sendTransit, value);
        }

        private double _cardTransit = 0;
        public double CardTransit
        {
            get => _cardTransit;
            set => Set(ref _cardTransit, value);
        }

        private double _fullTransit = 0;
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

        private DailyReportViewModel? _dailyReportVMobj = DailyReportViewModel.GetCurrentShift();
        public DailyReportViewModel? DailyReportVMobj
        {
            get => _dailyReportVMobj;
            set => Set(ref _dailyReportVMobj, value);
        }

        private OrderViewModel? _selectedOrder;
        public OrderViewModel? SelectedOrder
        {
            get => _selectedOrder;
            set => Set(ref _selectedOrder, value);
        }

        private ObservableCollection<ProductViewModel?> _productCollection = [];
        public ObservableCollection<ProductViewModel?> ProductCollection
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
            set
            {
                _startShiftTime = value;
                StartShiftTimeString = value.ToString();
                OnPropertyChanged();
            }
        }

        private TimeOnly? _endShiftTime;
        public TimeOnly? EndShiftTime
        {
            get => _endShiftTime;
            set
            {
                _endShiftTime = value;
                EndShiftTimeString = value.ToString();
                OnPropertyChanged();
            }
        }

        private string? _startShiftTimeString;
        public string? StartShiftTimeString
        {
            get => _startShiftTimeString;
            set => Set(ref _startShiftTimeString, value);
        }

        private string? _endShiftTimeString;
        public string? EndShiftTimeString
        {
            get => _endShiftTimeString;
            set => Set(ref _endShiftTimeString, value);
        }


        private DateOnly _currentDate = DateOnly.FromDateTime(DateTime.Today);
        public DateOnly CurrentDate
        {
            get => _currentDate;
            set => Set(ref _currentDate, value);
        }

        private int _salary = 0;
        public int Salary
        {
            get => _salary;
            set => Set(ref _salary, value);
        }

        private int _award = 0;
        public int Award
        {
            get => _award;
            set => Set(ref _award, value);
        }
        #endregion

        #region Command

        public RelayCommand StartShiftCommand { get; set; }
        private bool CanStartShiftCommandExecute(object p) => true;
        private async void OnStartShiftCommandExecuted(object p)
        {
            DailyReportViewModel dailyReport = await DailyReportViewModel.GetReport(CurrentDate);
            if (dailyReport != null)
            {
                AppCommand.WarningMessage($"Смена открыта и завершена сотрудником {dailyReport.UserInfoVM.FullName}");
                return;
            }
            StartShiftTime = TimeOnly.FromDateTime(DateTime.Now);
            DailyReportVMobj = await DailyReportViewModel.StartShift(CurrentDate, (TimeOnly)StartShiftTime);
            StartShiftVisibility = Visibility.Collapsed;
            ProcessShiftVisibility = Visibility.Visible;
            ProcessDoShiftVisibility = Visibility.Visible;
            EndShiftVisibility = Visibility.Collapsed;
            AppCommand.InfoMessage($"Смена {DailyReportVMobj.Id} открыта");
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
                DailyReportViewModel drvm = await DailyReportViewModel.EndShift(CurrentDate, EndShiftTime, int.Parse(CurrentCash), UserViewModel.GetCurrentUser().Id);
                AutoDailyReportViewModel adreport = await AutoDailyReportViewModel.GenEndShiftAuto(drvm!);
                StartShiftVisibility = Visibility.Collapsed;
                ProcessShiftVisibility = Visibility.Visible;
                ProcessDoShiftVisibility = Visibility.Collapsed;
                EndShiftVisibility = Visibility.Visible;
                AutoShift = adreport;
                AppCommand.InfoMessage($"Смена {drvm.Id} закрыта в {EndShiftTimeString}");
            }
        }

        public RelayCommand TakeSalaryCommand { get; set; }
        private bool CanTakeSalaryCommandExecute(object p) => true;
        private async void OnTakeSalaryCommandExecuted(object p)
        {
            if (DailyReportVMobj.UserId != UserViewModel.GetCurrentUser().Id)
            {
                AppCommand.WarningMessage("Вы не можете собрать зарплату за другого человека.");
                return;
            }
            if (DailyReportVMobj.TakedSalary)
            {
                AppCommand.WarningMessage("Зарплата уже собрана.");
                return;
            }
            if (Salary == 0)
            {
                AppCommand.WarningMessage("Укажите значение > 0");
                return;
            }
            if (!await DailyReportViewModel.TakeSalary(DailyReportVMobj, Award, Salary))
            {
                AppCommand.ErrorMessage("Не удалось получить зарплату");
                return;
            }
            Salary = AutoShift.Salary;
            Award = AutoShift.Award;
            AppCommand.InfoMessage("Готово");

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
            CurrentDate = DateOnly.FromDateTime(DateTime.Today);
            DailyReportViewModel dailyReport = await DailyReportViewModel.GetReport(CurrentDate);
            if (dailyReport == null)
            {
                StartShiftVisibility = Visibility.Visible;
                ProcessShiftVisibility = Visibility.Collapsed;
                ProcessDoShiftVisibility = Visibility.Collapsed;
                EndShiftVisibility = Visibility.Collapsed;
            }
            else if (DailyReportVMobj != null)
            {
                StartShiftTime = DailyReportVMobj.OpenTime;
                StartShiftVisibility = Visibility.Collapsed;
                ProcessShiftVisibility = Visibility.Visible;
                if (DailyReportVMobj.CloseTime != null)
                {
                    EndShiftTime = DailyReportVMobj.CloseTime;
                    ProcessDoShiftVisibility = Visibility.Collapsed;
                    EndShiftVisibility = Visibility.Visible;
                    AutoShift = DailyReportVMobj.AutoDreportVM!;
                }
                else
                {
                    ProcessDoShiftVisibility = Visibility.Visible;
                    EndShiftVisibility = Visibility.Collapsed;
                }
            }
            DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Today);
            CardTransit = (await OrderViewModel.GetDayOrdersToMethod(dateOnly, 1)).Sum(x => (double)x.SellCostWithDiscount!);
            NalTransit = (await OrderViewModel.GetDayOrdersToMethod(dateOnly, 2)).Sum(x => (double)x.SellCostWithDiscount!);
            SendTransit = (await OrderViewModel.GetDayOrdersToMethod(dateOnly, 3)).Sum(x => (double)x.SellCostWithDiscount!);
            FullTransit = SendTransit + CardTransit + NalTransit;
            DailyReportViewModel drvm = DailyReportViewModel.GetCurrentShift();
            if (drvm != null)
            {
                List<OrderViewModel> orders = await OrderViewModel.GetAllDayOrders((DateOnly)drvm.Data!);
                OrderCollection = new([.. orders.OrderBy(x => x.PaymentMethodId)]);
            }

        }

        public ShiftViewModel()
        {
            TakeSalaryCommand = new RelayCommand(OnTakeSalaryCommandExecuted, CanTakeSalaryCommandExecute);
            SeeOrderListCommand = new RelayCommand(OnSeeOrderListCommandExecuted, CanSeeOrderListCommandExecute);
            GoBackOrderCommand = new RelayCommand(OnGoBackOrderCommandExecuted, CanGoBackOrderCommandExecute);
            StartShiftCommand = new RelayCommand(OnStartShiftCommandExecuted, CanStartShiftCommandExecute);
            EndShiftCommand = new RelayCommand(OnEndShiftCommandExecuted, CanEndShiftCommandExecute);
            SeeCheckPanelCommand = new RelayCommand(OnSeeCheckPanelCommandExecuted, CanSeeCheckPanelCommandExecute);
            SeeShiftPanelCommand = new RelayCommand(OnSeeShiftPanelCommandExecuted, CanSeeShiftPanelCommandExecute);
        }
    }
}
