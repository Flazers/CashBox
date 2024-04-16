using Cashbox.Core;
using Cashbox.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static double GetSumInDay(DateOnly? dateOnly) => Order.GetSumInDay(dateOnly);
        public static double GetSumMethodInDay(DateOnly? dateOnly) => Order.GetSumMethodInDay(dateOnly);
        
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

        public virtual ICollection<OrderProduct> OrderProducts => _order.OrderProducts!;

        public virtual PaymentMethod PaymentMethod { get; set; } = null!;

        public virtual DailyReport DailyReport { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
