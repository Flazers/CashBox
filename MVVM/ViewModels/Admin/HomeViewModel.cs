using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.ViewModels.Data;
using System.Collections.ObjectModel;
using System.Windows;

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

        private double _cashInBox = MoneyBoxViewModel.GetMoney;
        public double CashInBox
        {
            get => _cashInBox;
            set => Set(ref _cashInBox, value);
        }

        private double _newCashInBox;
        public double NewCashInBox
        {
            get
            {
                if (_cashInBox <= 1500) 
                    return _newCashInBox;
                return _cashInBox - 1500;
            }
            set => Set(ref _newCashInBox, value);
        }

        #endregion

        #region Commands

        public RelayCommand InEditMoneyBoxCommand { get; set; }
        private bool CanInEditMoneyBoxCommandExecute(object p) => true;
        private async void OnInEditMoneyBoxCommandExecuted(object p)
        {
            double temp = NewCashInBox;
            await MoneyBoxViewModel.UpdateMoney(NewCashInBox, 1);
            MessageBox.Show($"{temp} ₽ внесено в кассу", "Успех");
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
            await MoneyBoxViewModel.UpdateMoney(NewCashInBox, 2);
            MessageBox.Show($"{temp} ₽ вычтено из кассы", "Успех");
            NewCashInBox = 0;
            CashInBox = MoneyBoxViewModel.GetMoney;
        }

        public RelayCommand GetDetailCommand { get; set; }
        private bool CanGetDetailCommandExecute(object p) => true;
        private async void OnGetDetailCommandExecuted(object p)
        {
            Details.Clear();
            List<ProductCategoryViewModel> productCategory = await ProductCategoryViewModel.GetProductCategory();
            List<ProductViewModel> product = await ProductViewModel.GetProducts(true);
            foreach (var category in productCategory)
                Details.Add(new()
                {
                    Category = category.Category,
                    Money = 0
                });
            foreach (var orderProduct in await OrderProductViewModel.GetOrderProduct(StartDate, EndDate))
            {
                var selectedproduct = product.FirstOrDefault(x => x.Id == orderProduct.ProductId);
                Details.FirstOrDefault(x => x.Category == selectedproduct.Category.Category).Money += selectedproduct.SellCost * orderProduct.Amount;
            }
        }


        #endregion

        public override async void OnLoad()
        {
            //var data = Order.GetSellDetail(DateOnly.Parse("2.04.2024"), DateOnly.Parse("5.04.2024"));
            AuthHistory = new(await AuthHistoryViewModel.GetAuthHistories());
            AuthHistory = new(AuthHistory.TakeLast(3).OrderByDescending(x => x.Datetime).ToList());

        }

        public HomeViewModel()
        {
            InEditMoneyBoxCommand = new RelayCommand(OnInEditMoneyBoxCommandExecuted, CanInEditMoneyBoxCommandExecute);
            OutEditMoneyBoxCommand = new RelayCommand(OnOutEditMoneyBoxCommandExecuted, CanOutEditMoneyBoxCommandExecute);
            GetDetailCommand = new RelayCommand(OnGetDetailCommandExecuted, CanGetDetailCommandExecute);
        }
    }
}
