using Cashbox.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class PStockViewModel
    {
        private readonly Stock _stock;
        public PStockViewModel(Stock stock) 
        {
            _stock = stock;
        }

        public async Task<PStockViewModel?> GetProductStock(int product_id) => await Stock.GetProductAmount(product_id);
        public async Task<PStockViewModel?> CreateProductStock(int product_id, int Amount) => await Stock.CreateProductStock(product_id,Amount);
        public async Task<PStockViewModel?> UpdateProductStock(int product_id, int Amount) => await Stock.UpdateProductAmount(product_id, Amount);

        public int ProductId => _stock.ProductId;

        public int Amount => _stock.Amount;
    }
}
