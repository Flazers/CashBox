using Cashbox.MVVM.Models;
using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cashbox.MVVM.Models
{
    public partial class ProductCategory
    {
        private ProductCategory() { }
        private static async Task<ProductCategoryViewModel?> NewProductCategory(string category)
        {
            try
            {
                ProductCategory productCategory = new ProductCategory() { Category = category};
                CashBoxDataContext.Context.ProductCategories.Add(productCategory);
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new(productCategory);
            }
            catch (Exception) { return null; }
        }

        private static async Task<ProductCategoryViewModel?> RemoveProductCategory(int id_category)
        {
            try
            {
                ProductCategory? productCategory = await CashBoxDataContext.Context.ProductCategories.FirstOrDefaultAsync(x => x.Id == id_category);
                if (productCategory == null) return null;
                if (productCategory.Products.Count != 0)
                    foreach (var product in productCategory.Products) 
                        product.CategoryId = 1;
                CashBoxDataContext.Context.ProductCategories.Remove(productCategory);
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new(productCategory);
            }
            catch (Exception) { return null; }
        }

        public static async Task<List<ProductCategoryViewModel>> GetProductCategories() => await CashBoxDataContext.Context.ProductCategories.Select(s => new ProductCategoryViewModel(s)).ToListAsync();
        public static async Task<ProductCategoryViewModel?> CreateProductCategories(string category) => await NewProductCategory(category);
        public static async Task<ProductCategoryViewModel?> RemoveProductCategories(int id_category) => await RemoveProductCategory(id_category);
    }
}
