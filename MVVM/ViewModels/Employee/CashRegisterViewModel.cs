using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.Models;
using Cashbox.MVVM.ViewModels.Data;
using System.Collections.ObjectModel;
using System.Windows;

namespace Cashbox.MVVM.ViewModels.Employee
{
    public class CashRegisterViewModel : ViewModelBase
    {
        #region Props
        public static UserViewModel? User { get => Models.User.CurrentUser; }
        bool canupdate = false;
        #region Visibility

        private Visibility _orderPanelVisibility = Visibility.Collapsed;
        public Visibility OrderPanelVisibility
        {
            get => _orderPanelVisibility;
            set => Set(ref _orderPanelVisibility, value);
        }

        private Visibility _menuPanelVisibility = Visibility.Visible;
        public Visibility MenuPanelVisibility
        {
            get => _menuPanelVisibility;
            set => Set(ref _menuPanelVisibility, value);
        }

        private Visibility _returnPanelVisibility = Visibility.Collapsed;
        public Visibility ReturnPanelVisibility
        {
            get => _returnPanelVisibility;
            set => Set(ref _returnPanelVisibility, value);
        }
        private Visibility _crackPanelVisibility = Visibility.Collapsed;
        public Visibility CrackPanelVisibility
        {
            get => _crackPanelVisibility;
            set => Set(ref _crackPanelVisibility, value);
        }

        private Visibility _drawPanelVisibility = Visibility.Collapsed;
        public Visibility DrawPanelVisibility
        {
            get => _drawPanelVisibility;
            set => Set(ref _drawPanelVisibility, value);
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

        private Visibility _totalVisibility = Visibility.Collapsed;
        public Visibility TotalVisibility
        {
            get => _totalVisibility;
            set => Set(ref _totalVisibility, value);
        }

        #endregion

        private ObservableCollection<ProductViewModel?> _selectedProductRef = [];
        public ObservableCollection<ProductViewModel?> SelectedProductRef
        {
            get => _selectedProductRef;
            set => Set(ref _selectedProductRef, value);
        }

        private ObservableCollection<ProductViewModel> _collectionProducts = [];
        public ObservableCollection<ProductViewModel> CollectionProducts
        {
            get
            {
                if (_collectionProducts.Count > 0)
                    VisibilityProduct = Visibility.Visible;
                else
                    VisibilityProduct = Visibility.Collapsed;
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
                OnPropertyChanged();
                if (canupdate)
                    Update().GetAwaiter();
            }
        }

        private string _searchStr = string.Empty;
        public string SearchStr
        {
            get => _searchStr;
            set => Set(ref _searchStr, value);
        }

        private ObservableCollection<OrderProductViewModel> _orderProductsBasket = [];
        public ObservableCollection<OrderProductViewModel> OrderProductsBasket
        {
            get
            {
                TotalCost = _orderProductsBasket.Sum(x => x.SellCost * x.Amount);
                return _orderProductsBasket;
            }
            set => Set(ref _orderProductsBasket, value);
        }

        private double _otherOrderCost;
        public double OtherOrderCost
        {
            get => _otherOrderCost;
            set => Set(ref _otherOrderCost, value);
        }

        private int _discountOrderCost = 0;
        public int DiscountOrderCost
        {
            get => _discountOrderCost;
            set
            {
                if (value < 0)
                    value = 0;
                if (value > 100)
                    value = 100;
                _discountOrderCost = value;
                OnPropertyChanged();
            }
        }

        private Order? _selectedOrder;
        public Order? SelectedOrder
        {
            get => _selectedOrder;
            set => Set(ref _selectedOrder, value);
        }

        private double _totalCost;
        public double TotalCost
        {
            get => _totalCost;
            set
            {
                _totalCost = value;
                ReTotalCost = value - (value / 100 * DiscountOrderCost);
                if (value != ReTotalCost)
                    TotalVisibility = Visibility.Visible;
                else
                    TotalVisibility = Visibility.Collapsed;
                OnPropertyChanged();
            }
        }

        private double _reTotalCost;
        public double ReTotalCost
        {
            get => _reTotalCost;
            set => Set(ref _reTotalCost, value);
        }

        private Refund? _currentRefund;
        public Refund? CurrentRefund
        {
            get => _currentRefund;
            set => Set(ref _currentRefund, value);
        }

        private string _refundReason = string.Empty;
        public string RefundReason
        {
            get => _refundReason;
            set => Set(ref _refundReason, value);
        }

        private DateTime _refundBuyDate = DateTime.Today;
        public DateTime RefundBuyDate
        {
            get => _refundBuyDate;
            set
            {
                if (value > DateTime.Today)
                {
                    AppCommand.WarningMessage("Неверная дата");
                    value = DateTime.Today;
                }
                _refundBuyDate = value;
                OnPropertyChanged();
            }
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
                if (value > ProductCount)
                    value = ProductCount;
                _showProductCount = value;
                OnPropertyChanged();
                if (canupdate)
                    Update().GetAwaiter();
            }
        }

