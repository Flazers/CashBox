using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.Models
{
    public partial class Order
    {
        private Order() {}

        public static async Task<List<OrderViewModel>> GetDayOrdersToMethod(DateTime dateTime, int method) => await CashBoxDataContext.Context.Orders.Where(x => x.PaymentMethodId == method && x.SellDatetime.Date == dateTime.Date).Select(s => new OrderViewModel(s)).ToListAsync();
        public static async Task<List<OrderViewModel>> GetAllDayOrders(DateTime dateTime) => await CashBoxDataContext.Context.Orders.Where(x => x.SellDatetime.Date == dateTime.Date).Select(s => new OrderViewModel(s)).ToListAsync();
        public static async Task<List<ProductCategoryViewModel>> GetProductCategories() => await CashBoxDataContext.Context.ProductCategories.Select(s => new ProductCategoryViewModel(s)).ToListAsync();
    }
}
