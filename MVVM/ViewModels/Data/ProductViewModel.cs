using Cashbox.Core;
using Cashbox.MVVM.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class ProductViewModel : ViewModelBase
    {
        private readonly Product _product;
        public ProductViewModel(Product product)
        {
            _product = product;
        }

        public static async Task<List<ProductViewModel>> GetProducts(bool ShowNoAvailable = false) => await Product.GetProducts(ShowNoAvailable);
        public static async Task<ProductViewModel?> CreateProduct(ProductViewModel? productVM) => await Product.CreateProducts(productVM);
        public static async Task<ProductViewModel?> UpdateProduct(ProductViewModel? productVM) => await Product.UpdateProducts(productVM);
        public static async Task<ProductViewModel?> RemoveProduct(int id) => await Product.AvailableProducts(id, false);
        public static async Task<ProductViewModel?> UnRemoveProduct(int id) => await Product.AvailableProducts(id, true);
        public static async Task<bool> ImportProduct(List<ProductViewModel?> productVM) => await Product.ImportProductVM(productVM);

        public int Id => _product.Id;

        public string? ArticulCode
        {
            get => _product.ArticulCode;
            set
            {
                _product.ArticulCode = value;
                OnPropertyChanged();
            }
        }

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
                _product.SellCost = value;
                OnPropertyChanged();
            }
        }

        public double ReSellCost { get; set; }
        public Visibility ReSellCostVisibility
        {
            get
            {
                if (ReSellCost != 0)
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
                return (SolidColorBrush)Application.Current.Resources["PressedW"];
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