        #endregion

        #region Command

        public RelayCommand SearchDataCommand { get; set; }
        private bool CanSearchDataCommandExecute(object p) => true;
        private async void OnSearchDataCommandExecuted(object p) => await Update();

        public RelayCommand IncreaseAmountProductBasketCommand { get; set; }
        private bool CanIncreaseAmountProductBasketCommandExecute(object p)
        {
            if (p == null)
                return false;
            OrderProductViewModel AddedProduct = OrderProductsBasket.FirstOrDefault(x => x.ProductId == (int)p);
            if (AddedProduct == null)
                return false;
            if (AddedProduct.Amount >= AddedProduct.ProductVM.Stock.Amount)
            {
                AddedProduct.Amount = AddedProduct.ProductVM.Stock.Amount;
                return false;
            }
            return true;
        }
        private void OnIncreaseAmountProductBasketCommandExecuted(object p)
        {
            OrderProductViewModel AddedProduct = OrderProductsBasket.FirstOrDefault(x => x.ProductId == (int)p);
            if (AddedProduct != null)
                AddedProduct.Amount += 1;
        }

        public RelayCommand DecreaseAmountProductBasketCommand { get; set; }
        private bool CanDecreaseAmountProductBasketCommandExecute(object p)
        {
            if (p == null)
                return false;
            OrderProductViewModel AddedProduct = OrderProductsBasket.FirstOrDefault(x => x.ProductId == (int)p);
            if (AddedProduct == null)
                return false;
            if (AddedProduct.Amount == 1)
                return false;
            return true;
        }
        private void OnDecreaseAmountProductBasketCommandExecuted(object p)
        {
            OrderProductViewModel AddedProduct = OrderProductsBasket.FirstOrDefault(x => x.ProductId == (int)p);
            if (AddedProduct != null)
                AddedProduct.Amount -= 1;
        }

        public RelayCommand RemoveProductInBasketCommand { get; set; }
        private bool CanRemoveProductInBasketCommandExecute(object p) => true;
        private void OnRemoveProductInBasketCommandExecuted(object p)
        {
            OrderProductViewModel AddedProduct = OrderProductsBasket.FirstOrDefault(x => x.ProductId == (int)p);
            if (AddedProduct == null)
                return;
            if (AppCommand.QuestionMessage("Убрать товар из заказа?") == MessageBoxResult.Yes)
                OrderProductsBasket.Remove(AddedProduct);
        }

        public RelayCommand AddProductInBasketCommand { get; set; }
        private bool CanAddProductInBasketCommandExecute(object p)
        {
            if (p == null)
                return false;
            return true;
        }
        private void OnAddProductInBasketCommandExecuted(object p)
        {
            if (ReturnPanelVisibility == Visibility.Visible || CrackPanelVisibility == Visibility.Visible || DrawPanelVisibility == Visibility.Visible)
            {
                SelectedProductRef = [];
                SelectedProductRef.Add(CollectionProducts.FirstOrDefault(x => x.Id == (int)p));
                return;
            }

            if (SelectedOrder == null) return;

            OrderProductViewModel AddedProduct = OrderProductsBasket.FirstOrDefault(x => x.ProductId == (int)p);
            ProductViewModel SelectedProduct = CollectionProducts.FirstOrDefault(x => x.Id == (int)p);

            if (AddedProduct != null)
            {
                OrderProductsBasket.FirstOrDefault(x => x.OrderId == OrderViewModel.OrderComposition.Id && x.ProductId == SelectedProduct.Id).Amount += 1;
                return;
            }
            if (SelectedProduct != null)
            {
                OrderProductsBasket.Add(
                    new(
                        new()
                        {
                            ProductId = SelectedProduct.Id,
                            SellCost = SelectedProduct.SellCost,
                            OrderId = OrderViewModel.OrderComposition.Id,
                            Amount = 1,
                        }
                    )
                    { ProductVM = SelectedProduct }
                );
            }
        }

