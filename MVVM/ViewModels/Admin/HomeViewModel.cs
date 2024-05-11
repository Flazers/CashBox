using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.Models;
using Cashbox.MVVM.ViewModels.Data;
using System.Collections.ObjectModel;

namespace Cashbox.MVVM.ViewModels.Admin
{
    public class Detail
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public double Money { get; set; }
    }

    public class HomeViewModel : ViewModelBase
    {

        #region Props

        private ObservableCollection<Detail> _details = [];
        public ObservableCollection<Detail> Details
        {
            get => _details;
            set => Set(ref _details, value);
        }

        private double _income = 0;
        public double Income
        {
            get => _income;
            set => Set(ref _income, value);
        }

        private double _expensesProduct = 0;
        public double ExpensesProduct
        {
            get => _expensesProduct;
            set => Set(ref _expensesProduct, value);
        }

        private double _expensesDiscount = 0;
        public double ExpensesDiscount
        {
            get => _expensesDiscount;
            set => Set(ref _expensesDiscount, value);
        }

        private double _expensesSalary = 0;
        public double ExpensesSalary
        {
            get => _expensesSalary;
            set => Set(ref _expensesSalary, value);
        }

        private double _expenses = 0;
        public double Expenses
        {
            get => _expenses;
            set => Set(ref _expenses, value);
        }

        private double _profit = 0;
        public double Profit
        {
            get => _profit;
            set => Set(ref _profit, value);
        }
        

        private ObservableCollection<AuthHistoryViewModel>? _authHistory;
        public ObservableCollection<AuthHistoryViewModel>? AuthHistory
        {
            get => _authHistory;
            set => Set(ref _authHistory, value);
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

        private int _cashInBox = MoneyBoxViewModel.GetMoney;
        public int CashInBox
        {
            get => _cashInBox;
            set => Set(ref _cashInBox, value);
        }

        private int _newCashInBox = 0;
        public int NewCashInBox
        {
            get => _newCashInBox;
            set => Set(ref _newCashInBox, value);
        }

        #endregion

        #region Commands

        public RelayCommand InEditMoneyBoxCommand { get; set; }
        private bool CanInEditMoneyBoxCommandExecute(object p) => true;
        private async void OnInEditMoneyBoxCommandExecuted(object p)
        {
            double temp = NewCashInBox;
            if (temp <= 0)
            {
                AppCommand.WarningMessage("Значение должно быть больше 0");
                return;
            }
            await MoneyBoxViewModel.UpdateMoney(NewCashInBox, 1);
            UserViewModel user = UserViewModel.GetCurrentUser();
            await AdminMoneyLogViewModel.CreateTransitMB($"Администратор (id: {user.Id}) {user.UserInfo.ShortName} внес в кассу {temp} ₽", temp);
            AppCommand.InfoMessage($"{temp} ₽ внесено в кассу");
            NewCashInBox = 0;
            CashInBox = MoneyBoxViewModel.GetMoney;
        }

        public RelayCommand OutEditMoneyBoxCommand { get; set; }
        private bool CanOutEditMoneyBoxCommandExecute(object p)
        {
            if (CashInBox == 0)
                return false;
            return true;
        }
        private async void OnOutEditMoneyBoxCommandExecuted(object p)
        {
            double temp = NewCashInBox;
            if (temp > CashInBox)
            {
                AppCommand.WarningMessage("Вы не можете забрать больше денег, чем есть в кассе");
                return;
            }
            if (temp <= 0)
            {
                AppCommand.WarningMessage("Значение должно быть больше 0");
                return;
            }
            await MoneyBoxViewModel.UpdateMoney(NewCashInBox, 2);
            UserViewModel user = UserViewModel.GetCurrentUser();
            await AdminMoneyLogViewModel.CreateTransitMB($"Администратор (id: {user.Id}) {user.UserInfo.ShortName} забрал из кассы {temp} ₽", temp);
            AppCommand.InfoMessage($"{temp} ₽ вычтено из кассы");
            NewCashInBox = 0;
            CashInBox = MoneyBoxViewModel.GetMoney;
        }

        public RelayCommand GetDetailCommand { get; set; }
        private bool CanGetDetailCommandExecute(object p) => true;
        private async void OnGetDetailCommandExecuted(object p)
        {
            Details.Clear();
            Expenses = 0;
            Income = 0;
            ExpensesProduct = 0;
            ExpensesSalary = 0;
            Profit = 0;
            ExpensesDiscount = 0;

            List<ProductCategoryViewModel> productCategory = await ProductCategoryViewModel.GetProductCategory();
            productCategory.Remove(productCategory[0]);
            List<ProductViewModel> product = await ProductViewModel.GetProducts(true);
            foreach (var category in productCategory)
                Details.Add(new()
                {
                    Category = category.Category,
                    Money = 0
                });
            List<OrderProductViewModel> ordersProd = await OrderProductViewModel.GetOrderProduct(StartDate, EndDate);
            List<OrderViewModel> orders = await OrderViewModel.GetSellDetail(DateOnly.FromDateTime(StartDate), DateOnly.FromDateTime(EndDate));
            foreach (var order in orders)
                ExpensesDiscount += order.SellCostWithDiscount;
            foreach (var orderProduct in ordersProd)
            {
                double allcost = orderProduct.SellCost * orderProduct.Amount;
                Details.FirstOrDefault(x => x.Category == orderProduct.Product.Category.Category).Money += allcost;
                Income += allcost;
            }
            List<DailyReportViewModel> dailyReports = await DailyReportViewModel.GetPeriodReports(DateOnly.FromDateTime(StartDate), DateOnly.FromDateTime(EndDate));
            foreach (var item in dailyReports)
                ExpensesSalary += item.AutoDreportVM.Salary + item.AutoDreportVM.Award;
            List<ComingProductViewModel> comingProducts = await ComingProductViewModel.GetComingFromData(StartDate, EndDate);
            foreach (var item in comingProducts)
                ExpensesProduct += item.BuyCost;
            ExpensesDiscount = Math.Round(Income - ExpensesDiscount, 1);
            Expenses = ExpensesSalary + ExpensesProduct;
            Profit = Income - Expenses;
        }


        #endregion

        public override async void OnLoad()
        {
            NewCashInBox = CashInBox;
            List<AuthHistoryViewModel> authHistoryViewModels = await AuthHistoryViewModel.GetAuthHistories();
            AuthHistory = new([.. authHistoryViewModels.TakeLast(3).OrderByDescending(x => x.Datetime)]);
        }

        public HomeViewModel()
        {
            InEditMoneyBoxCommand = new RelayCommand(OnInEditMoneyBoxCommandExecuted, CanInEditMoneyBoxCommandExecute);
            OutEditMoneyBoxCommand = new RelayCommand(OnOutEditMoneyBoxCommandExecuted, CanOutEditMoneyBoxCommandExecute);
            GetDetailCommand = new RelayCommand(OnGetDetailCommandExecuted, CanGetDetailCommandExecute);
        }
    }
}
