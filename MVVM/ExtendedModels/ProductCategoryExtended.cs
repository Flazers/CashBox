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
                ProductCategory productCategory = new() { Category = category };
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
                if (productCategory.Products.Count > 0) return false;
                CashBoxDataContext.Context.ProductCategories.Remove(productCategory);
                await CashBoxDataContext.Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                AppCommand.ErrorMessage(ex.Message);
                return false;
            }
        }

        public static ProductCategoryViewModel NewExample(string category)
        {
            try
            {
                ProductCategory productCategory = new() { Category = category };
                return new(productCategory);
            }
            catch (Exception ex) 
            { 
                AppCommand.ErrorMessage(ex.Message);
                return null!; 
            }
        }
        public static async Task<List<ProductCategoryViewModel>> GetProductCategories() => await CashBoxDataContext.Context.ProductCategories.Select(s => new ProductCategoryViewModel(s)).ToListAsync();
        public static async Task<ProductCategoryViewModel> CreateProductCategories(string category) => await NewProductCategory(category);
        public static async Task<bool> RemoveProductCategories(int id_category) => await RemoveProductCategory(id_category);
    }
}
