using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.Models
{
    public partial class Product
    {
        private Product() { }
        private static async Task<ProductViewModel?> NewProduct(string? _articulCode, string _title, string _description, byte[]? _image, string _brand, int? _categoryId, double _purchaseСost, double _sellCost,int _amount)
        {
            try
            {
                Product product = new()
                {
                    ArticulCode = _articulCode,
                    Title = _title,
                    Description = _description,
                    Image = _image,
                    Brand = _brand,
                    CategoryId = _categoryId,
                    PurchaseСost = _purchaseСost,
                    SellCost = _sellCost,
                    IsAvailable = true
                };
                CashBoxDataContext.Context.Products.Add(product);
                await CashBoxDataContext.Context.SaveChangesAsync();
                await PStockViewModel.CreateProductStock(product.Id, _amount);
                return new(product);
            }
            catch (Exception) { return null; }
        }

        private static async Task<ProductViewModel?> UpdateProduct(int _id, string? _articulCode, string _title, string _description, byte[]? _image, string _brand, int? _categoryId, double _purchaseСost, double _sellCost, int _amount)
        {
            try
            {
                Product? product = CashBoxDataContext.Context.Products.FirstOrDefault(x => x.Id == _id); //CashBoxDataContext.Context.Products.FirstOrDefault(x => x.Id == itemProduct.Id);
                if (product == null) return null;
                product.ArticulCode = _articulCode;
                product.Title = _title;
                product.Description = _description;
                product.Image = _image;
                product.Brand = _brand;
                product.CategoryId = _categoryId;
                product.PurchaseСost = _purchaseСost;
                product.SellCost = _sellCost;
                product.Stock!.Amount = _amount;
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new(product);
            }
            catch (Exception) { return null; }
        }

        private static async Task<ProductViewModel?> AvailableProduct(int id, bool Available)
        {
            try
            {
                Product? product = CashBoxDataContext.Context.Products.FirstOrDefault(x => x.Id == id);
                if (product == null) return null;
                product.IsAvailable = Available;
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new(product);
            }
            catch (Exception) { return null; }
        }

        public static async Task<List<ProductViewModel>> GetProducts() => await CashBoxDataContext.Context.Products.Where(x => x.IsAvailable == true).Select(s => new ProductViewModel(s)).ToListAsync();
        public static async Task<List<ProductViewModel>> GetAllProducts() => await CashBoxDataContext.Context.Products.Select(s => new ProductViewModel(s)).ToListAsync();
        public static async Task<ProductViewModel?> CreateProducts(string? ArticulCode, string Title, string Description, byte[]? Image, string Brand, int? CategoryId, double PurchaseСost, double SellCost, int Amount) => await NewProduct(ArticulCode, Title, Description, Image, Brand, CategoryId, PurchaseСost, SellCost, Amount);
        public static async Task<ProductViewModel?> UpdateProducts(int id, string? ArticulCode, string Title, string Description, byte[]? Image, string Brand, int? CategoryId, double PurchaseСost, double SellCost, int Amount) => await UpdateProduct(id, ArticulCode, Title, Description, Image, Brand, CategoryId, PurchaseСost, SellCost, Amount);
        public static async Task<ProductViewModel?> AvailableProducts(int id, bool Available) => await AvailableProduct(id, Available);
    }
}
