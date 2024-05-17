using Cashbox.Core;
using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;

namespace Cashbox.MVVM.Models
{
    public partial class ProductCategory
    {
        private ProductCategory() { }
        private static async Task<ProductCategoryViewModel> NewProductCategory(string category)
        {
            try
            {
                ProductCategory productCategory = new() { Category = category, IsAvailable = true };
                CashBoxDataContext.Context.ProductCategories.Add(productCategory);
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new(productCategory);
            }
            catch (Exception ex) 
            {
                AppCommand.ErrorMessage(ex.Message);
                return null!;
            }
        }

        private static async Task<bool> RemoveProductCategory(int id_category)
        {
            try
            {
                ProductCategory? productCategory = await CashBoxDataContext.Context.ProductCategories.FirstOrDefaultAsync(x => x.Id == id_category);
                if (productCategory == null) return false;
                foreach (var item in productCategory.Products)
                {
                    try
                    {
                        if (item.OrderProducts.Count > 0)
                        {
                            item.IsAvailable = false;
                            item.Brand = $"[Удалено] {item.Brand}";
                        } 
                        else
                        {
                            CashBoxDataContext.Context.Stocks.Remove(item.Stock!);
                            CashBoxDataContext.Context.Products.Remove(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        AppCommand.ErrorMessage(ex.Message);
                        return false;
                    }
                }
                productCategory.IsAvailable = false;
                await CashBoxDataContext.Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }

        public static async Task<List<ProductCategoryViewModel>> GetProductCategories() 
        {
            List<ProductCategoryViewModel> productsCategory = [];
            productsCategory.Add(new(new() { Category = "Все категории", IsAvailable = true }));
            productsCategory.AddRange(await CashBoxDataContext.Context.ProductCategories.Where(x => x.IsAvailable == true).Select(s => new ProductCategoryViewModel(s)).ToListAsync());
            return new(productsCategory);
        }
        public static async Task<ProductCategoryViewModel> CreateProductCategories(string category) => await NewProductCategory(category);
        public static async Task<bool> RemoveProductCategories(int id_category) => await RemoveProductCategory(id_category);
    }
}
