using Cashbox.Core;
using Cashbox.MVVM.Models;
using MahApps.Metro.IconPacks;
using System.Windows;
using System.Windows.Media;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class OrderViewModel(Order order) : ViewModelBase
    {
        private readonly Order _order = order;

        public static Order? OrderComposition => Order.OrderComposition;
        public static async Task<bool> SellOrder(int paymet, double sellcost, int discount, List<OrderProductViewModel> orderProducts) => await Order.SellOrder(paymet, sellcost, discount, orderProducts);
        public static async Task<OrderViewModel> CreateOrder() => await Order.CreateOrder();
        public static async Task<OrderViewModel> RemoveCurrentOrder() => await Order.RemoveCurrentOrder();
        public static async Task<List<OrderViewModel>> GetDayOrdersToMethod(DateOnly dateOnly, int method) => await Order.GetDayOrdersToMethod(dateOnly, method);
        public static async Task<List<OrderViewModel>> GetAllDayOrders(DateOnly dateOnly) => await Order.GetAllDayOrders(dateOnly);
        public static async Task<List<OrderViewModel>> GetSellDetail(DateOnly StartData, DateOnly EndData) => await Order.GetSellDetail(StartData, EndData);
        public static async Task<double> GetSumInDay(DateOnly? dateOnly) => await Order.GetSumInDay(dateOnly);
        public static double GetSumMethodInDay(DateOnly? dateOnly) => Order.GetSumMethodInDay(dateOnly);
        public static async Task<bool> RemoveNullReferenceOrder() => await Order.RemoveNullReferenceOrder();


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

        public Visibility DiscountVisibility
        {
            get
            {
                if (Discount != 0)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        public double? SellCostWithDiscount
        {
            get => _order.SellCost - (_order.SellCost / 100 * _order.Discount);
        }

        public SolidColorBrush? BackGroundColor
        {
            get
            {
                if (_order.OrderProducts!.Where(x => x.SellCost != x.Product.SellCost).Any())
                    if (_order.Discount != 0)
                        return (SolidColorBrush)Application.Current.Resources["BasicGray"];
                    else
                        return (SolidColorBrush)Application.Current.Resources["BasicRed"];
                if (_order.Discount != 0)
                    return (SolidColorBrush)Application.Current.Resources["BasicCyan"];
                return (SolidColorBrush)Application.Current.Resources["BasicW"];
            }
        }
        public PackIconMaterialKind Kind
        {
            get => _order.PaymentMethodId switch
            {
                1 => PackIconMaterialKind.SmartCard,
                2 => PackIconMaterialKind.Cash,
                3 => PackIconMaterialKind.CubeSend,
                _ => PackIconMaterialKind.Close,
            };
            
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
