using Aspose.Cells;
using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.ViewModels.Data;
using ExcelDataReader;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows;

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

        #endregion

        private bool _isShowAllProduct = false;
        public bool IsShowAllProduct
        {
            get => _isShowAllProduct;
            set
            {
                _isShowAllProduct = value;
                Update();
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ProductViewModel> _collectionProducts = [];
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

        private ObservableCollection<ProductCategoryViewModel> _collectionProductCategories = [];
        public ObservableCollection<ProductCategoryViewModel> CollectionProductCategories
        {
            get => _collectionProductCategories;
            set => Set(ref _collectionProductCategories, value);
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
                MessageBoxResult result = MessageBox.Show("В выбранной категории присутствуют товары \nНажмите \"Да\"если хотите перенести все товары выбранную категорию \nНажмите \"Нет\" если хотите удалить все товары из категории \nНажмите \"Отмена\" для отмены удаления ", "Предупреджение", MessageBoxButton.YesNoCancel);
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
                UpdateCategory();
                AppCommand.InfoMessage("Категория удалена");
            }

        }
        public RelayCommand GetAllProductCommand { get; set; }
        private bool CanGetAllProductCommandExecute(object p) => true;
        private void OnGetAllProductCommandExecuted(object p)
        {
            SelectedProductCategory = null;
        }


        #region ProductCommand
        public RelayCommand RemoveProductCommand { get; set; }
        private bool CanRemoveProductCommandExecute(object p) => true;
        private async void OnRemoveProductCommandExecuted(object p)
        {
            var data = await ProductViewModel.RemoveProduct((int)p);
            if (data != null)
            {
                Update();
                AppCommand.InfoMessage("Товар удален");
            }
        }

        public async void Update()
        {
            var list = await ProductViewModel.GetProducts(IsShowAllProduct);
            CollectionProducts = new(list.OrderBy(x => x.Stock.Amount));
            if (SelectedProductCategory == null)
                return;
            if (SelectedProductCategory.Category != "Все категории")
                CollectionProducts = new(CollectionProducts.Where(x => x.CategoryId == SelectedProductCategory?.Id).ToList());
        }

        public RelayCommand EditProductDataCommand { get; set; }
        private bool CanEditProductDataCommandExecute(object p) => true;
        private async void OnEditProductDataCommandExecuted(object p)
        {
            List<ProductViewModel> products = await ProductViewModel.GetProducts(false);
            OpenFileDialog openFileDialog = new() { Filter = "EXCEL Files (*.xlsx)|*.xlsx|EXCEL Files 2003 (*.xls)|*.xls|All files (*.*)|*.*", RestoreDirectory = true };
            bool? resultOpen = openFileDialog.ShowDialog();
            if (resultOpen == true)
            {
                try
                {
                    IExcelDataReader edr;
                    using FileStream stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                    using IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
                    string? extension = openFileDialog.FileName[openFileDialog.FileName.LastIndexOf('.')..];

                    if (extension == ".xlsx")
                        edr = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    else if (extension == ".xls")
                        edr = ExcelReaderFactory.CreateBinaryReader(stream);
                    else
                    {
                        AppCommand.WarningMessage("Данные должны быть в формате xlsx или xls");
                        return;
                    }

                    do
                    {
                        if (reader.Name == "Evaluation Warning")
                            break;
                        ProductCategoryViewModel category = CollectionProductCategories.FirstOrDefault(x => x.Category == reader.Name);
                        category ??= await ProductCategoryViewModel.CreateProductCategory(reader.Name);
                        reader.Read();
                        int line = 2;
                        while (reader.Read())
                        {
                            string error = string.Empty;
                            string id = reader.GetValue(0).ToString();
                            if (string.IsNullOrEmpty(id))
                                error += "id, ";
                            string articule = reader.GetString(1);
                            string brand = reader.GetString(2);
                            if (string.IsNullOrEmpty(brand))
                                error += "Производитель, ";
                            string title = reader.GetString(3);
                            if (string.IsNullOrEmpty(title))
                                error += "Название, ";
                            string description = reader.GetString(4);
                            if (string.IsNullOrEmpty(description))
                                error += "Описание, ";
                            if (error.Length > 30)
                                continue;
                            if (!double.TryParse(reader.GetValue(5).ToString(), out double sellcost))
                                error += "Продажа, ";
                            if (!int.TryParse(reader.GetValue(6).ToString(), out int amount))
                                error += "Колличество, ";
                            if (!double.TryParse(reader.GetValue(7).ToString(), out double IsAvailable) || IsAvailable < 0 || IsAvailable > 1)
                                error += "В продаже / Снят с продаж, ";

                            if (!string.IsNullOrEmpty(error))
                            {
                                AppCommand.ErrorMessage($"Ошибка в категории{reader.Name}, строка {line}, полe(-ях): {error}");
                                return;
                            }

                            ProductViewModel readedproduct = new(new());
                            if (string.IsNullOrEmpty(id))
                            {
                                AppCommand.WarningMessage($"Не найден товар из строки {line}.");
                                continue;
                            }
                                
                            readedproduct = products.FirstOrDefault(x => x.Id == int.Parse(id));
                            if (readedproduct != null)
                            {
                                readedproduct.Brand = brand;
                                readedproduct.Title = title;
                                readedproduct.Description = description;
                                readedproduct.SellCost = sellcost;
                                readedproduct.CategoryId = category.Id;
                                readedproduct.AmountRes = amount;
                                if (IsAvailable == 1) readedproduct.IsAvailable = true;
                                else readedproduct.IsAvailable = false;
                                if (articule == null) readedproduct.ArticulCode = string.Empty;
                                else readedproduct.ArticulCode = articule;
                            }
                            line++;
                        }
                    } while (reader.NextResult());
                    edr.Close();
                    if (await ProductViewModel.EditProduct(products))
                        AppCommand.InfoMessage("Товары отредактированы");
                    UpdateCategory();
                    Update();
                }
                catch (IOException)
                {
                    AppCommand.ErrorMessage("Процесс используется другим приложением (Возможно файл открыт).");
                    return;
                }
            }
        }

        public RelayCommand ImportProductDataCommand { get; set; }
        private bool CanImportProductDataCommandExecute(object p) => true;
        private async void OnImportProductDataCommandExecuted(object p)
        {
            List<ProductViewModel> products = await ProductViewModel.GetProducts(true);
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
                    {
                        AppCommand.WarningMessage("Данные должны быть в формате xlsx или xls");
                        return;
                    }

                    do
                    {
                        if (reader.Name == "Evaluation Warning")
                            break;
                        ProductCategoryViewModel category = CollectionProductCategories.FirstOrDefault(x => x.Category == reader.Name);
                        category ??= await ProductCategoryViewModel.CreateProductCategory(reader.Name);
                        reader.Read();
                        int addedcount = 0;
                        int editedcount = 0;
                        int line = 2;
                        while (reader.Read())
                        {
                            try
                            {
                                string error = string.Empty;
                                string articule = reader.GetString(1);
                                string brand = reader.GetString(2);
                                if (string.IsNullOrEmpty(brand))
                                    error += "Производитель, ";
                                string title = reader.GetString(3);
                                if (string.IsNullOrEmpty(title))
                                    error += "Название, ";
                                string description = reader.GetString(4);
                                if (string.IsNullOrEmpty(description))
                                    error += "Описание, ";
                                if (error.Length > 30)
                                    continue;
                                if (!double.TryParse(reader.GetValue(5).ToString(), out double sellcost))
                                    error += "Продажа, ";
                                if (!int.TryParse(reader.GetValue(6).ToString(), out int amount))
                                    error += "Колличество, ";
                                if (!double.TryParse(reader.GetValue(7).ToString(), out double IsAvailable) || IsAvailable < 0 || IsAvailable > 1)
                                    error += "В продаже / Снят с продаж, ";

                                if (!string.IsNullOrEmpty(error))
                                {
                                    AppCommand.ErrorMessage($"Ошибка в категории{reader.Name}, строка {line}, полe(-ях): {error}");
                                    return;
                                }


                                ProductViewModel product = products.FirstOrDefault(x => x.Brand == brand && x.Title == title && x.Description == description);
                                if (product != null)
                                {
                                    product.SellCost = sellcost;
                                    product.AmountRes += amount;
                                    editedcount++;
                                }
                                else
                                {
                                    product = new(
                                        new()
                                        {
                                            Brand = brand,
                                            Title = title,
                                            Description = description,
                                            SellCost = sellcost,
                                            CategoryId = category.Id,
                                        })
                                    { AmountRes = amount };

                                    if (articule == null) product.ArticulCode = string.Empty;
                                    else product.ArticulCode = articule;

                                    if (IsAvailable == 1) product.IsAvailable = true;
                                    else product.IsAvailable = false;

                                    products.Add(product);
                                    addedcount++;
                                }
                                line++;
                            }
                            catch (NullReferenceException ex)
                            {
                                AppCommand.ErrorMessage(ex.Message);
                                continue;
                            }
                        }
                        AppCommand.InfoMessage($"Категория \"{reader.Name}\" \nДобавлено новых товаров: {addedcount} \nОтредактировано товаров: {editedcount}");
                    } while (reader.NextResult());
                    edr.Close();
                    await ProductViewModel.ImportProduct(products);
                    UpdateCategory();
                    Update();
                }
                catch (IOException)
                {
                    AppCommand.WarningMessage("Процесс используется другим приложением (Возможно файл открыт).");
                    return;
                }
                catch (Exception ex)
                {
                    AppCommand.ErrorMessage(ex.Message);
                    return;
                }
            }
        }

        public void SetRowStyle(Workbook wb, int i, int cell, Aspose.Cells.Style style)
        {
            wb.Worksheets[i].Cells[$"A{cell}"].SetStyle(style);
            wb.Worksheets[i].Cells[$"B{cell}"].SetStyle(style);
            wb.Worksheets[i].Cells[$"C{cell}"].SetStyle(style);
            wb.Worksheets[i].Cells[$"D{cell}"].SetStyle(style);
            wb.Worksheets[i].Cells[$"E{cell}"].SetStyle(style);
            wb.Worksheets[i].Cells[$"F{cell}"].SetStyle(style);
            wb.Worksheets[i].Cells[$"G{cell}"].SetStyle(style);
            wb.Worksheets[i].Cells[$"H{cell}"].SetStyle(style);
        }

        public void AddProductRow(Workbook wb, int i, int cell, ProductViewModel product)
        {
            wb.Worksheets[i].Cells[$"A{cell}"].PutValue(product.Id);
            wb.Worksheets[i].Cells[$"B{cell}"].PutValue(product.ArticulCode);
            wb.Worksheets[i].Cells[$"C{cell}"].PutValue(product.Brand);
            wb.Worksheets[i].Cells[$"D{cell}"].PutValue(product.Title);
            wb.Worksheets[i].Cells[$"E{cell}"].PutValue(product.Description);
            wb.Worksheets[i].Cells[$"F{cell}"].PutValue(product.SellCost);
            wb.Worksheets[i].Cells[$"G{cell}"].PutValue(product.Stock.Amount);
            if (product.IsAvailable)
                wb.Worksheets[i].Cells[$"H{cell}"].PutValue(1);
            else
                wb.Worksheets[i].Cells[$"H{cell}"].PutValue(0);
        }

        public RelayCommand ExportProductDataCommand { get; set; }
        private bool CanExportProductDataCommandExecute(object p) => true;
        private async void OnExportProductDataCommandExecuted(object p)
        {
            SaveFileDialog sfd = new()
            {
                Filter = "Excel Standart(*.xlsx)|*.xlsx|Excel 97-2003 (*.xls)|*.xls|All files(*.*)|*.*",
                FileName = "ExportedProductData"
            };
            sfd.ShowDialog();

            List<ProductViewModel> products = await ProductViewModel.GetProducts(true);
            List<ProductCategoryViewModel> category = await ProductCategoryViewModel.GetProductCategory();

            CellsFactory cellsFactory = new();
            Aspose.Cells.Style style = cellsFactory.CreateStyle();
            style.Borders.SetStyle(CellBorderType.Thin);
            style.Borders[BorderType.DiagonalDown].LineStyle = CellBorderType.None;
            style.Borders[BorderType.DiagonalUp].LineStyle = CellBorderType.None;
            style.Borders.SetColor(Color.Black);

            CellsFactory cellsHeader = new();
            Aspose.Cells.Style styleHeader = cellsHeader.CreateStyle();
            styleHeader.Borders.SetStyle(CellBorderType.Thin);
            styleHeader.Borders[BorderType.DiagonalDown].LineStyle = CellBorderType.None;
            styleHeader.Borders[BorderType.DiagonalUp].LineStyle = CellBorderType.None;
            styleHeader.Borders.SetColor(Color.Black);
            styleHeader.HorizontalAlignment = TextAlignmentType.Center;
            styleHeader.VerticalAlignment = TextAlignmentType.Center;
            styleHeader.SetPatternColor(BackgroundType.Solid, Color.LightGreen, Color.LightGreen);


            void SetRowHeader(Workbook wb, int i, int cell, string title, int width)
            {
                Worksheet sheet = wb.Worksheets[i];
                sheet.Cells[0, cell].PutValue(title);
                sheet.Cells.SetColumnWidth(cell, width);
            }

            if (sfd.FileName != "")
            {
                Workbook wb = new();

                for (int i = 0; i < category.Count; i++)
                {
                    if (i == 0)
                        wb.Worksheets[i].Name = category[i].Category;
                    else
                        wb.Worksheets.Add(category[i].Category);
                    SetRowHeader(wb, i, 0, "id*", 10);
                    SetRowHeader(wb, i, 1, "Артикул*", 10);
                    SetRowHeader(wb, i, 2, "Производитель", 17);
                    SetRowHeader(wb, i, 3, "Название", 27);
                    SetRowHeader(wb, i, 4, "Описание", 17);
                    SetRowHeader(wb, i, 5, "Продажа", 10);
                    SetRowHeader(wb, i, 6, "Колличество", 14);
                    SetRowHeader(wb, i, 7, "В продаже / Снят с продаж", 26);

                    wb.Worksheets[i].Cells.SetRowHeight(0, 30);
                    SetRowStyle(wb, i, 1, styleHeader);
                }

                for (int i = 0; i < category.Count; i++)
                {
                    int count = 1;
                    int position = 2;
                    foreach (ProductViewModel item in products.Where(x => x.CategoryId == category[i].Id))
                    {
                        AddProductRow(wb, i, position, item);
                        SetRowStyle(wb, i, position, style);
                        wb.Worksheets[i].Cells.SetRowHeight(count, 16);
                        position++;
                    }
                }

                wb.Save(sfd.FileName);
                AppCommand.InfoMessage("Готово");
            }
        }

        public RelayCommand FileForImportCommand { get; set; }
        private bool CanFileForImportCommandExecute(object p) => true;
        private void OnFileForImportCommandExecuted(object p)
        {
            SaveFileDialog sfd = new()
            {
                Filter = "Excel Standart(*.xlsx)|*.xlsx|Excel 97-2003 (*.xls)|*.xls|All files(*.*)|*.*",
                FileName = "ExampleProductData"
            };
            sfd.ShowDialog();

            CellsFactory cellsFactory = new();
            Aspose.Cells.Style style = cellsFactory.CreateStyle();
            style.Borders.SetStyle(CellBorderType.Thin);
            style.Borders[BorderType.DiagonalDown].LineStyle = CellBorderType.None;
            style.Borders[BorderType.DiagonalUp].LineStyle = CellBorderType.None;
            style.Borders.SetColor(Color.Black);

            CellsFactory cellsHeader = new();
            Aspose.Cells.Style styleHeader = cellsHeader.CreateStyle();
            styleHeader.Borders.SetStyle(CellBorderType.Thin);
            styleHeader.Borders[BorderType.DiagonalDown].LineStyle = CellBorderType.None;
            styleHeader.Borders[BorderType.DiagonalUp].LineStyle = CellBorderType.None;
            styleHeader.Borders.SetColor(Color.Black);
            styleHeader.HorizontalAlignment = TextAlignmentType.Center;
            styleHeader.VerticalAlignment = TextAlignmentType.Center;
            styleHeader.SetPatternColor(BackgroundType.Solid, Color.LightGreen, Color.LightGreen);

            void SetRowHeader(Workbook wb, int i, int cell, string title, int width)
            {
                Worksheet sheet = wb.Worksheets[i];
                sheet.Cells[0, cell].PutValue(title);
                sheet.Cells.SetColumnWidth(cell, width);
            }

            if (sfd.FileName != string.Empty)
            {
                Workbook wb = new();

                wb.Worksheets[0].Name = "Категория";
                SetRowHeader(wb, 0, 0, "id*", 10);
                SetRowHeader(wb, 0, 1, "Артикул*", 10);
                SetRowHeader(wb, 0, 2, "Производитель", 17);
                SetRowHeader(wb, 0, 3, "Название", 27);
                SetRowHeader(wb, 0, 4, "Описание", 17);
                SetRowHeader(wb, 0, 5, "Продажа", 10);
                SetRowHeader(wb, 0, 6, "Колличество", 14);
                SetRowHeader(wb, 0, 7, "В продаже / Снят с продаж", 26);
                wb.Worksheets[0].Cells.SetRowHeight(0, 30);
                SetRowStyle(wb, 0, 1, styleHeader);

                wb.Worksheets[0].Cells[$"A2"].PutValue(1);
                wb.Worksheets[0].Cells[$"B2"].PutValue("ES2134");
                wb.Worksheets[0].Cells[$"C2"].PutValue("Хаски");
                wb.Worksheets[0].Cells[$"D2"].PutValue("Киви яблоко");
                wb.Worksheets[0].Cells[$"E2"].PutValue("20 mg strong");
                wb.Worksheets[0].Cells[$"F2"].PutValue("450");
                wb.Worksheets[0].Cells[$"G2"].PutValue(10);
                wb.Worksheets[0].Cells[$"H2"].PutValue(1);

                wb.Save(sfd.FileName);

                AppCommand.InfoMessage("Шаблон готов");
                File.OpenRead(sfd.FileName);
            }
        }

        #endregion

        private async void UpdateCategory()
        {
            List<ProductCategoryViewModel> productcategory = [];
            ProductCategoryViewModel AllCategory = await ProductCategoryViewModel.NewExample("Все категории");
            productcategory.Add(AllCategory);
            productcategory.AddRange(await ProductCategoryViewModel.GetProductCategory());
            CollectionProductCategories = new(productcategory);
            SelectedProductCategory = CollectionProductCategories[0];
        }

        #endregion

        public StockViewModel()
        {
            UpdateCategory();
            RemoveCategoryCommand = new RelayCommand(OnRemoveCategoryCommandExecuted, CanRemoveCategoryCommandExecute);
            RemoveProductCommand = new RelayCommand(OnRemoveProductCommandExecuted, CanRemoveProductCommandExecute);
            GetAllProductCommand = new RelayCommand(OnGetAllProductCommandExecuted, CanGetAllProductCommandExecute);
            ImportProductDataCommand = new RelayCommand(OnImportProductDataCommandExecuted, CanImportProductDataCommandExecute);
            ExportProductDataCommand = new RelayCommand(OnExportProductDataCommandExecuted, CanExportProductDataCommandExecute);
            EditProductDataCommand = new RelayCommand(OnEditProductDataCommandExecuted, CanEditProductDataCommandExecute);
            FileForImportCommand = new RelayCommand(OnFileForImportCommandExecuted, CanFileForImportCommandExecute);
        }
    }
}
