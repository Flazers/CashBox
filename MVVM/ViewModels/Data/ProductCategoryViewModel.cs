﻿using Cashbox.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class ProductCategoryViewModel
    {
        private readonly ProductCategory _productCategory;
        public ProductCategoryViewModel(ProductCategory productCategory)
        {
            _productCategory = productCategory;
        }
        public static async Task<List<ProductCategoryViewModel>> GetProductCategory() => await ProductCategory.GetProductCategories();
        public static async Task<ProductCategoryViewModel?> CreateProductCategory(string category) => await ProductCategory.CreateProductCategories(category);

        public int Id => _productCategory.Id;

        public string Category => _productCategory.Category;
    }
}