        public RelayCommand RemoveCurrentOrderCommand { get; set; }
        private bool CanRemoveCurrentOrderCommandExecute(object p)
        {
            if (OrderViewModel.OrderComposition == null)
                return false;
            return true;
        }
        private async void OnRemoveCurrentOrderCommandExecuted(object p)
        {
            await OrderViewModel.RemoveCurrentOrder();
            SelectedOrder = null;
            OrderProductsBasket.Clear();
            OrderPanelVisibility = Visibility.Collapsed;
            MenuPanelVisibility = Visibility.Visible;
        }

        public RelayCommand ChangeSellCostCommand { get; set; }
        private bool CanChangeSellCostCommandExecute(object p) => true;
        private void OnChangeSellCostCommandExecuted(object p)
        {
            OrderProductViewModel item = OrderProductsBasket.FirstOrDefault(x => x.ProductVM.Id == (int)p);
            if (string.IsNullOrEmpty(item.ProductVM.ReSellCost))
            {
                AppCommand.WarningMessage("Поле не должно быть пустым");
                return;
            }
            item.SellCost = double.Parse(item.ProductVM.ReSellCost);
            AppCommand.InfoMessage("Цена выставлена");
        }

        public RelayCommand OpenBasketOrderPanelCommand { get; set; }
        private bool CanOpenBasketOrderPanelCommandExecute(object p) => true;
        private async void OnOpenBasketOrderPanelCommandExecuted(object p)
        {
            if (OrderViewModel.OrderComposition == null)
                await OrderViewModel.CreateOrder();
            SelectedOrder = Order.OrderComposition;
            OrderPanelVisibility = Visibility.Visible;
            MenuPanelVisibility = Visibility.Collapsed;
        }

        public RelayCommand OpenReturnOrderPanelCommand { get; set; }
        private bool CanOpenReturnOrderPanelCommandExecute(object p) => true;
        private async void OnOpenReturnOrderPanelCommandExecuted(object p)
        {
            if (RefundViewModel.CurrentRefund == null)
                await RefundViewModel.CreateRefund();
            RefundReason = string.Empty;
            CurrentRefund = RefundViewModel.CurrentRefund;
            ReturnPanelVisibility = Visibility.Visible;
            MenuPanelVisibility = Visibility.Collapsed;
        }

        public RelayCommand OpenDrawPanelCommand { get; set; }
        private bool CanOpenDrawPanelCommandExecute(object p) => true;
        private async void OnOpenDrawPanelCommandExecuted(object p)
        {
            if (RefundViewModel.CurrentRefund == null)
                await RefundViewModel.CreateRefund();
            CurrentRefund = RefundViewModel.CurrentRefund;
            DrawPanelVisibility = Visibility.Visible;
            MenuPanelVisibility = Visibility.Collapsed;
        }

        public RelayCommand OpenCrackOrderPanelCommand { get; set; }
        private bool CanOpenCrackOrderPanelCommandExecute(object p) => true;
        private async void OnOpenCrackOrderPanelCommandExecuted(object p)
        {
            if (RefundViewModel.CurrentRefund == null)
                await RefundViewModel.CreateRefund();
            RefundReason = string.Empty;
            CurrentRefund = RefundViewModel.CurrentRefund;
            CrackPanelVisibility = Visibility.Visible;
            MenuPanelVisibility = Visibility.Collapsed;
        }

        public RelayCommand OpenMenuPanelCommand { get; set; }
        private bool CanOpenMenuPanelCommandExecute(object p) => true;
        private void OnOpenMenuPanelCommandExecuted(object p)
        {
            OrderPanelVisibility = Visibility.Collapsed;
            ReturnPanelVisibility = Visibility.Collapsed;
            CrackPanelVisibility = Visibility.Collapsed;
            DrawPanelVisibility = Visibility.Collapsed;
            MenuPanelVisibility = Visibility.Visible;
            SelectedProductRef = [];
            OrderProductsBasket = [];
        }

