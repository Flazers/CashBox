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
                ProductCategory productCategory = new() { Category = category};
                CashBoxDataContext.Context.ProductCategories.Add(productCategory);
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new(productCategory);
            }
            catch (Exception) { return null; }
        }

        private static async Task<ProductCategoryViewModel?> RemoveProductCategory(int id_category, int t)
        {
            try
            {
                ProductCategory? productCategory = await CashBoxDataContext.Context.ProductCategories.FirstOrDefaultAsync(x => x.Id == id_category);
                if (productCategory == null) return null;
                if (t == 1)
                    foreach (var product in productCategory.Products)
                        product.CategoryId = 1;
                else if (t == 2)
                    foreach (var product in productCategory.Products)
                        await ProductViewModel.RemoveProduct(product.Id);
                CashBoxDataContext.Context.ProductCategories.Remove(productCategory);
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new(productCategory);
            }
            catch (Exception) { return null; }
        }

        public static async Task<ProductCategoryViewModel> NewExample(string category)
        {
            try
            {
                ProductCategory productCategory = new() { Category = category };
                return new(productCategory);
            }
            catch (Exception) { return null!; }
        }
        public static async Task<List<ProductCategoryViewModel>> GetProductCategories() => await CashBoxDataContext.Context.ProductCategories.Select(s => new ProductCategoryViewModel(s)).ToListAsync();
        public static async Task<ProductCategoryViewModel?> CreateProductCategories(string category) => await NewProductCategory(category);
        public static async Task<ProductCategoryViewModel?> RemoveProductCategories(int id_category, int prodRect) => await RemoveProductCategory(id_category, prodRect);
    }
}
