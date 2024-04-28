using Cashbox.MVVM.Models;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class PStockViewModel(Stock stock)
    {
        private readonly Stock _stock = stock;

        public static async Task<PStockViewModel?> GetProductStock(int product_id) => await Stock.GetProductAmount(product_id);
        public static async Task<PStockViewModel?> CreateProductStock(int product_id, int Amount) => await Stock.CreateProductStock(product_id, Amount);
        public static async Task<PStockViewModel?> UpdateProductStock(int product_id, int Amount) => await Stock.UpdateProductAmount(product_id, Amount);

        public int ProductId => _stock.ProductId;

        public int Amount => _stock.Amount;
    }
}