        public RelayCommand RemoveCurrentRefundCommand { get; set; }
        private bool CanRemoveCurrentRefundCommandExecute(object p) => true;
        private async void OnRemoveCurrentRefundCommandExecuted(object p)
        {
            if (CurrentRefund == null) return;
            await RefundViewModel.RemoveCurrentRefund();
            CurrentRefund = null;
            OnOpenMenuPanelCommandExecuted(p);
        }

        public RelayCommand SetDiscountCommand { get; set; }
        private bool CanSetDiscountCommandExecute(object p) => true;
        private void OnSetDiscountCommandExecuted(object p)
        {
            ReTotalCost = TotalCost - (TotalCost / 100 * DiscountOrderCost);
        }


        public RelayCommand SellOrderCommand { get; set; }
        private bool CanSellOrderCommandExecute(object p)
        {
            if (OrderProductsBasket.Count == 0)
                return false;
            return true;
        }
        private async void OnSellOrderCommandExecuted(object p)
        {
            int method = int.Parse(p.ToString()!);
            switch (method)
            {
                case 1:
                    if (AppCommand.QuestionMessage("Оплата картой?") != MessageBoxResult.Yes)
                        return;
                    break;
                case 2:
                    if (AppCommand.QuestionMessage("Оплата наличными?") != MessageBoxResult.Yes)
                        return;
                    break;
                case 3:
                    if (AppCommand.QuestionMessage("Оплата переводом?") != MessageBoxResult.Yes)
                        return;
                    break;
            }
            if (!await OrderViewModel.SellOrder(method, TotalCost, DiscountOrderCost, [.. OrderProductsBasket]))
                return;
            await Update();
            OnOpenMenuPanelCommandExecuted(p);
        }

        public RelayCommand ReturnProductCommand { get; set; }
        private bool CanReturnProductCommandExecute(object p)
        {
            if (SelectedProductRef.FirstOrDefault() == null)
                return false;
            return true;
        }
        private async void OnReturnProductCommandExecuted(object p)
        {
            if (string.IsNullOrEmpty(RefundReason))
            {
                AppCommand.WarningMessage("Укажите причину возврата");
                return;
            }
            if (await RefundViewModel.CreateRefundReason(RefundReason, DateOnly.FromDateTime(RefundBuyDate), SelectedProductRef.FirstOrDefault().Id))
                AppCommand.InfoMessage("Возврат продукта выполнен");
            CurrentRefund = null;
            OnOpenMenuPanelCommandExecuted(p);
        }


        public RelayCommand ReturnCrackProductCommand { get; set; }
        private bool CanReturnCrackProductCommandExecute(object p)
        {
            if (SelectedProductRef.FirstOrDefault() == null)
                return false;
            return true;
        }
        private async void OnReturnCrackProductCommandExecuted(object p)
        {
            if (string.IsNullOrEmpty(RefundReason))
            {
                AppCommand.WarningMessage("Укажите причину брака");
                return;
            }
            await PStockViewModel.UpdateProductStock(SelectedProductRef.FirstOrDefault().Id, SelectedProductRef.FirstOrDefault().Stock.Amount - 1);
            if (await RefundViewModel.CreateRefundDefect(SelectedProductRef.FirstOrDefault().Id, RefundReason))
                AppCommand.InfoMessage("Брак отмечен");
            CurrentRefund = null;
            OnOpenMenuPanelCommandExecuted(p);
        }

        public RelayCommand DrawProductCommand { get; set; }
        private bool CanDrawProductCommandExecute(object p)
        {
            if (SelectedProductRef.FirstOrDefault() == null)
                return false;
            return true;
        }
        private async void OnDrawProductCommandExecuted(object p)
        {
            if (await RefundViewModel.CreateDraw(SelectedProductRef.FirstOrDefault().Id, DateOnly.FromDateTime(RefundBuyDate)))
                AppCommand.InfoMessage("Продукт разыгран");
            CurrentRefund = null;
            OnOpenMenuPanelCommandExecuted(p);
        }

