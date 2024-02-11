using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.Models;
using Cashbox.MVVM.ViewModels.Data;
using Cashbox.MVVM.Views.Pages.Admin;
using Cashbox.Service;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Cashbox.MVVM.ViewModels.Admin
{
    public class StockViewModel : ViewModelBase
    {
        #region Props
        public static UserViewModel? User { get => Models.User.CurrentUser; }

        #region VisibilityPanel

        private Visibility _panelMainProductVisibility = Visibility.Visible;
        public Visibility PanelMainProductVisibility
        {
            get => _panelMainProductVisibility;
            set => Set(ref _panelMainProductVisibility, value);
        }

        private Visibility _panelCreateProductVisibility = Visibility.Collapsed;
        public Visibility PanelCreateProductVisibility
        {
            get => _panelCreateProductVisibility;
            set => Set(ref _panelCreateProductVisibility, value);
        }

        private Visibility _panelCurrentProductVisibility = Visibility.Collapsed;
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

        private Visibility _panelContentProductVisibility = Visibility.Collapsed;
        public Visibility PanelContentProductVisibility
        {
            get => _panelContentProductVisibility;
            set => Set(ref _panelContentProductVisibility, value);
        }


        #endregion

        private string _newCategoryTitle = string.Empty;
        public string NewCategoryTitle
        {
            get => _newCategoryTitle;
            set => Set(ref _newCategoryTitle, value);
        }

        #region ProductData

        private string? _articulCode;
        public string? ArticulCode
        {
            get => _articulCode;
            set => Set(ref _articulCode, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }

        private byte[] _image;
        public byte[] Image
        {
            get => _image;
            set => Set(ref _image, value);
        }

        private byte[] _imageProduct;
        public byte[] ImageProduct
        {
            get => _imageProduct;
            set => Set(ref _imageProduct, value);
        }

        private string _brand;
        public string Brand
        {
            get => _brand;
            set => Set(ref _brand, value);
        }

        private ProductCategoryViewModel? _category;
        public ProductCategoryViewModel? Category
        {
            get => _category;
            set => Set(ref _category, value);
        }

        private double _purchaseСost;
        public double PurchaseСost
        {
            get => _purchaseСost;
            set => Set(ref _purchaseСost, value);
        }

        private double _sellCost;
        public double SellCost
        {
            get => _sellCost;
            set => Set(ref _sellCost, value);
        }

        private int _amount;
        public int Amount
        {
            get => _amount;
            set => Set(ref _amount, value);
        }

        private bool _isAvailable;
        public bool IsAvailable
        {
            get => _isAvailable;
            set => Set(ref _isAvailable, value);
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
            get
            {
                PanelContentProductVisibility = Visibility.Collapsed;
                if (_collectionProducts.Any())
                    PanelContentProductVisibility = Visibility.Visible;
                return _collectionProducts;
            }
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
                if (PanelEditProductVisibility == Visibility.Collapsed && PanelCreateProductVisibility == Visibility.Collapsed)
                    _selectedProduct = value;
                PanelCurrentProductVisibility = Visibility.Visible;
                if (_selectedProduct != null)
                    PanelCurrentProductVisibility = Visibility.Visible;
                else
                    PanelCurrentProductVisibility = Visibility.Collapsed;
                OnPropertyChanged();
            }
        }

        private ProductCategoryViewModel? _selectedProductCategory;
        public ProductCategoryViewModel? SelectedProductCategory
        {
            get => _selectedProductCategory;
            set
            {
                _selectedProductCategory = value;
                Update();
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

        public RelayCommand RemoveCategoryCommand { get; set; }
        private bool CanRemoveCategoryCommandExecute(object p) 
        { 
            if (SelectedProductCategory == null) 
                return false;
            return true;
        }
        private async void OnRemoveCategoryCommandExecuted(object p)
        {
            if (SelectedProductCategory.Id <= 1)
            {
                MessageBox.Show("Нельзя удалить категорию по умолчанию");
                return;
            }
            ProductCategoryViewModel data = null;
            if (SelectedProductCategory.Products.Count != 0)
            {
                MessageBoxResult result = MessageBox.Show("В выбранной категории присутствуют товары \nНажмите \"Да\"если хотите перенести все товары в категорию по умолчанию \nНажмите \"Нет\" если хотите удалить все товары из категории \nНажмите \"Отмена\" для отмены удаления ", "Предупреджение", MessageBoxButton.YesNoCancel);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        data = await ProductCategoryViewModel.RemoveProductCategory(SelectedProductCategory.Id, 1);
                        break;
                    case MessageBoxResult.No:
                        data = await ProductCategoryViewModel.RemoveProductCategory(SelectedProductCategory.Id, 2);
                        break;
                    case MessageBoxResult.Cancel:
                        return;
                }
            } 
            else
            {
                data = await ProductCategoryViewModel.RemoveProductCategory(SelectedProductCategory.Id, 3);
            }
            if (data != null)
            {
                SelectedProductCategory = null;
                CollectionProductCategories = new(ProductCategoryViewModel.GetProductCategory().Result);
                MessageBox.Show("Категория удалена", "Успех");
            }

        }
        public RelayCommand GetAllProductCommand { get; set; }
        private bool CanGetAllProductCommandExecute(object p) => true;
        private void OnGetAllProductCommandExecuted(object p)
        {
            SelectedProductCategory = null;
        }

        public RelayCommand AddImageCommand { get; set; }
        private bool CanAddImageCommandExecute(object p) => true;
        private void OnAddImageCommandExecuted(object p)
        {
            OpenFileDialog openFileDialog = new() { Filter = "Все файлы (*.*)|*.*", RestoreDirectory = true };
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                ImageProduct = File.ReadAllBytes(openFileDialog.FileName);
                ImageStringProduct = openFileDialog.FileName;
            }
            else
                ImageProduct = [];
        }

        public RelayCommand EditImageCommand { get; set; }
        private bool CanEditImageCommandExecute(object p) => true;
        private void OnEditImageCommandExecuted(object p)
        {
            OpenFileDialog openFileDialog = new() { Filter = "Все файлы (*.*)|*.*", RestoreDirectory = true };
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                SelectedProduct.Image = File.ReadAllBytes(openFileDialog.FileName);
                ImageStringProduct = openFileDialog.FileName;
            }
        }

        public RelayCommand OpenPanelProductCreateCommand { get; set; }
        private bool CanOpenPanelProductCreateCommandExecute(object p) => true;
        private void OnOpenPanelProductCreateCommandExecuted(object p)
        {
            ArticulCode = string.Empty;
            Title = string.Empty;
            Brand = string.Empty;
            Description = string.Empty;
            Image = [];
            ImageProduct = [];
            Category = null;
            PurchaseСost = 0;
            SellCost = 0;
            Amount = 0;
            ImageStringProduct = "Выбрать фото товара";

            PanelCreateProductVisibility = Visibility.Visible;
            PanelMainProductVisibility = Visibility.Collapsed;
            PanelEditProductVisibility = Visibility.Collapsed;
        }

        public RelayCommand OpenPanelProductEditCommand { get; set; }
        private bool CanOpenPanelProductEditCommandExecute(object p) => true;
        private void OnOpenPanelProductEditCommandExecuted(object p)
        {
            ImageStringProduct = "Выбрать фото товара";

            PanelCreateProductVisibility = Visibility.Collapsed;
            PanelMainProductVisibility = Visibility.Collapsed;
            PanelEditProductVisibility = Visibility.Visible;
        }

        public RelayCommand ClosePanelProductCommand { get; set; }
        private bool CanClosePanelProductCommandExecute(object p) => true;
        private void OnClosePanelProductCommandExecuted(object p)
        {
            PanelCreateProductVisibility = Visibility.Collapsed;
            PanelMainProductVisibility = Visibility.Visible;
            PanelEditProductVisibility = Visibility.Collapsed;
        }

        #region ProductCommand
        public RelayCommand AddProductCommand { get; set; }
        private bool CanAddProductCommandExecute(object p)
        {
            return true;
        }
        private async void OnAddProductCommandExecuted(object p)
        {
            if (ImageProduct != null)
                Image = ImageProduct;
            var data = await ProductViewModel.CreateProduct(ArticulCode, Title, Description, Image, Brand, Category.Id, PurchaseСost, SellCost, Amount);
            if (data != null)
            {
                Update();
                OnClosePanelProductCommandExecuted(0);
                SelectedProduct = CollectionProducts.FirstOrDefault(x => x == data);
                if (SelectedProductCategory != null && SelectedProductCategory.Id != data.Category.Id)
                    SelectedProductCategory = CollectionProductCategories.FirstOrDefault(x => x.Id == data.Category.Id);
                MessageBox.Show("Товар добавлен", "Успех");
            }
        }

        public RelayCommand EditProductCommand { get; set; }
        private bool CanEditProductCommandExecute(object p) => true;
        private async void OnEditProductCommandExecuted(object p)
        {
            if (ImageProduct != null)
                Image = ImageProduct;
            var data = await ProductViewModel.UpdateProduct(SelectedProduct.Id, SelectedProduct.ArticulCode, SelectedProduct.Title, SelectedProduct.Description, SelectedProduct.Image, SelectedProduct.Brand, SelectedProduct.CategoryId, SelectedProduct.PurchaseСost, SelectedProduct.SellCost, SelectedProduct.Stock.Amount);
            if (data != null)
            {
                Update();
                SelectedProduct = data;
                MessageBox.Show("Товар обновлен", "Успех");
                OnClosePanelProductCommandExecuted(0);
            }
        }

        public RelayCommand RemoveProductCommand { get; set; }
        private bool CanRemoveProductCommandExecute(object p) => true;
        private async void OnRemoveProductCommandExecuted(object p)
        {
            var data = await ProductViewModel.RemoveProduct(SelectedProduct.Id);
            if (data != null)
            {
                Update();
                SelectedProduct = null;
                MessageBox.Show("Товар удален", "Успех");
                OnClosePanelProductCommandExecuted(0);
            }
        }

        public void Update()
        {
            CollectionProducts = new(ProductViewModel.GetProducts().Result);
            if (SelectedProductCategory != null)
                CollectionProducts = new(_collectionProducts.Where(x => x.CategoryId == SelectedProductCategory?.Id).ToList());
        }
        #endregion

        #endregion

#pragma warning disable CS8618
        public StockViewModel()
        {
            AddCategoryCommand = new RelayCommand(OnAddCategoryCommandExecuted, CanAddCategoryCommandExecute);
            RemoveCategoryCommand = new RelayCommand(OnRemoveCategoryCommandExecuted, CanRemoveCategoryCommandExecute);
            AddProductCommand = new RelayCommand(OnAddProductCommandExecuted, CanAddProductCommandExecute);
            EditProductCommand = new RelayCommand(OnEditProductCommandExecuted, CanEditProductCommandExecute);
            RemoveProductCommand = new RelayCommand(OnRemoveProductCommandExecuted, CanRemoveProductCommandExecute);
            OpenPanelProductCreateCommand = new RelayCommand(OnOpenPanelProductCreateCommandExecuted, CanOpenPanelProductCreateCommandExecute);
            OpenPanelProductEditCommand = new RelayCommand(OnOpenPanelProductEditCommandExecuted, CanOpenPanelProductEditCommandExecute);
            ClosePanelProductCommand = new RelayCommand(OnClosePanelProductCommandExecuted, CanClosePanelProductCommandExecute);
            GetAllProductCommand = new RelayCommand(OnGetAllProductCommandExecuted, CanGetAllProductCommandExecute);
            AddImageCommand = new RelayCommand(OnAddImageCommandExecuted, CanAddImageCommandExecute);
            EditImageCommand = new RelayCommand(OnEditImageCommandExecuted, CanEditImageCommandExecute);
        }
    }
}
