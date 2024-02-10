using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.Models
{
    public partial class Stock
    {
        private Stock() { }
        public static async Task<PStockViewModel?> CreateProductStock(int product_id, int Amount)
        {
            try
            {
                Stock stock = new()
                {
                    ProductId = product_id,
                    Amount = Amount
                };
                CashBoxDataContext.Context.Stocks.Add(stock);
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new PStockViewModel(stock);
            }
            catch (Exception) { return null; }
        }

        public static async Task<PStockViewModel?> GetProductAmount(int product_id)
        {
            Stock? stock = await CashBoxDataContext.Context.Stocks.FirstOrDefaultAsync(x => x.ProductId == product_id);
            if (stock == null) return null;
            return new PStockViewModel(stock);
        }

        public static async Task<PStockViewModel?> UpdateProductAmount(int product_id, int Amount)
        {
            try
            {
                Stock? stock = await CashBoxDataContext.Context.Stocks.FirstOrDefaultAsync(x => x.ProductId == product_id);
                if (stock == null) return null;
                stock.Amount = Amount;
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new PStockViewModel(stock);
            }
            catch (Exception) { return null; }
        }
    }
}
