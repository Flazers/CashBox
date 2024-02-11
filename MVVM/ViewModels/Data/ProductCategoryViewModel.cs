using Cashbox.Core;
using Cashbox.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class ProductCategoryViewModel(ProductCategory productCategory) : ViewModelBase
    {
        private readonly ProductCategory _productCategory = productCategory;

        public static async Task<List<ProductCategoryViewModel>> GetProductCategory() => await ProductCategory.GetProductCategories();
        public static async Task<ProductCategoryViewModel?> CreateProductCategory(string category) => await ProductCategory.CreateProductCategories(category);
        public static async Task<ProductCategoryViewModel?> RemoveProductCategory(int id_category, int prodRect) => await ProductCategory.RemoveProductCategories(id_category, prodRect);

        public int Id => _productCategory.Id;

        public string Category => _productCategory.Category;

        public virtual ICollection<Product> Products => _productCategory.Products;
    }
}
