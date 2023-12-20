using Cashbox.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class ProductViewModel
    {
        private readonly Product _product;
        public ProductViewModel(Product product)
        {
            _product = product;
        }

        public static async Task<List<ProductViewModel>> GetProduct() => await Product.GetProducts();
        public static async Task<ProductViewModel?> CreateProduct(string ArticulCode, string Title, string Description, byte[] Image, string Brand, int CategoryId, double PurchaseСost, double SellCost) => await Product.CreateProducts(ArticulCode, Title, Description, Image, Brand, CategoryId, PurchaseСost, SellCost);
        public static async Task<ProductViewModel?> UpdateProduct(int id, string ArticulCode, string Title, string Description, byte[] Image, string Brand, int CategoryId, double PurchaseСost, double SellCost) => await Product.UpdateProducts(id, ArticulCode, Title, Description, Image, Brand, CategoryId, PurchaseСost, SellCost);
        public static async Task<ProductViewModel?> RemoveProduct(int id) => await Product.AvailableProducts(id, false);
        public static async Task<ProductViewModel?> UnRemoveProduct(int id) => await Product.AvailableProducts(id, true);

        public int Id => _product.Id;

        public string? ArticulCode => _product.ArticulCode;

        public string Title => _product.Title;

        public string Description => _product.Description;

        public byte[]? Image => _product.Image;

        public string Brand => _product.Brand;

        public int CategoryId => _product.CategoryId;

        public double PurchaseСost => _product.PurchaseСost;

        public double SellCost => _product.SellCost;
        public bool isAvailable => _product.isAvailable;

        public virtual Stock? Stock => _product.Stock;
    }
}
