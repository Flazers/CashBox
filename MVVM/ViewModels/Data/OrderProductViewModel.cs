using Cashbox.Core;
using Cashbox.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class OrderProductViewModel(OrderProduct orderProduct) : ViewModelBase
    {
        private readonly OrderProduct _orderProduct = orderProduct;

        public static List<OrderProduct> OrderProducts => OrderProduct.OrderProducts;
        public static void AddListOrderInDataBase(List<OrderProductViewModel> orderProducts) => OrderProduct.AddListOrderInDataBase(orderProducts);
        public static async Task<List<OrderProductViewModel>> GetInOrderProduct(int OrderId) => await OrderProduct.GetInOrderProduct(OrderId);

        public int Id => _orderProduct.Id;

        public int OrderId
        {
            get => _orderProduct.OrderId;
            set
            {
                _orderProduct.OrderId = value;
                OnPropertyChanged();
            }
        }
        public int ProductId
        {
            get => _orderProduct.ProductId;
            set
            {
                _orderProduct.ProductId = value;
                OnPropertyChanged();
            }
        }
        public int Amount
        {
            get => _orderProduct.Amount;
            set
            {
                _orderProduct.Amount = value;
                OnPropertyChanged();
            }
        }
        public double PurchaseСost
        {
            get => _orderProduct.PurchaseСost;
            set
            {
                _orderProduct.PurchaseСost = value;
                OnPropertyChanged();
            }
        }
        public double SellCost
        {
            get => _orderProduct.SellCost;
            set
            {
                _orderProduct.SellCost = value;
                OnPropertyChanged();
            }
        }

        public virtual Order Order { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;

        public ProductViewModel ProductVM { get; set; } = null!;

    }
}
