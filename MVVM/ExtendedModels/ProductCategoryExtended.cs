using Cashbox.MVVM.Models;
using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return new ProductCategoryViewModel(productCategory);
            }
            catch (Exception) { return null; }
        }

        public static async Task<List<ProductCategoryViewModel>> GetProductCategories() => await CashBoxDataContext.Context.ProductCategories.Select(s => new ProductCategoryViewModel(s)).ToListAsync();
        public static async Task<ProductCategoryViewModel?> CreateProductCategories(string category) => await NewProductCategory(category);
    }
}
