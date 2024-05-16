using Cashbox.Core;
using Cashbox.MVVM.Models;
using System.Windows;
using System.Windows.Media;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class ProductViewModel(Product product) : ViewModelBase
    {
        private readonly Product _product = product;

        public static async Task<List<ProductViewModel>> GetProducts(bool ShowNoAvailable = false) => await Product.GetProducts(ShowNoAvailable);
        public static async Task<ProductViewModel?> CreateProduct(ProductViewModel? productVM) => await Product.CreateProducts(productVM);
        public static async Task<ProductViewModel?> RemoveProduct(int id) => await Product.AvailableProducts(id, false);
        public static async Task<ProductViewModel?> UnRemoveProduct(int id) => await Product.AvailableProducts(id, true);
        public static async Task<bool> EditProductAmount(int product_id, int amount) => await Product.EditProductAmountVM(product_id, amount);
        public static async Task<bool> ImportProducts(List<ProductViewModel> productVM) => await Product.ImportProductsVM(productVM);
        public static async Task<bool> ClearCountSell() => await Product.ClearCountSellVM();
        public static async Task<bool> EditProducts(List<ProductViewModel> productVM) => await Product.EditProductsVM(productVM);

        public int Id => _product.Id;

        public string Title
        {
            get => _product.Title;
            set
            {
                _product.Title = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _product.Description;
            set
            {
                _product.Description = value;
                OnPropertyChanged();
            }
        }

        public string Brand
        {
            get => _product.Brand;
            set
            {
                _product.Brand = value;
                OnPropertyChanged();
            }
        }

        public int CategoryId
        {
            get => _product.CategoryId;
            set
            {
                _product.CategoryId = value;
                OnPropertyChanged();
            }
        }

        public double SellCost
        {
            get => _product.SellCost;
            set
            {
                if (value < 0)
                    value *= -1;
                _product.SellCost = value;
                OnPropertyChanged();
            }
        }

        public int CountSell
        {
            get => _product.CountSell;
            set
            {
                _product.CountSell = value;
                OnPropertyChanged();
            }
        }

        public string ReSellCost { get; set; } = string.Empty;
        public Visibility ReSellCostVisibility
        {
            get
            {
                if (ReSellCost != string.Empty)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        public bool IsAvailable
        {
            get => _product.IsAvailable;
            set
            {
                _product.IsAvailable = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush? BackGroundColor
        {
            get
            {
                if (_product.Stock.Amount == 0)
                    return (SolidColorBrush)Application.Current.Resources["DisabledRed"];
                if (IsAvailable)
                    return (SolidColorBrush)Application.Current.Resources["BasicW"];
                return (SolidColorBrush)Application.Current.Resources["HoverW"];
            }
        }

        public int AmountRes { get; set; }

        public ProductCategoryViewModel CategoryVM
        {
            get => new(_product.Category);
            set
            {
                _product.CategoryId = value.Id;
                OnPropertyChanged();
            }
        }

        public virtual ProductCategory Category
        {
            get => _product.Category;
            set
            {
                _product.Category = value;
                OnPropertyChanged();
            }
        }

        public virtual Stock? Stock
        {
            get => _product.Stock;
            set
            {
                _product.Stock = value;
                OnPropertyChanged();
            }
        }

    }
}