        public RelayCommand ClearSelectedProductCommand { get; set; }
        private bool CanClearSelectedProductCommandExecute(object p) => true;
        private void OnClearSelectedProductCommandExecuted(object p)
        {
            if (SelectedProductRef == null)
                return;
            if (AppCommand.QuestionMessage("Убрать товар?") == MessageBoxResult.Yes)
            {
                SelectedProductRef = [];
                RefundBuyDate = DateTime.Today;

            }
        }

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

        public override async void OnLoad()
        {
            CollectionProductCategories = new(await ProductCategoryViewModel.GetProductCategory());
            SelectedProductCategory = CollectionProductCategories[0];
            await Update();
        }

        private async Task Update()
        {
            VisibilityProduct = Visibility.Collapsed;
            VisibilityLoadProduct = Visibility.Visible;
            await Task.Run(async () =>
            {
                try
                {
                    List<ProductViewModel> products = await ProductViewModel.GetProducts();
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
                    VisibilityLoadProduct = Visibility.Collapsed;
                    VisibilityProduct = Visibility.Visible;
                }
                catch (Exception)
                {
                    AppCommand.ErrorMessage("Возник конфликт потоков. Повторите операцию");
                    return;
                }
            });
        }

        #endregion
        public CashRegisterViewModel()
        {
            SortSwapCommand = new RelayCommand(OnSortSwapCommandExecuted, CanSortSwapCommandExecute);
            SearchDataCommand = new RelayCommand(OnSearchDataCommandExecuted, CanSearchDataCommandExecute);

            AddProductInBasketCommand = new RelayCommand(OnAddProductInBasketCommandExecuted, CanAddProductInBasketCommandExecute);
            IncreaseAmountProductBasketCommand = new RelayCommand(OnIncreaseAmountProductBasketCommandExecuted, CanIncreaseAmountProductBasketCommandExecute);
            DecreaseAmountProductBasketCommand = new RelayCommand(OnDecreaseAmountProductBasketCommandExecuted, CanDecreaseAmountProductBasketCommandExecute);
            RemoveProductInBasketCommand = new RelayCommand(OnRemoveProductInBasketCommandExecuted, CanRemoveProductInBasketCommandExecute);
            RemoveCurrentOrderCommand = new RelayCommand(OnRemoveCurrentOrderCommandExecuted, CanRemoveCurrentOrderCommandExecute);
            RemoveCurrentRefundCommand = new RelayCommand(OnRemoveCurrentRefundCommandExecuted, CanRemoveCurrentRefundCommandExecute);
            SellOrderCommand = new RelayCommand(OnSellOrderCommandExecuted, CanSellOrderCommandExecute);
            ReturnProductCommand = new RelayCommand(OnReturnProductCommandExecuted, CanReturnProductCommandExecute);
            ReturnCrackProductCommand = new RelayCommand(OnReturnCrackProductCommandExecuted, CanReturnCrackProductCommandExecute);
            DrawProductCommand = new RelayCommand(OnDrawProductCommandExecuted, CanDrawProductCommandExecute);
            ClearSelectedProductCommand = new RelayCommand(OnClearSelectedProductCommandExecuted, CanClearSelectedProductCommandExecute);
            SetDiscountCommand = new RelayCommand(OnSetDiscountCommandExecuted, CanSetDiscountCommandExecute);

            OpenBasketOrderPanelCommand = new RelayCommand(OnOpenBasketOrderPanelCommandExecuted, CanOpenBasketOrderPanelCommandExecute);
            OpenReturnOrderPanelCommand = new RelayCommand(OnOpenReturnOrderPanelCommandExecuted, CanOpenReturnOrderPanelCommandExecute);
            OpenCrackOrderPanelCommand = new RelayCommand(OnOpenCrackOrderPanelCommandExecuted, CanOpenCrackOrderPanelCommandExecute);
            OpenMenuPanelCommand = new RelayCommand(OnOpenMenuPanelCommandExecuted, CanOpenMenuPanelCommandExecute);
            OpenDrawPanelCommand = new RelayCommand(OnOpenDrawPanelCommandExecuted, CanOpenDrawPanelCommandExecute);
            ChangeSellCostCommand = new RelayCommand(OnChangeSellCostCommandExecuted, CanChangeSellCostCommandExecute);
            canupdate = true;
        }
    }
}
