using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Cashbox.MVVM.Models
{
    public partial class Product
    {
        private Product() { }
        private static async Task<ProductViewModel?> NewProduct(string? _ArticulCode, string _Title, string _Description, byte[]? _Image, string _Brand, int _CategoryId, double _PurchaseСost, double _SellCost, int _Amount)
        {
            try
            {
                Product product = new()
                {
                    ArticulCode = _ArticulCode,
                    Title = _Title,
                    Description = _Description,
                    Image = _Image,
                    Brand = _Brand,
                    CategoryId = _CategoryId,
                    PurchaseСost = _PurchaseСost,
                    SellCost = _SellCost,
                    IsAvailable = true,
                };
                CashBoxDataContext.Context.Products.Add(product);
                await CashBoxDataContext.Context.SaveChangesAsync();
                await PStockViewModel.CreateProductStock(product.Id, _Amount);
                return new(product);
            }
            catch (Exception) { return null; }
        }

        private static async Task<ProductViewModel?> UpdateProduct(int _id, string? _articulCode, string _title, string _description, byte[]? _image, string _brand, int _categoryId, double _purchaseСost, double _sellCost, int _amount)
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
        public static async Task<ProductViewModel?> CreateProducts(string? _ArticulCode, string _Title, string _Description, byte[]? _Image, string _Brand, int _CategoryId, double _PurchaseСost, double _SellCost, int _Amount) => await NewProduct(_ArticulCode, _Title, _Description, _Image, _Brand, _CategoryId, _PurchaseСost, _SellCost, _Amount);
        public static async Task<ProductViewModel?> UpdateProducts(int _id, string? _ArticulCode, string _Title, string _Description, byte[]? _Image, string _Brand, int _CategoryId, double _PurchaseСost, double _SellCost, int _Amount) => await UpdateProduct(_id, _ArticulCode, _Title, _Description, _Image, _Brand, _CategoryId, _PurchaseСost, _SellCost, _Amount);
        public static async Task<ProductViewModel?> AvailableProducts(int id, bool Available) => await AvailableProduct(id, Available);
    }
}
