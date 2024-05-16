using Cashbox.Core;
using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;

namespace Cashbox.MVVM.Models
{
    public partial class Product
    {
        public Product() { }

        private static async Task<ProductViewModel?> NewProduct(ProductViewModel? productVM)
        {
            try
            {
                Product product = new()
                {
                    Title = productVM.Title,
                    Description = productVM.Description,
                    Brand = productVM.Brand,
                    CategoryId = productVM.CategoryId,
                    SellCost = productVM.SellCost,
                    CountSell = productVM.CountSell,
                    IsAvailable = true,
                };
                CashBoxDataContext.Context.Products.Add(product);
                await CashBoxDataContext.Context.SaveChangesAsync();
                await PStockViewModel.CreateProductStock(product.Id, productVM.AmountRes);
                return new(product);
            }
            catch (Exception) { return null; }
        }

        public static async Task<List<ProductViewModel>> GetProducts(bool ShowNoAvailable)
        {
            if (ShowNoAvailable)
                return await CashBoxDataContext.Context.Products.Select(s => new ProductViewModel(s)).ToListAsync();
            else
                return await CashBoxDataContext.Context.Products.Where(x => x.IsAvailable == true && x.Stock.Amount > 0).Select(s => new ProductViewModel(s)).ToListAsync();
        }

        private static async Task<ProductViewModel?> UpdateProduct(ProductViewModel? productVM, bool SetOrPlus)
        {
            try
            {
                Product? product = CashBoxDataContext.Context.Products.FirstOrDefault(x => x.Id == productVM.Id);
                if (product == null) return null;
                product.Title = productVM.Title;
                product.Description = productVM.Description;
                product.Brand = productVM.Brand;
                product.CategoryId = productVM.CategoryId;
                product.SellCost = productVM.SellCost;
                if (SetOrPlus)
                    product.Stock!.Amount = productVM.AmountRes;
                else
                    product.Stock!.Amount += productVM.AmountRes;
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new(product);
            }
            catch (Exception) { return null; }
        }

        private static async Task<bool> ImportProducts(List<ProductViewModel> productVM)
        {
            try
            {
                foreach (ProductViewModel item in productVM)
                {
                    Product? product = CashBoxDataContext.Context.Products.FirstOrDefault(x => x.Brand == item.Brand && x.Title == item.Title && x.Description == item.Description);
                    if (product != null)
                        await UpdateProduct(item, false);
                    else
                        await NewProduct(item);
                }
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }

        private static async Task<bool> EditProducts(List<ProductViewModel> productVM)
        {
            try
            {
                foreach (ProductViewModel item in productVM)
                {
                    Product? product = CashBoxDataContext.Context.Products.FirstOrDefault(x => x.Id == item.Id);
                    await UpdateProduct(item, true);
                }
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }

        private static async Task<bool> EditProductAmount(int product_id, int amount)
        {
            try
            {
                Product? product = CashBoxDataContext.Context.Products.FirstOrDefault(x => x.Id == product_id);
                if (product == null) return false;
                product.Stock.Amount += amount;
                await CashBoxDataContext.Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }

        private static async Task<bool> ClearCountSell()
        {
            try
            {
                List<Product> products = [.. CashBoxDataContext.Context.Products.Where(x => x.CountSell > 0)];
                if (products.Count == 0) return false;
                foreach (var item in products)
                    item.CountSell = 0;
                await CashBoxDataContext.Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }

        private static async Task<ProductViewModel> AvailableProduct(int id, bool Available)
        {
            try
            {
                Product? product = CashBoxDataContext.Context.Products.FirstOrDefault(x => x.Id == id);
                if (product == null) return null!;
                product.IsAvailable = Available;
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new(product);
            }
            catch (Exception) { return null!; }
        }


        public static async Task<ProductViewModel?> CreateProducts(ProductViewModel? productVM) => await NewProduct(productVM);
        public static async Task<ProductViewModel?> AvailableProducts(int id, bool Available) => await AvailableProduct(id, Available);
        public static async Task<bool> ImportProductsVM(List<ProductViewModel> productVM) => await ImportProducts(productVM);
        public static async Task<bool> EditProductsVM(List<ProductViewModel> productVM) => await EditProducts(productVM);
        public static async Task<bool> ClearCountSellVM() => await ClearCountSell();
        public static async Task<bool> EditProductAmountVM(int product_id, int amount) => await EditProductAmount(product_id, amount);
    }
}
