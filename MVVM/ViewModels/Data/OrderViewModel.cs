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

        public static async Task<List<OrderViewModel>> GetDayOrdersToMethod(DateTime dateTime, int method) => await Order.GetDayOrdersToMethod(dateTime, method);
        public static async Task<List<OrderViewModel>> GetAllDayOrders(DateTime dateTime) => await Order.GetAllDayOrders(dateTime);

        public int Id => _order.Id;

        public DateTime SellDatetime 
        {
            get => _order.SellDatetime;
            set
            {
                _order.SellDatetime = value;
                OnPropertyChanged();
            }
        }

        public int PaymentMethodId
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

        public double SellCost
        {
            get => _order.SellCost;
            set
            {
                _order.SellCost = value;
                OnPropertyChanged();
            }
        }

        public double Discount
        {
            get => _order.Discount;
            set
            {
                _order.Discount = value;
                OnPropertyChanged();
            }
        }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

        public virtual PaymentMethod PaymentMethod { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
