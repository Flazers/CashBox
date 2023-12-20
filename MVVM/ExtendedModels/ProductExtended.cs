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
        private static async Task<ProductViewModel?> NewProduct(string ArticulCode, string Title, string Description, byte[] Image, string Brand, int CategoryId, double PurchaseСost, double SellCost)
        {
            try
            {
                Product product = new Product()
                {
                    ArticulCode = ArticulCode,
                    Title = Title,
                    Description = Description,
                    Image = Image,
                    Brand = Brand,
                    CategoryId = CategoryId,
                    PurchaseСost = PurchaseСost,
                    SellCost = SellCost,
                    isAvailable = true
                };
                CashBoxDataContext.Context.Products.Add(product);
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new ProductViewModel(product);
            }
            catch (Exception) { return null; }
        }

        private static async Task<ProductViewModel?> UpdateProduct(int id, string ArticulCode, string Title, string Description, byte[] Image, string Brand, int CategoryId, double PurchaseСost, double SellCost)
        {
            try
            {
                Product? product = CashBoxDataContext.Context.Products.FirstOrDefault(x => x.Id == id);
                if (product == null) return null;
                product.ArticulCode = ArticulCode;
                product.Title = Title;
                product.Description = Description;
                product.Image = Image;
                product.Brand = Brand;
                product.CategoryId = CategoryId;
                product.PurchaseСost = PurchaseСost;
                product.SellCost = SellCost;
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new ProductViewModel(product);
            }
            catch (Exception) { return null; }
        }

        private static async Task<ProductViewModel?> AvailableProduct(int id, bool Available)
        {
            try
            {
                Product? product = CashBoxDataContext.Context.Products.FirstOrDefault(x => x.Id == id);
                if (product == null) return null;
                product.isAvailable = Available;
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new ProductViewModel(product);
            }
            catch (Exception) { return null; }
        }

        public static async Task<List<ProductViewModel>> GetProducts() => await CashBoxDataContext.Context.Products.Select(s => new ProductViewModel(s)).ToListAsync();
        public static async Task<ProductViewModel?> CreateProducts(string ArticulCode, string Title, string Description, byte[] Image, string Brand, int CategoryId, double PurchaseСost, double SellCost) => await NewProduct(ArticulCode, Title, Description, Image, Brand, CategoryId, PurchaseСost, SellCost);
        public static async Task<ProductViewModel?> UpdateProducts(int id, string ArticulCode, string Title, string Description, byte[] Image, string Brand, int CategoryId, double PurchaseСost, double SellCost) => await UpdateProduct(id, ArticulCode, Title, Description, Image, Brand, CategoryId, PurchaseСost, SellCost);
        public static async Task<ProductViewModel?> AvailableProducts(int id, bool Available) => await AvailableProduct(id, Available);
    }
}
