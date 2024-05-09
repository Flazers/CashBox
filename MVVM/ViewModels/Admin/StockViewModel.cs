using Aspose.Cells;
using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.Models;
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

        private bool canupdate = false;
        private Visibility _panelMainProductVisibility = Visibility.Visible;
        public Visibility PanelMainProductVisibility
        {
            get => _panelMainProductVisibility;
            set => Set(ref _panelMainProductVisibility, value);
        }

        private Visibility _visibilityLoadProduct = Visibility.Collapsed;
        public Visibility VisibilityLoadProduct
        {
            get => _visibilityLoadProduct;
            set => Set(ref _visibilityLoadProduct, value);
        }

        private Visibility _visibilityProduct = Visibility.Collapsed;
        public Visibility VisibilityProduct
        {
            get => _visibilityProduct;
            set => Set(ref _visibilityProduct, value);
        }

        private Visibility _swapUp = Visibility.Collapsed;
        public Visibility SwapUp
        {
            get => _swapUp;
            set => Set(ref _swapUp, value);
        }

        private Visibility _swapDown = Visibility.Visible;
        public Visibility SwapDown
        {
            get => _swapDown;
            set => Set(ref _swapDown, value);
        }

        private ProductViewModel? _productData;
        public ProductViewModel? ProductData
        {
            get => _productData;
            set => Set(ref _productData, value);
        }

        private bool _isShowAllProduct = false;
        public bool IsShowAllProduct
        {
            get => _isShowAllProduct;
            set
            {
                _isShowAllProduct = value;
                OnPropertyChanged();
                if (canupdate)
                    Update().GetAwaiter();
            }
        }

        private string _stringCostImport = string.Empty;
        public string StringCostImport
        {
            get => _stringCostImport;
            set => Set(ref _stringCostImport, value);
        }

        private ObservableCollection<ProductViewModel> _collectionProducts = [];
        public ObservableCollection<ProductViewModel> CollectionProducts
        {
            get
            {
                VisibilityProduct = Visibility.Collapsed;
                if (_collectionProducts.Any())
                    VisibilityProduct = Visibility.Visible;
                return _collectionProducts;
            }
            set => Set(ref _collectionProducts, value);
        }

        private ObservableCollection<ComingProductViewModel> _collectionComing = [];
        public ObservableCollection<ComingProductViewModel> CollectionComing
        {
            get => _collectionComing;
            set => Set(ref _collectionComing, value);
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
                OnPropertyChanged();
                Update().GetAwaiter();
            }
        }

        private string _searchStr = string.Empty;
        public string SearchStr
        {
            get => _searchStr;
            set => Set(ref _searchStr, value);
        }

        private int _sort = 0;
        public int Sort
        {
            get => _sort;
            set
            {
                _sort = value;
                OnPropertyChanged();
                if (canupdate)
                    Update().GetAwaiter();
            }
        }

        private int _productCount = 0;
        public int ProductCount
        {
            get => _productCount;
            set => Set(ref _productCount, value);
        }

        private int _showProductCount = 20;
        public int ShowProductCount
        {
            get => _showProductCount;
            set
            {
                _showProductCount = value;
                OnPropertyChanged();
                if (canupdate)
                    Update().GetAwaiter();
            }
        }

        private int _showedProductCount = 0;
        public int ShowedProductCount
        {
            get => _showedProductCount;
            set => Set(ref _showedProductCount, value);
        }
        #endregion


        #region Command

        public RelayCommand SearchDataCommand { get; set; }
        private bool CanSearchDataCommandExecute(object p) => true;
        private async void OnSearchDataCommandExecuted(object p) => await Update();


        public RelayCommand RemoveCategoryCommand { get; set; }
        private bool CanRemoveCategoryCommandExecute(object p) => true;
        private async void OnRemoveCategoryCommandExecuted(object p)
        {
            if (SelectedProductCategory.Category == "Все категории")
            {
                AppCommand.WarningMessage("Данную категорию нельзя удалить");
                return;
            }
            if (SelectedProductCategory.Products.Count != 0)
            {
                AppCommand.WarningMessage("В выбранной категории присутствуют товары.\n Для удаления выполните экспорт товаров,\n а затем переместите все товары в нужную категорию (лист) и повторите попытку.");
                return;
            }
            if (!await ProductCategoryViewModel.RemoveProductCategory(SelectedProductCategory.Id))
                return;
            UpdateCategory();
            AppCommand.InfoMessage("Категория удалена");
        }

        public RelayCommand SelectCategoryCommand { get; set; }
        private bool CanSelectCategoryCommandExecute(object p)
        {
            if (SelectedProductCategory.Id == (int)p)
                return false;
            return true;
        }
        private void OnSelectCategoryCommandExecuted(object p)
        {
            SelectedProductCategory = CollectionProductCategories.FirstOrDefault(x => x.Id == (int)p);
        }

        public RelayCommand GetAllProductCommand { get; set; }
        private bool CanGetAllProductCommandExecute(object p) => true;
        private void OnGetAllProductCommandExecuted(object p)
        {
            SelectedProductCategory = null;
        }


        #region ProductCommand

        public RelayCommand SortSwapCommand { get; set; }
        private bool CanSortSwapCommandExecute(object p) => true;
        private async void OnSortSwapCommandExecuted(object p)
        {
            if (SwapUp == Visibility.Visible)
            {
                SwapUp = Visibility.Collapsed;
                SwapDown = Visibility.Visible;
            }
            else
            {
                SwapUp = Visibility.Visible;
                SwapDown = Visibility.Collapsed;
            }
            await Update();
        }

        public RelayCommand RemoveProductCommand { get; set; }
        private bool CanRemoveProductCommandExecute(object p) => true;
        private async void OnRemoveProductCommandExecuted(object p)
        {
            var data = await ProductViewModel.RemoveProduct((int)p);
            if (data != null)
            {
                await Update();
                AppCommand.InfoMessage("Товар снят с продажи");
            }
        }

        public RelayCommand ReturnProductCommand { get; set; }
        private bool CanReturnProductCommandExecute(object p) => true;
        private async void OnReturnProductCommandExecuted(object p)
        {
            var data = await ProductViewModel.UnRemoveProduct((int)p);
            if (data != null)
            {
                await Update();
                AppCommand.InfoMessage("Товар возвращен");
            }
        }

        private async Task<List<ProductViewModel>> Analyzer(FileStream stream, bool isEdit)
        {
            List<ProductViewModel> listProducts = new(await ProductViewModel.GetProducts(true));
            List<ProductViewModel> listProductVM = [];
            List<string> errors = [];
            string info = string.Empty;
            using IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);

            do
            {
                if (reader.Name == "Evaluation Warning")
                    continue;
                ProductCategoryViewModel category = CollectionProductCategories.FirstOrDefault(x => x.Category == reader.Name);
                if (category == null)
                    continue;
                reader.Read();
                int addedcount = 0;
                int editedcount = 0;
                int movedcount = 0;
                int line = 1;
                while (reader.Read())
                {
                    line++;
                    string error = string.Empty;
                    int id = 0;
                    string brand = string.Empty;
                    string title = string.Empty;
                    string description = string.Empty;
                    double sellcost = 0;
                    int amount = 0;
                    int IsAvailable = 0;

                    int errorline = 0;
                    for (int i = 0; i <= 6; i++)
                        if (reader.GetValue(i) == null)
                            errorline++;
                    if (errorline == 7)
                        continue;

                    if (isEdit)
                    {
                        if (reader.GetValue(0) == null)
                            error += "Поле \"id\" пустое\n";
                        else if (!int.TryParse(reader.GetValue(0).ToString(), out id))
                            error += "Поле \"id\" должно быть целым числом\n";

                        if (reader.GetValue(6) == null)
                            error += "Поле \"В продаже / Снят с продаж\" пустое\n";
                        else if (!int.TryParse(reader.GetValue(6).ToString(), out IsAvailable) || IsAvailable < 0 || IsAvailable > 1)
                            error += "Поле \"В продаже / Снят с продаж\" допускает только 1 и 0\n";
                    }

                    if (reader.GetValue(1) != null)
                        brand = reader.GetValue(1).ToString();
                    else
                        error += "Поле \"Производитель\" пустое\n";

                    if (reader.GetValue(2) != null)
                        title = reader.GetValue(2).ToString();
                    else
                        error += "Поле \"Название\" пустое\n";

                    if (reader.GetValue(3) != null)
                        description = reader.GetValue(3).ToString();
                    else
                        error += "Поле \"Описание\" пустое\n";

                    if (reader.GetValue(4) == null)
                        error += "Поле \"Продажа\" пустое\n";
                    else if (!double.TryParse(reader.GetValue(4).ToString(), out sellcost))
                        error += "В поле \"Продажа\" допустимы целые и дробные числа\n";

                    if (reader.GetValue(5) == null)
                        error += "Поле \"Колличество\" пустое\n";
                    else if (!int.TryParse(reader.GetValue(5).ToString(), out amount))
                        error += "В поле\"Колличество\" допустимы только целые числа\n";

                    if (!string.IsNullOrEmpty(error))
                    {
                        errors.Add($"Ошибка в категории {reader.Name}. Строка {line}. Ошибки:\n {error}");
                        continue;
                    }

                    ProductViewModel productVM = new(new());

                    if (isEdit)
                    {
                        productVM = listProducts.FirstOrDefault(x => x.Id == id);
                        if (productVM == null)
                        {
                            errors.Add($"Не удалось найти товар с id {id}. Cтрока {line}\n");
                            continue;
                        }
                        if (IsAvailable == 1) productVM.IsAvailable = true;
                        else productVM.IsAvailable = false;
                        if (productVM.CategoryId != category.Id)
                            movedcount++;
                    }
                    else
                    {
                        ProductViewModel product = listProducts.FirstOrDefault(x => x.Brand == brand && x.Title == title && x.Description == description);
                        if (product != null)
                        {
                            product.SellCost = sellcost;
                            product.AmountRes += amount;
                            listProductVM.Add(product);
                            editedcount++;
                            continue;
                        }
                        else addedcount++;
                    }

                    productVM.CategoryId = category.Id;
                    productVM.Brand = brand!;
                    productVM.Title = title!;
                    productVM.Description = description!;
                    productVM.SellCost = sellcost;
                    productVM.AmountRes = amount;

                    listProductVM.Add(productVM);
                }
                if (!isEdit)
                    info += $"Категория \"{reader.Name}\". Добавлено: {addedcount} Отредактировано: {editedcount}\n";
                else if (movedcount > 0)
                    info += $"В категорию \"{reader.Name}\". Перемещено {movedcount} товаров\n";
            } while (reader.NextResult());

            if (errors.Count > 0)
            {
                string AllError = string.Empty;
                foreach (var error in errors)
                    AllError += $"{error}\n";
                AppCommand.ErrorMessage(AllError);
                return null!;
            }
            if (!string.IsNullOrEmpty(info))
                AppCommand.InfoMessage(info);

            return listProductVM;
        }

        private static IExcelDataReader CheckFileStream(FileStream stream, string extension)
        {
            if (extension == ".xlsx")
                return ExcelReaderFactory.CreateOpenXmlReader(stream);
            else if (extension == ".xls")
                return ExcelReaderFactory.CreateBinaryReader(stream);
            else
            {
                AppCommand.WarningMessage("Данные должны быть в формате xlsx или xls");
                return null!;
            }
        }

        public RelayCommand EditProductDataCommand { get; set; }
        private bool CanEditProductDataCommandExecute(object p) => true;
        private async void OnEditProductDataCommandExecuted(object p)
        {
            OpenFileDialog openFileDialog = new() { Filter = "EXCEL Files (*.xlsx)|*.xlsx|EXCEL Files 2003 (*.xls)|*.xls|All files (*.*)|*.*", RestoreDirectory = true };
            bool? resultOpen = openFileDialog.ShowDialog();
            if (resultOpen == true)
            {
                try
                {
                    using FileStream stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                    using IExcelDataReader readerCategory = ExcelReaderFactory.CreateReader(stream);
                    IExcelDataReader edr = CheckFileStream(stream, openFileDialog.FileName[openFileDialog.FileName.LastIndexOf('.')..]);

                    int AddedcategoryCount = 0;
                    int RemovedcategoryCount = 0;
                    do
                    {
                        if (readerCategory.Name == "Evaluation Warning")
                            break;
                        if (readerCategory.Name.Trim().Equals("все категории", StringComparison.CurrentCultureIgnoreCase))
                            break;
                        ProductCategoryViewModel category = CollectionProductCategories.FirstOrDefault(x => x.Category.Equals(readerCategory.Name.Trim(), StringComparison.CurrentCultureIgnoreCase));
                        if (category == null)
                        {
                            await ProductCategoryViewModel.CreateProductCategory(readerCategory.Name.Trim());
                            AddedcategoryCount++;
                        }
                    } while (readerCategory.NextResult());
                    CollectionProductCategories = new(await ProductCategoryViewModel.GetProductCategory());
                    if (AddedcategoryCount > 0 || RemovedcategoryCount > 0)
                        AppCommand.InfoMessage($"Добавлено новых категорий {AddedcategoryCount}");

                    List<ProductViewModel> Products = await Analyzer(stream, true);
                    edr.Close();

                    if (Products == null)
                    {
                        AppCommand.WarningMessage("Исправьте все ошибки и повторите попытку");
                        return;
                    }

                    if (await ProductViewModel.EditProducts(Products))
                        AppCommand.InfoMessage("Товары и категории отредактированы");

                    UpdateCategory();
                    await Update();
                }
                catch (IOException)
                {
                    AppCommand.WarningMessage("Процесс используется другим приложением (Возможно файл открыт).");
                    return;
                }
            }
        }

        public RelayCommand ImportProductDataCommand { get; set; }
        private bool CanImportProductDataCommandExecute(object p) => true;
        private async void OnImportProductDataCommandExecuted(object p)
        {
            if (string.IsNullOrEmpty(StringCostImport))
            {
                AppCommand.WarningMessage("Укажите стоимость прихода.");
                return;
            }
            if (!double.TryParse(StringCostImport, out double CostImport))
            {
                AppCommand.WarningMessage("Укажите стоимость в виде целого или десятичного числа.");
                return;
            }

            OpenFileDialog openFileDialog = new() { Filter = "EXCEL Files (*.xlsx)|*.xlsx|EXCEL Files 2003 (*.xls)|*.xls|All files (*.*)|*.*", RestoreDirectory = true };
            bool? resultOpen = openFileDialog.ShowDialog();
            if (resultOpen == true)
            {
                try
                {
                    using FileStream stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                    using IExcelDataReader readerCategory = ExcelReaderFactory.CreateReader(stream);
                    IExcelDataReader edr = CheckFileStream(stream, openFileDialog.FileName[openFileDialog.FileName.LastIndexOf('.')..]);
                    List<ProductCategoryViewModel> addedcategories = [];

                    int categoryCount = 0;
                    do
                    {
                        if (readerCategory.Name == "Evaluation Warning")
                            break;
                        ProductCategoryViewModel category = CollectionProductCategories.FirstOrDefault(x => x.Category == readerCategory.Name);
                        if (category == null)
                        {
                            addedcategories.Add(await ProductCategoryViewModel.CreateProductCategory(readerCategory.Name));
                            categoryCount++;
                        }
                    } while (readerCategory.NextResult());
                    CollectionProductCategories = new(await ProductCategoryViewModel.GetProductCategory());
                    if (categoryCount > 0)
                        AppCommand.InfoMessage($"Добавлено новых категорий {categoryCount}");

                    List<ProductViewModel> Products = await Analyzer(stream, false);
                    edr.Close();

                    if (Products == null)
                    {
                        AppCommand.WarningMessage("Исправьте все ошибки и повторите попытку. \nНовые категории удалены.");
                        foreach (var item in addedcategories)
                            await ProductCategoryViewModel.RemoveProductCategory(item.Id);
                        UpdateCategory();
                        return;
                    }

                    if (await ProductViewModel.ImportProducts(Products))
                        AppCommand.InfoMessage("Готово");

                    if (!await ComingProductViewModel.NewComing(CostImport))
                        AppCommand.InfoMessage("Не удалось создать отчет о приходе");

                    StringCostImport = string.Empty;
                    UpdateCategory();
                    await Update();
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

        public static void SetRowStyle(Workbook wb, int i, int cell, Aspose.Cells.Style style)
        {
            wb.Worksheets[i].Cells[$"A{cell}"].SetStyle(style);
            wb.Worksheets[i].Cells[$"B{cell}"].SetStyle(style);
            wb.Worksheets[i].Cells[$"C{cell}"].SetStyle(style);
            wb.Worksheets[i].Cells[$"D{cell}"].SetStyle(style);
            wb.Worksheets[i].Cells[$"E{cell}"].SetStyle(style);
            wb.Worksheets[i].Cells[$"F{cell}"].SetStyle(style);
            wb.Worksheets[i].Cells[$"G{cell}"].SetStyle(style);
        }

        public static void AddProductRow(Workbook wb, int i, int cell, ProductViewModel product)
        {
            wb.Worksheets[i].Cells[$"A{cell}"].PutValue(product.Id);
            wb.Worksheets[i].Cells[$"B{cell}"].PutValue(product.Brand);
            wb.Worksheets[i].Cells[$"C{cell}"].PutValue(product.Title);
            wb.Worksheets[i].Cells[$"D{cell}"].PutValue(product.Description);
            wb.Worksheets[i].Cells[$"E{cell}"].PutValue(product.SellCost);
            wb.Worksheets[i].Cells[$"F{cell}"].PutValue(product.Stock.Amount);
            if (product.IsAvailable)
                wb.Worksheets[i].Cells[$"G{cell}"].PutValue(1);
            else
                wb.Worksheets[i].Cells[$"G{cell}"].PutValue(0);
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
            bool? resultOpen = sfd.ShowDialog();
            if (resultOpen == true)
            {
                List<ProductViewModel> products = await ProductViewModel.GetProducts(true);
                products = [.. products.OrderByDescending(x => x.IsAvailable)];
                List<ProductCategoryViewModel> category = await ProductCategoryViewModel.GetProductCategory();
                category.Remove(category[0]);

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

                static void SetRowHeader(Workbook wb, int i, int cell, string title, int width)
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
                        SetRowHeader(wb, i, 1, "Производитель", 17);
                        SetRowHeader(wb, i, 2, "Название", 27);
                        SetRowHeader(wb, i, 3, "Описание", 17);
                        SetRowHeader(wb, i, 4, "Продажа", 10);
                        SetRowHeader(wb, i, 5, "Колличество", 14);
                        SetRowHeader(wb, i, 6, "В продаже / Снят с продаж", 26);

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
        }

        public RelayCommand FileForImportCommand { get; set; }
        private bool CanFileForImportCommandExecute(object p) => true;
        private void OnFileForImportCommandExecuted(object p)
        {
            SaveFileDialog sfd = new()
            {
                Filter = "Excel Standart(*.xlsx)|*.xlsx|Excel 97-2003 (*.xls)|*.xls|All files(*.*)|*.*",
                FileName = "ExampleImport"
            };
            bool? resultOpen = sfd.ShowDialog();
            if (resultOpen == true)
            {
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

                static void SetRowHeader(Workbook wb, int i, int cell, string title, int width)
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
                    SetRowHeader(wb, 0, 1, "Производитель", 17);
                    SetRowHeader(wb, 0, 2, "Название", 27);
                    SetRowHeader(wb, 0, 3, "Описание", 17);
                    SetRowHeader(wb, 0, 4, "Продажа", 10);
                    SetRowHeader(wb, 0, 5, "Колличество", 14);
                    wb.Worksheets[0].Cells.SetRowHeight(0, 30);
                    SetRowStyle(wb, 0, 1, styleHeader);

                    wb.Worksheets[0].Cells[$"A2"].PutValue(1);
                    wb.Worksheets[0].Cells[$"B2"].PutValue("Хаски");
                    wb.Worksheets[0].Cells[$"C2"].PutValue("Киви яблоко");
                    wb.Worksheets[0].Cells[$"D2"].PutValue("20 mg strong");
                    wb.Worksheets[0].Cells[$"E2"].PutValue("450");
                    wb.Worksheets[0].Cells[$"F2"].PutValue(10);

                    wb.Save(sfd.FileName);

                    AppCommand.InfoMessage("Шаблон готов");
                }
            }
        }

        #endregion

        private async void UpdateCategory()
        {
            CollectionProductCategories = new(await ProductCategoryViewModel.GetProductCategory());
            SelectedProductCategory = CollectionProductCategories[0];
        }

        private async Task Update()
        {
            VisibilityProduct = Visibility.Collapsed;
            VisibilityLoadProduct = Visibility.Visible;
            try
            {
                await Task.Run(async () =>
                {
                    List<ComingProductViewModel> listcoming = await ComingProduct.GetComing();
                    CollectionComing = new(listcoming.TakeLast(20).OrderByDescending(x => x.CommingDatetime).ToList());
                    List<ProductViewModel> products = await ProductViewModel.GetProducts(IsShowAllProduct);
                    if (SelectedProductCategory != null && SelectedProductCategory.Category != "Все категории")
                        products = products.Where(x => x.CategoryId == SelectedProductCategory.Id).ToList();
                    ProductCount = products.Count;
                    if (ShowProductCount > ProductCount)
                        ShowProductCount = ProductCount;
                    switch (Sort)
                    {
                        case 1:
                            if (SwapDown == Visibility.Visible)
                                products = [.. products.OrderByDescending(x => x.SellCost)];
                            else
                                products = [.. products.OrderBy(x => x.SellCost)];
                            break;
                        case 2:
                            if (SwapDown == Visibility.Visible)
                                products = [.. products.OrderByDescending(x => x.Stock.Amount)];
                            else
                                products = [.. products.OrderBy(x => x.Stock.Amount)];
                            break;
                        case 3:
                            if (SwapDown == Visibility.Visible)
                                products = [.. products.OrderBy(x => x.IsAvailable)];
                            else
                                products = [.. products.OrderByDescending(x => x.IsAvailable)];
                            break;
                        default:
                            if (SwapDown == Visibility.Visible)
                                products = [.. products.OrderByDescending(x => x.CountSell)];
                            else
                                products = [.. products.OrderBy(x => x.CountSell)];
                            break;
                    }
                    string searchs = SearchStr.ToLower().Trim();
                    if (!string.IsNullOrEmpty(searchs))
                        products = products.Where(x => x.Brand.Contains(searchs, StringComparison.CurrentCultureIgnoreCase) ||
                                                       x.Title.Contains(searchs, StringComparison.CurrentCultureIgnoreCase) ||
                                                       x.SellCost.ToString().Contains(searchs, StringComparison.CurrentCultureIgnoreCase) ||
                                                       x.Description.Contains(searchs, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    CollectionProducts = new(products.Take(ShowProductCount));
                    ShowedProductCount = CollectionProducts.Count;
                });
            }
            catch (Exception)
            {
                AppCommand.ErrorMessage("Возник конфликт потоков. Повторите операцию");
                return;
            }
            VisibilityLoadProduct = Visibility.Collapsed;
            VisibilityProduct = Visibility.Visible;
        }

        #endregion

        public StockViewModel()
        {
            UpdateCategory();
            SortSwapCommand = new RelayCommand(OnSortSwapCommandExecuted, CanSortSwapCommandExecute);
            ReturnProductCommand = new RelayCommand(OnReturnProductCommandExecuted, CanReturnProductCommandExecute);
            SelectCategoryCommand = new RelayCommand(OnSelectCategoryCommandExecuted, CanSelectCategoryCommandExecute);
            SearchDataCommand = new RelayCommand(OnSearchDataCommandExecuted, CanSearchDataCommandExecute);
            RemoveCategoryCommand = new RelayCommand(OnRemoveCategoryCommandExecuted, CanRemoveCategoryCommandExecute);
            RemoveProductCommand = new RelayCommand(OnRemoveProductCommandExecuted, CanRemoveProductCommandExecute);
            GetAllProductCommand = new RelayCommand(OnGetAllProductCommandExecuted, CanGetAllProductCommandExecute);
            ImportProductDataCommand = new RelayCommand(OnImportProductDataCommandExecuted, CanImportProductDataCommandExecute);
            ExportProductDataCommand = new RelayCommand(OnExportProductDataCommandExecuted, CanExportProductDataCommandExecute);
            EditProductDataCommand = new RelayCommand(OnEditProductDataCommandExecuted, CanEditProductDataCommandExecute);
            FileForImportCommand = new RelayCommand(OnFileForImportCommandExecuted, CanFileForImportCommandExecute);
            canupdate = true;
        }
    }
}
