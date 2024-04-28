using Cashbox.Core;
using Cashbox.MVVM.Models;
using System.Windows;
using System.Windows.Media;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class OrderViewModel(Order order) : ViewModelBase
    {
        private readonly Order _order = order;

        public static Order? OrderComposition => Order.OrderComposition;
        public static async Task<bool> SellOrder(int paymet, double sellcost, double discount, List<OrderProductViewModel> orderProducts) => await Order.SellOrder(paymet, sellcost, discount, orderProducts);
        public static async Task<OrderViewModel> CreateOrder() => await Order.CreateOrder();
        public static async Task<OrderViewModel> RemoveCurrentOrder() => await Order.RemoveCurrentOrder();
        public static async Task<List<OrderViewModel>> GetDayOrdersToMethod(DateOnly dateOnly, int method) => await Order.GetDayOrdersToMethod(dateOnly, method);
        public static async Task<List<OrderViewModel>> GetAllDayOrders(DateOnly dateOnly) => await Order.GetAllDayOrders(dateOnly);
        public static async Task<List<OrderViewModel>> GetSellDetail(DateOnly StartData, DateOnly EndData) => await Order.GetSellDetail(StartData, EndData);
        public static async Task<double> GetSumInDay(DateOnly? dateOnly) => await Order.GetSumInDay(dateOnly);
        public static double GetSumMethodInDay(DateOnly? dateOnly) => Order.GetSumMethodInDay(dateOnly);
        public static bool RemoveNullReferenceOrder() => Order.RemoveNullReferenceOrder();


        public int Id => _order.Id;

        public DateTime? SellDatetime
        {
            get => _order.SellDatetime;
            set
            {
                _order.SellDatetime = value;
                OnPropertyChanged();
            }
        }

        public int? PaymentMethodId
        {
            get => _order.PaymentMethodId;
            set
            {
                _order.PaymentMethodId = value;
                OnPropertyChanged();
            }
        }

        public int UserId
        {
            get => _order.UserId;
            set
            {
                _order.UserId = value;
                OnPropertyChanged();
            }
        }

        public int DailyReportId
        {
            get => _order.DailyReportId;
            set
            {
                _order.DailyReportId = value;
                OnPropertyChanged();
            }
        }

        public double? SellCost
        {
            get => _order.SellCost;
            set
            {
                _order.SellCost = value;
                OnPropertyChanged();
            }
        }

        public double? Discount
        {
            get => _order.Discount;
            set
            {
                _order.Discount = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush? BackGroundColor
        {
            get
            {
                if (_order.Discount != 0)
                    return (SolidColorBrush)Application.Current.Resources["BasicCyan"];
                if (_order.OrderProducts!.Where(x => x.SellCost != x.Product.SellCost).Count() > 0)
                    return (SolidColorBrush)Application.Current.Resources["BasicRed"];
                return (SolidColorBrush)Application.Current.Resources["BasicW"];
            }
        }

        public virtual ICollection<OrderProduct> OrderProducts => _order.OrderProducts!;

        public virtual PaymentMethod PaymentMethod
        {
            get => _order.PaymentMethod!;
            set
            {
                _order.PaymentMethod = value;
                OnPropertyChanged();
            }
        }

        public virtual DailyReport DailyReport { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
