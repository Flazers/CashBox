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

        public static void AddProduct(int orderid, ProductViewModel product, int amount)
        {
            OrderProducts.Add(new OrderProduct()
            {
                OrderId = orderid,
                ProductId = product.Id,
                PurchaseСost = product.PurchaseСost,
                SellCost = product.SellCost,
                Amount = amount,
            });
        }

        public static void EditOrderProduct(int orderid, ProductViewModel product, int amount)
        {
            OrderProducts.FirstOrDefault(x => x.OrderId == orderid && x.Product.Id == product.Id).Amount = amount;
        }

        public static async Task<List<OrderProduct>> SellOrder()
        {
            await CashBoxDataContext.Context.AddRangeAsync(OrderProducts);
            await CashBoxDataContext.Context.SaveChangesAsync();
            return OrderProducts;
        }
    }
}
