using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;
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

        public static async void AddProduct(List<OrderProductViewModel> orderProducts)
        {
            foreach (var item in orderProducts) 
            {
                OrderProducts.Add(new()
                {
                    OrderId = item.OrderId,
                    ProductId = item.ProductId,
                    SellCost = item.SellCost,
                    Amount = item.Amount,
                });
                await PStockViewModel.UpdateProductStock(item.ProductId, item.ProductVM.Stock.Amount - item.Amount);
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

        public static async Task<List<OrderProductViewModel>> GetInOrderProduct(int OrderId) => await CashBoxDataContext.Context.OrderProducts.Where(x => x.OrderId == OrderId).Select(s => new OrderProductViewModel(s)).ToListAsync();
        public static async Task<List<OrderProductViewModel>> GetOrderProduct(DateTime StartDate, DateTime EndDate) => await CashBoxDataContext.Context.OrderProducts.Where(x => x.Order.SellDatetime!.Value.Date >= StartDate.Date && x.Order.SellDatetime!.Value.Date <= EndDate.Date).Select(s => new OrderProductViewModel(s)).ToListAsync();
    }
}
