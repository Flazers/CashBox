using Cashbox.Core;
using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;

namespace Cashbox.MVVM.Models
{
    public partial class Order
    {
        private Order() { }

        private static Order? _orderComposition;
        public static Order? OrderComposition
        {
            get => _orderComposition;
            set => _orderComposition = value;
        }

        public static async Task<double> GetSumInDay(DateOnly? dateOnly)
        {
            if (dateOnly == null)
                return 0;
            var data = await GetAllDayOrders(dateOnly);
            double sum = 0;
            foreach (var item in data)
                sum += (double)item.SellCost!;
            return sum;
        }

        public static double GetSumMethodInDay(DateOnly? dateOnly)
        {
            if (dateOnly == null)
                return 0;
            var data = GetDayOrdersToMethod(dateOnly, 2).Result;
            double sum = 0;
            foreach (var item in data)
                sum += (double)item.SellCost!;
            return sum;
        }

        public static bool RemoveNullReferenceOrder()
        {
            try
            {
                CashBoxDataContext.Context.RemoveRange(CashBoxDataContext.Context.Orders.Where(x => x.PaymentMethodId == null));
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }

        public static async Task<List<OrderViewModel>> GetSellDetail(DateOnly StartData, DateOnly EndData) => await CashBoxDataContext.Context.Orders
            .Where(x => DateOnly.FromDateTime((DateTime)x.SellDatetime!) >= StartData
            && DateOnly.FromDateTime((DateTime)x.SellDatetime!) <= EndData)
            .Select(s => new OrderViewModel(s)).ToListAsync();


        public static async Task<OrderViewModel> CreateOrder()
        {
            OrderComposition = new()
            {
                UserId = UserViewModel.GetCurrentUser().Id,
                DailyReportId = DailyReportViewModel.GetCurrentShift().Id,
                SellDatetime = DateTime.Now,
            };
            CashBoxDataContext.Context.Add(OrderComposition);
            await CashBoxDataContext.Context.SaveChangesAsync();
            return new(OrderComposition);
        }

        public static async Task<bool> SellOrder(int paymet, double sellcost, double discount, List<OrderProductViewModel> orderProducts)
        {
            try
            {
                OrderComposition.PaymentMethodId = paymet;
                OrderComposition.Discount = discount;
                OrderComposition.SellCost = sellcost;
                OrderProduct.AddListOrderInDataBase(orderProducts);
                await CashBoxDataContext.Context.SaveChangesAsync();
                OrderComposition = null;
                OrderProduct.OrderProducts = [];
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }

        public static async Task<OrderViewModel> RemoveCurrentOrder()
        {
            if (OrderComposition == null)
                return new(null!);
            CashBoxDataContext.Context.Orders.Remove(OrderComposition);
            await CashBoxDataContext.Context.SaveChangesAsync();
            OrderComposition = null;
            return new(null!);
        }

        public static async Task<List<OrderViewModel>> GetDayOrdersToMethod(DateOnly? dateOnly, int method) => await CashBoxDataContext.Context.Orders.Where(x => x.PaymentMethodId == method && DateOnly.FromDateTime((DateTime)x.SellDatetime!) == dateOnly).Select(s => new OrderViewModel(s)).ToListAsync();
        public static async Task<List<OrderViewModel>> GetAllDayOrders(DateOnly? dateOnly) => await CashBoxDataContext.Context.Orders.Where(x => DateOnly.FromDateTime((DateTime)x.SellDatetime!) == dateOnly && x.PaymentMethod != null).Select(s => new OrderViewModel(s)).ToListAsync();
        public static async Task<List<ProductCategoryViewModel>> GetProductCategories() => await CashBoxDataContext.Context.ProductCategories.Select(s => new ProductCategoryViewModel(s)).ToListAsync();
    }
}
