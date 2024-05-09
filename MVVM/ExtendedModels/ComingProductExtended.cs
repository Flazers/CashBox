using Cashbox.Core;
using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;

namespace Cashbox.MVVM.Models
{
    public partial class ComingProduct
    {
        public ComingProduct() { }

        private static async Task<bool> CreateNewComing(double BuyCost)
        {
            try
            {
                ComingProduct comingProduct = new()
                {
                    CommingDatetime = DateTime.Now,
                    UserId = UserViewModel.GetCurrentUser().Id,
                    BuyCost = BuyCost,
                };
                CashBoxDataContext.Context.ComingProducts.Add(comingProduct);
                await CashBoxDataContext.Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }

        public static async Task<bool> NewComing(double BuyCost) => await CreateNewComing(BuyCost);
        public static async Task<List<ComingProductViewModel>> GetComing() => await CashBoxDataContext.Context.ComingProducts.Select(s => new ComingProductViewModel(s)).ToListAsync();
        public static async Task<List<ComingProductViewModel>> GetComingFromData(DateTime startDate, DateTime endDate) => await CashBoxDataContext.Context.ComingProducts.Where(x => x.CommingDatetime >= startDate && x.CommingDatetime <= endDate).Select(s => new ComingProductViewModel(s)).ToListAsync();
    }
}
