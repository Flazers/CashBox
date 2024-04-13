using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.Models;
using Cashbox.MVVM.ViewModels.Data;
using ExcelDataReader;
using Microsoft.Win32;
using ScottPlot.Statistics;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace Cashbox.MVVM.ViewModels.Admin
{
    public class StockViewModel : ViewModelBase
    {
        #region Props

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

        #region ProductData

        private ProductViewModel? _productData;
        public ProductViewModel? ProductData
        {
            get => _productData;
            set => Set(ref _productData, value);
        }

        private bool _isAvailable;
        public bool IsAvailable
        {
            get => _isAvailable;
            set => Set(ref _isAvailable, value);
        }

        private int _amount;
        public int Amount
        {
            get => _amount;
            set => Set(ref _amount, value);
        }

        #endregion

        private ObservableCollection<ProductViewModel> _collectionProducts;
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

        private ObservableCollection<ProductCategoryViewModel> _collectionProductCategories;
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

        public RelayCommand RemoveCategoryCommand { get; set; }
        private bool CanRemoveCategoryCommandExecute(object p)
        {
            if (SelectedProductCategory == null)
                return false;
            return true;
        }
        private async void OnRemoveCategoryCommandExecuted(object p)
        {
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

        public RelayCommand OpenPanelProductCreateCommand { get; set; }
        private bool CanOpenPanelProductCreateCommandExecute(object p) => true;
        private void OnOpenPanelProductCreateCommandExecuted(object p)
        {
            ProductData = new(new());
            Amount = 0;

            PanelCreateProductVisibility = Visibility.Visible;
            PanelMainProductVisibility = Visibility.Collapsed;
            PanelEditProductVisibility = Visibility.Collapsed;
        }

        public RelayCommand OpenPanelProductEditCommand { get; set; }
        private bool CanOpenPanelProductEditCommandExecute(object p) => true;
        private void OnOpenPanelProductEditCommandExecuted(object p)
        {
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
            if (ProductData.CategoryId == 0) ProductData.CategoryId = 1;

            var data = await ProductViewModel.CreateProduct(ProductData, Amount);
            if (data == null)
                return;
            Update();
            OnClosePanelProductCommandExecuted(0);
            SelectedProduct = CollectionProducts.FirstOrDefault(x => x == data);
            if (SelectedProductCategory != null && SelectedProductCategory.Id != data.Category.Id)
                SelectedProductCategory = CollectionProductCategories.FirstOrDefault(x => x.Id == data.Category.Id);
            MessageBox.Show("Товар добавлен", "Успех");
        }

        public RelayCommand EditProductCommand { get; set; }
        private bool CanEditProductCommandExecute(object p) => true;
        private async void OnEditProductCommandExecuted(object p)
        {
            var data = await ProductViewModel.UpdateProduct(SelectedProduct, SelectedProduct.Stock.Amount);
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

        public async void Update()
        {
            CollectionProducts = new(await ProductViewModel.GetProducts());
            if (SelectedProductCategory != null)
                if (SelectedProductCategory.Category == "Все категории")
                    CollectionProducts = new(await ProductViewModel.GetProducts());
                else
                    CollectionProducts = new(_collectionProducts.Where(x => x.CategoryId == SelectedProductCategory?.Id).ToList());
        }

        public RelayCommand ImportProductDataCommand { get; set; }
        private bool CanImportProductDataCommandExecute(object p) => true;
        private void OnImportProductDataCommandExecuted(object p)
        {
            List<ProductViewModel> prod = [];
            OpenFileDialog openFileDialog = new() { Filter = "EXCEL Files (*.xlsx)|*.xlsx|EXCEL Files 2003 (*.xls)|*.xls|All files (*.*)|*.*", RestoreDirectory = true };
            bool? resultOpen = openFileDialog.ShowDialog();
            if (resultOpen == true)
            {
                try
                {
                    IExcelDataReader edr;
                    using var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                    using var reader = ExcelReaderFactory.CreateReader(stream);
                    var extension = openFileDialog.FileName.Substring(openFileDialog.FileName.LastIndexOf('.'));

                    if (extension == ".xlsx")
                        edr = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    else if (extension == ".xls")
                        edr = ExcelReaderFactory.CreateBinaryReader(stream);
                    else
                        edr = ExcelReaderFactory.CreateOpenXmlReader(stream);

                    do
                    {
                        reader.Read();
                        ProductCategoryViewModel category = CollectionProductCategories.FirstOrDefault(x => x.Category == reader.Name);
                        if (category == null) break;
                        while (reader.Read())
                        {
                            Product product = new()
                            {
                                Brand = reader.GetString(1),
                                Title = reader.GetString(2),
                                Description = reader.GetString(3),
                                PurchaseСost = reader.GetDouble(4),
                                SellCost = reader.GetDouble(5),
                                CategoryId = category.Id,
                                IsAvailable = true
                            };
                            if (reader.GetValue(0) == null)
                                product.ArticulCode = string.Empty;
                            else
                                product.ArticulCode = (reader.GetDouble(0)).ToString();
                                prod.Add(new(product));
                        }
                    } while (reader.NextResult());
                    edr.Close();

                }
                catch (IOException)
                {
                    AppCommand.ErrorMessage("Процесс используется другим приложением (Возможно файл открыт).");
                    return;
                }
            }

            
        }
        #endregion

        #endregion

        public StockViewModel()
        {
            List<ProductCategoryViewModel> productcategory = [];
            productcategory.Add(ProductCategoryViewModel.NewExample("Все категории").Result);
            productcategory.AddRange(ProductCategoryViewModel.GetProductCategory().Result);
            CollectionProductCategories = new(productcategory);

            RemoveCategoryCommand = new RelayCommand(OnRemoveCategoryCommandExecuted, CanRemoveCategoryCommandExecute);
            AddProductCommand = new RelayCommand(OnAddProductCommandExecuted, CanAddProductCommandExecute);
            EditProductCommand = new RelayCommand(OnEditProductCommandExecuted, CanEditProductCommandExecute);
            RemoveProductCommand = new RelayCommand(OnRemoveProductCommandExecuted, CanRemoveProductCommandExecute);
            OpenPanelProductCreateCommand = new RelayCommand(OnOpenPanelProductCreateCommandExecuted, CanOpenPanelProductCreateCommandExecute);
            OpenPanelProductEditCommand = new RelayCommand(OnOpenPanelProductEditCommandExecuted, CanOpenPanelProductEditCommandExecute);
            ClosePanelProductCommand = new RelayCommand(OnClosePanelProductCommandExecuted, CanClosePanelProductCommandExecute);
            GetAllProductCommand = new RelayCommand(OnGetAllProductCommandExecuted, CanGetAllProductCommandExecute);
            ImportProductDataCommand = new RelayCommand(OnImportProductDataCommandExecuted, CanImportProductDataCommandExecute);
        }
    }
}
