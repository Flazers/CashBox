using Cashbox.Core;
using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Identity.Client.NativeInterop;
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
        private static async Task<ProductViewModel?> NewProduct(ProductViewModel? productVM)
        {
            try
            {
                Product product = new()
                {
                    ArticulCode = productVM.ArticulCode,
                    Title = productVM.Title,
                    Description = productVM.Description,
                    Brand = productVM.Brand,
                    CategoryId = productVM.CategoryId,
                    SellCost = productVM.SellCost,
                    IsAvailable = true,
                };
                CashBoxDataContext.Context.Products.Add(product);
                await CashBoxDataContext.Context.SaveChangesAsync();
                await PStockViewModel.CreateProductStock(product.Id, productVM.AmountRes);
                return new(product);
            }
            catch (Exception) { return null; }
        }

        private static async Task<ProductViewModel?> UpdateProduct(ProductViewModel? productVM)
        {
            try
            {
                Product? product = CashBoxDataContext.Context.Products.FirstOrDefault(x => x.Id == productVM.Id);
                if (product == null) return null;
                product.ArticulCode = productVM.ArticulCode;
                product.Title = productVM.Title;
                product.Description = productVM.Description;
                product.Brand = productVM.Brand;
                product.CategoryId = productVM.CategoryId;
                product.SellCost = productVM.SellCost;
                product.Stock!.Amount = productVM.AmountRes;
                await CashBoxDataContext.Context.SaveChangesAsync();
                return new(product);
            }
            catch (Exception) { return null; }
        }

        private static async Task<bool> ImportProduct(List<ProductViewModel?> productVM)
        {
            try
            {
                foreach (ProductViewModel item in productVM)
                {
                    Product? product = CashBoxDataContext.Context.Products.FirstOrDefault(x => x.Brand == item.Brand && x.Title == item.Title);
                    if (product == null)
                        await NewProduct(item);
                    else
                        await UpdateProduct(item);
                }
                return true;
            }
            catch (Exception ex) 
            {
                AppCommand.ErrorMessage(ex.Message);
                return false; 
            }
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

        public static async Task<List<ProductViewModel>> GetProducts(bool NoAvailable) {
            if (!NoAvailable)
                return await CashBoxDataContext.Context.Products.Where(x => x.IsAvailable == true).Select(s => new ProductViewModel(s)).ToListAsync();
            else
                return await CashBoxDataContext.Context.Products.Select(s => new ProductViewModel(s)).ToListAsync();
        }
        public static async Task<ProductViewModel?> CreateProducts(ProductViewModel? productVM) => await NewProduct(productVM);
        public static async Task<ProductViewModel?> UpdateProducts(ProductViewModel? productVM) => await UpdateProduct(productVM);
        public static async Task<ProductViewModel?> AvailableProducts(int id, bool Available) => await AvailableProduct(id, Available);
        public static async Task<bool> ImportProductVM(List<ProductViewModel?> productVM) => await ImportProduct(productVM);
    }
}
