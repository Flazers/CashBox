using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.Models;
using Cashbox.MVVM.ViewModels.Data;
using Cashbox.MVVM.Views.Pages.Admin;
using Cashbox.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Cashbox.MVVM.ViewModels.Admin
{
    public class StockViewModel : ViewModelBase
    {
        #region Props

        private UserViewModel? _user;
        public UserViewModel? User { get => _user = Models.User.CurrentUser; }

        private string _newCategoryTitle = string.Empty;
        public string NewCategoryTitle
        {
            get => _newCategoryTitle;
            set => Set(ref _newCategoryTitle, value);
        }

        private Visibility _newProductVisibility = Visibility.Collapsed;
        public Visibility NewProductVisibility
        {
            get => _newProductVisibility;
            set => Set(ref _newProductVisibility, value);
        }

        private string _imageStringProduct = "Выбрать фото товара";
        public string ImageStringProduct
        {
            get => _imageStringProduct;
            set => Set(ref _imageStringProduct, value);
        }

        private string? _articulCodeProduct = string.Empty;
        public string? ArticulCodeProduct
        {
            get => _articulCodeProduct;
            set => Set(ref _articulCodeProduct, value);
        }

        private string _titleProduct = string.Empty;
        public string TitleProduct
        {
            get => _titleProduct;
            set => Set(ref _titleProduct, value);
        }

        private string _descriptionProduct = string.Empty;
        public string DescriptionProduct
        {
            get => _descriptionProduct;
            set => Set(ref _descriptionProduct, value);
        }

        private byte[]? _imageProduct;
        public byte[]? ImageProduct
        {
            get => _imageProduct;
            set => Set(ref _imageProduct, value);
        }

        private string _brandProduct = string.Empty;
        public string BrandProduct
        {
            get => _brandProduct;
            set => Set(ref _brandProduct, value);
        }

        private ProductCategoryViewModel _idCategoryProduct;
        public ProductCategoryViewModel IdCategoryProduct
        {
            get => _idCategoryProduct;
            set => Set(ref _idCategoryProduct, value);
        }

        private double _purchaseСostProduct;
        public double PurchaseСostProduct
        {
            get => _purchaseСostProduct;
            set => Set(ref _purchaseСostProduct, value);
        }

        private double _sellCostProduct;
        public double SellCostProduct
        {
            get => _sellCostProduct;
            set => Set(ref _sellCostProduct, value);
        }

        private bool _isAvailableProduct;
        public bool IsAvailableProduct
        {
            get => _isAvailableProduct;
            set => Set(ref _isAvailableProduct, value);
        }

        private int _amountProduct = 1;
        public int AmountProduct
        {
            get => _amountProduct;
            set => Set(ref _amountProduct, value);
        }

        private List<ProductViewModel> _collectionProducts = ProductViewModel.GetProducts().Result;
        public List<ProductViewModel> CollectionProducts
        {
            get => _collectionProducts;
            set => Set(ref _collectionProducts, value);
        }
        private List<ProductCategoryViewModel> _collectionProductCategories = ProductCategoryViewModel.GetProductCategory().Result;
        public List<ProductCategoryViewModel> CollectionProductCategories
        {
            get => _collectionProductCategories;
            set => Set(ref _collectionProductCategories, value);
        }
        #endregion


        #region Command

        public RelayCommand AddCategoryCommand { get; set; }
        private bool CanAddCategoryCommandExecute(object p)
        {
            if (NewCategoryTitle == null)
                return false;
            return true;
        }
        private async void OnAddCategoryCommandExecuted(object p)
        {
            var data = await ProductCategoryViewModel.CreateProductCategory(NewCategoryTitle);
            if (data != null) UpdateData();
        }


        public RelayCommand OpenPanelProductAddCommand { get; set; }
        private bool CanOpenPanelProductAddCommandExecute(object p)
        {
            if (NewCategoryTitle == null)
                return false;
            return true;

        }
        private async void OnOpenPanelProductAddCommandExecuted(object p)
        {
            var data = await ProductCategoryViewModel.CreateProductCategory(NewCategoryTitle);
        }

        public RelayCommand ClosePanelProductAddCommand { get; set; }
        private bool CanClosePanelProductAddCommandExecute(object p) => true;
        private void OnClosePanelProductAddCommandExecuted(object p)
        {

        }


        public RelayCommand AddProductCommand { get; set; }
        private bool CanAddProductCommandExecute(object p) => true;
        private async void OnAddProductCommandExecuted(object p)
        {
            var data = await ProductViewModel.CreateProduct(ArticulCodeProduct, TitleProduct, DescriptionProduct, ImageProduct, BrandProduct, IdCategoryProduct.Id, PurchaseСostProduct, SellCostProduct, AmountProduct);
            if (data != null) UpdateData();
        }

        #endregion

        private async void UpdateData()
        {
            CollectionProducts= await ProductViewModel.GetProducts();
            CollectionProductCategories = await ProductCategoryViewModel.GetProductCategory();
        }

#pragma warning disable CS8618
        public StockViewModel()
        {
            AddCategoryCommand = new RelayCommand(OnAddCategoryCommandExecuted, CanAddCategoryCommandExecute);
            AddProductCommand = new RelayCommand(OnAddProductCommandExecuted, CanAddProductCommandExecute);
            OpenPanelProductAddCommand = new RelayCommand(OnOpenPanelProductAddCommandExecuted, CanOpenPanelProductAddCommandExecute);
            ClosePanelProductAddCommand = new RelayCommand(OnClosePanelProductAddCommandExecuted, CanClosePanelProductAddCommandExecute);
        }
    }
}
