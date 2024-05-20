using Cashbox.Core;
using Cashbox.MVVM.Models;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class ProductCategoryViewModel(ProductCategory productCategory) : ViewModelBase
    {
        private readonly ProductCategory _productCategory = productCategory;

        public static async Task<List<ProductCategoryViewModel>> GetProductCategory(bool HideCategory = true) => await ProductCategory.GetProductCategories(HideCategory);
        public static async Task<ProductCategoryViewModel> CreateProductCategory(string category) => await ProductCategory.CreateProductCategories(category);
        public static async Task<bool> RemoveProductCategory(int id_category) => await ProductCategory.RemoveProductCategories(id_category);

        public int Id => _productCategory.Id;

        public string Category
        {
            get => _productCategory.Category;
            set
            {
                _productCategory.Category = value;
                OnPropertyChanged();
            }
        }

        public bool IsAvailable
        {
            get => _productCategory.IsAvailable;
            set
            {
                _productCategory.IsAvailable = value;
                OnPropertyChanged();
            }
        }

        public virtual ICollection<Product> Products => _productCategory.Products;
    }
}
