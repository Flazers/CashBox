using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Cashbox.MVVM.Models
{
    public partial class Product
    {
        public Product() { }
        private static async Task<ProductViewModel?> NewProduct(ProductViewModel? productVM, int Amount)
        {
            try
            {
                Product product = new()
                {
                    ArticulCode = productVM.ArticulCode,
                    Title = productVM.Title,
                    Description = productVM.Description,
                    Image = productVM.Image,
                    Brand = productVM.Brand,
                    CategoryId = productVM.CategoryId,
                    PurchaseСost = productVM.PurchaseСost,
                    SellCost = productVM.SellCost,
                    IsAvailable = true,
                };
                CashBoxDataContext.Context.Products.Add(product);
                await CashBoxDataContext.Context.SaveChangesAsync();
                if (product.Image == "./Assets/Image/ProductIdTempSaved.jpeg")
                {
                    FileSystem.Rename(product.Image, $"./Assets/Image/ProductId{product.Id}Saved.jpeg");
                    product.Image = $"./Assets/Image/ProductId{product.Id}Saved.jpeg";
                }
                await PStockViewModel.CreateProductStock(product.Id, Amount);
                return new(product);
            }
            catch (Exception) { return null; }
        }

        private static async Task<ProductViewModel?> UpdateProduct(ProductViewModel? productVM, int _amount)
        {
            try
            {
                Product? product = CashBoxDataContext.Context.Products.FirstOrDefault(x => x.Id == productVM.Id);
                if (product == null) return null;
                product.ArticulCode = productVM.ArticulCode;
                product.Title = productVM.Title;
                product.Description = productVM.Description;
                product.Image = productVM.Image;
                product.Brand = productVM.Brand;
                product.CategoryId = productVM.CategoryId;
                product.PurchaseСost = productVM.PurchaseСost;
                product.SellCost = productVM.SellCost;
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
        public static async Task<ProductViewModel?> CreateProducts(ProductViewModel? productVM, int Amount) => await NewProduct(productVM, Amount);
        public static async Task<ProductViewModel?> UpdateProducts(ProductViewModel? productVM, int _Amount) => await UpdateProduct(productVM, _Amount);
        public static async Task<ProductViewModel?> AvailableProducts(int id, bool Available) => await AvailableProduct(id, Available);
    }
}
