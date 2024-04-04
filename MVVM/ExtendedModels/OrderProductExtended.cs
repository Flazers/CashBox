using Cashbox.MVVM.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.Models
{
    public partial class OrderProduct
    {
        public OrderProduct() { }

        private static List<OrderProduct> _orderProducts = [];
        public static List<OrderProduct> OrderProducts
        {
            get => _orderProducts;
            set => _orderProducts = value;
        }

        public static void AddProduct(List<OrderProductViewModel> orderProducts)
        {
            foreach (var item in orderProducts) 
            {
                OrderProducts.Add(new OrderProduct()
                {
                    OrderId = item.OrderId,
                    ProductId = item.ProductId,
                    PurchaseСost = item.PurchaseСost,
                    SellCost = item.SellCost,
                    Amount = item.Amount,
                });
            }
        }

        public static void EditOrderProduct(int orderid, ProductViewModel product, int amount)
        {
            OrderProducts.FirstOrDefault(x => x.OrderId == orderid && x.Product.Id == product.Id).Amount = amount;
        }

        public static bool AddListOrderInDataBase(List<OrderProductViewModel> orderProducts)
        {
            AddProduct(orderProducts);
            CashBoxDataContext.Context.OrderProducts.AddRangeAsync(OrderProducts);
            return true;
        }
    }
}
