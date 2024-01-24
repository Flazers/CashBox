using Cashbox.Core;
using Cashbox.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class ProductViewModel(Product product) : ViewModelBase
    {
        private readonly Product _product = product;

        public static async Task<List<ProductViewModel>> GetProducts() => await Product.GetProducts();
        public static async Task<List<ProductViewModel>> GetAllProducts() => await Product.GetAllProducts();
        public static async Task<ProductViewModel?> CreateProduct(string? _ArticulCode, string _Title, string _Description, byte[]? _Image, string _Brand, int _CategoryId, double _PurchaseСost, double _SellCost, int _Amount) => await Product.CreateProducts(_ArticulCode, _Title, _Description, _Image, _Brand, _CategoryId, _PurchaseСost, _SellCost, _Amount);
        public static async Task<ProductViewModel?> UpdateProduct(int id, string? ArticulCode, string Title, string Description, byte[]? Image, string Brand, int CategoryId, double PurchaseСost, double SellCost, int Amount) => await Product.UpdateProducts(id, ArticulCode, Title, Description, Image, Brand, CategoryId, PurchaseСost, SellCost, Amount);
        public static async Task<ProductViewModel?> RemoveProduct(int id) => await Product.AvailableProducts(id, false);
        public static async Task<ProductViewModel?> UnRemoveProduct(int id) => await Product.AvailableProducts(id, true);

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

        public byte[]? Image
        {
            get => _product.Image;
            set
            {
                _product.Image = value;
                OnPropertyChanged();
            }
        }

        public BitmapImage ImageGet
        {
            get
            {
                BitmapImage image = new();
                byte[] data = Image;
                if (Image.Length < 5) data = File.ReadAllBytes(@"Assets\Image\Zagl.png");
                using (var mem = new MemoryStream(data))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }
                image.Freeze();
                return image;
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

        public double PurchaseСost
        {
            get => _product.PurchaseСost;
            set
            {
                _product.PurchaseСost = value;
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
        public bool IsAvailable
        {
            get => _product.IsAvailable;
            set
            {
                _product.IsAvailable = value;
                OnPropertyChanged();
            }
        }

        public virtual ProductCategory? Category
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
