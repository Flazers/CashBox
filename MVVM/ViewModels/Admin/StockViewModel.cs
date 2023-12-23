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

        #region VisibilityPanel

        private Visibility _panelCreateProductVisibility = Visibility.Collapsed;
        public Visibility PanelCreateProductVisibility
        {
            get => _panelCreateProductVisibility;
            set => Set(ref _panelCreateProductVisibility, value);
        }

        private Visibility _panelCurrentProductVisibility = Visibility.Visible;
        public Visibility PanelCurrentProductVisibility
        {
            get => _panelCurrentProductVisibility;
            set => Set(ref _panelCurrentProductVisibility, value);
        }

        private Visibility _panelEditProductVisibility = Visibility.Collapsed;
        public Visibility PanelEditProductVisibility
        {
            get => _panelEditProductVisibility;
            set => Set(ref _panelEditProductVisibility, value);
        }

        #endregion

        #region ProductData

        private string _newCategoryTitle = string.Empty;
        public string NewCategoryTitle
        {
            get => _newCategoryTitle;
            set => Set(ref _newCategoryTitle, value);
        }

        private int _idProduct;
        public int IdProduct
        {
            get => _idProduct;
            set => Set(ref _idProduct, value);
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

        #endregion

        private string _imageStringProduct = "Выбрать фото товара";
        public string ImageStringProduct
        {
            get => _imageStringProduct;
            set => Set(ref _imageStringProduct, value);
        }

        private ObservableCollection<ProductViewModel> _collectionProducts = new(ProductViewModel.GetProducts().Result);
        public ObservableCollection<ProductViewModel> CollectionProducts
        {
            get => _collectionProducts;
            set => Set(ref _collectionProducts, value);
        }
        private ObservableCollection<ProductCategoryViewModel> _collectionProductCategories = new(ProductCategoryViewModel.GetProductCategory().Result);
        public ObservableCollection<ProductCategoryViewModel> CollectionProductCategories
        {
            get => _collectionProductCategories;
            set => Set(ref _collectionProductCategories, value);
        }

        private ProductViewModel? _selectedProduct;
        public ProductViewModel? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                if (_selectedProduct != null && PanelEditProductVisibility != Visibility.Visible && PanelCreateProductVisibility != Visibility.Visible)
                {
                    IdProduct = _selectedProduct.Id;
                    ArticulCodeProduct = _selectedProduct.ArticulCode;
                    BrandProduct = _selectedProduct.Brand;
                    TitleProduct = _selectedProduct.Title;
                    DescriptionProduct = _selectedProduct.Description;
                    ImageProduct = _selectedProduct.Image;
                    PurchaseСostProduct = _selectedProduct.PurchaseСost;
                    SellCostProduct = _selectedProduct.SellCost;
                    IdCategoryProduct = CollectionProductCategories.FirstOrDefault(x => x.Id == _selectedProduct.CategoryId)!;
                    AmountProduct = _selectedProduct.Stock!.Amount;
                    IsAvailableProduct = _selectedProduct.isAvailable;
                }
                OnPropertyChanged();
            }
        }

        private ProductCategoryViewModel _selectedProductCategory;
        public ProductCategoryViewModel SelectedProductCategory
        {
            get => _selectedProductCategory;
            set
            {
                _selectedProductCategory = value;
                CollectionProducts = new(ProductViewModel.GetProducts().Result);
                if (_selectedProductCategory != null)
                    CollectionProducts = new(_collectionProducts.Where(x => x.CategoryId == SelectedProductCategory.Id).ToList());
                OnPropertyChanged();
            }
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
            if (data != null) CollectionProductCategories.Add(data);
        }


        public RelayCommand OpenPanelProductCreateCommand { get; set; }
        private bool CanOpenPanelProductCreateCommandExecute(object p) => true;
        private void OnOpenPanelProductCreateCommandExecuted(object p)
        {
            ArticulCodeProduct = string.Empty;
            BrandProduct = string.Empty;
            TitleProduct = string.Empty;
            DescriptionProduct = string.Empty;
            ImageProduct = null;
            PurchaseСostProduct = 0;
            SellCostProduct = 0;
            IdCategoryProduct = null;
            AmountProduct = 0;
            IsAvailableProduct = true;

            PanelCreateProductVisibility = Visibility.Visible;
            PanelCurrentProductVisibility = Visibility.Collapsed;
            PanelEditProductVisibility = Visibility.Collapsed;
        }

        public RelayCommand OpenPanelProductEditCommand { get; set; }
        private bool CanOpenPanelProductEditCommandExecute(object p) => true;
        private void OnOpenPanelProductEditCommandExecuted(object p)
        {
            PanelCreateProductVisibility = Visibility.Collapsed;
            PanelCurrentProductVisibility = Visibility.Collapsed;
            PanelEditProductVisibility = Visibility.Visible;
        }

        public RelayCommand ClosePanelProductCommand { get; set; }
        private bool CanClosePanelProductCommandExecute(object p) => true;
        private void OnClosePanelProductCommandExecuted(object p)
        {
            PanelCreateProductVisibility = Visibility.Collapsed;
            PanelCurrentProductVisibility = Visibility.Visible;
            PanelEditProductVisibility = Visibility.Collapsed;
        }

        public RelayCommand AddProductCommand { get; set; }
        private bool CanAddProductCommandExecute(object p) => true;
        private async void OnAddProductCommandExecuted(object p)
        {
            var data = await ProductViewModel.CreateProduct(ArticulCodeProduct, TitleProduct, DescriptionProduct, ImageProduct, BrandProduct, IdCategoryProduct.Id, PurchaseСostProduct, SellCostProduct, AmountProduct);
            if (data != null)
            {
                CollectionProducts.Add(data);
                SelectedProduct = data;
                MessageBox.Show("Товар добавлен", "Успех");
                OnClosePanelProductCommandExecuted(0);
            }
        }

        public RelayCommand EditProductCommand { get; set; }
        private bool CanEditProductCommandExecute(object p) => true;
        private async void OnEditProductCommandExecuted(object p)
        {
            var data = await ProductViewModel.UpdateProduct(IdProduct, ArticulCodeProduct, TitleProduct, DescriptionProduct, ImageProduct, BrandProduct, IdCategoryProduct.Id, PurchaseСostProduct, SellCostProduct, AmountProduct);
            if (data != null)
            {
                CollectionProducts = new(ProductViewModel.GetProducts().Result);
                SelectedProduct = data;
                MessageBox.Show("Товар обновлен", "Успех");
                OnClosePanelProductCommandExecuted(0);
            }
        }

        public RelayCommand RemoveProductCommand { get; set; }
        private bool CanRemoveProductCommandExecute(object p) => true;
        private async void OnRemoveProductCommandExecuted(object p)
        {
            var data = await ProductViewModel.RemoveProduct(IdProduct);
            if (data != null)
            {
                CollectionProducts = new(ProductViewModel.GetProducts().Result);
                SelectedProduct = null;
                MessageBox.Show("Товар удален", "Успех");
                OnClosePanelProductCommandExecuted(0);
            }
        }

        #endregion

#pragma warning disable CS8618
        public StockViewModel()
        {
            AddCategoryCommand = new RelayCommand(OnAddCategoryCommandExecuted, CanAddCategoryCommandExecute);
            AddProductCommand = new RelayCommand(OnAddProductCommandExecuted, CanAddProductCommandExecute);
            EditProductCommand = new RelayCommand(OnEditProductCommandExecuted, CanEditProductCommandExecute);
            RemoveProductCommand = new RelayCommand(OnRemoveProductCommandExecuted, CanRemoveProductCommandExecute);
            OpenPanelProductCreateCommand = new RelayCommand(OnOpenPanelProductCreateCommandExecuted, CanOpenPanelProductCreateCommandExecute);
            OpenPanelProductEditCommand = new RelayCommand(OnOpenPanelProductEditCommandExecuted, CanOpenPanelProductEditCommandExecute);
            ClosePanelProductCommand = new RelayCommand(OnClosePanelProductCommandExecuted, CanClosePanelProductCommandExecute);
        }
    }
}
