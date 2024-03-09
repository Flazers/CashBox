﻿using Cashbox.Core;
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

        #region Visibility
        private Visibility _sellPanelVisibility = Visibility.Collapsed;
        public Visibility SellPanelVisibility
        {
            get => _sellPanelVisibility;
            set => Set(ref _sellPanelVisibility, value);
        }
        private Visibility _orderPanelVisibility = Visibility.Collapsed;
        public Visibility OrderPanelVisibility
        {
            get => _orderPanelVisibility;
            set => Set(ref _orderPanelVisibility, value);
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
        private Visibility _menuPanelVisibility = Visibility.Visible;
        public Visibility MenuPanelVisibility
        {
            get => _menuPanelVisibility;
            set => Set(ref _menuPanelVisibility, value);
        }

        #endregion

        private static ObservableCollection<ProductViewModel> CollectionProductsBase => new(ProductViewModel.GetProducts().Result);

        private ObservableCollection<ProductViewModel> _collectionProducts = CollectionProductsBase;
        public ObservableCollection<ProductViewModel> CollectionProducts => _collectionProducts;

        private string? _searchCollectionProduct;
        public string? SearchCollectionProduct
        {
            get => _searchCollectionProduct;
            set
            {
                //OnLoad();
                //_searchCollectionProduct = value;
                //Task.Delay(200);
                //if (string.IsNullOrWhiteSpace(_searchCollectionProduct) || value == null)
                //    CollectionProducts = CollectionProductsBase;
                //else
                //{
                //    CollectionProducts =
                //        new(CollectionProductsBase
                //            .Where(x =>
                //                x.Title.Contains(value) ||
                //                x.Brand.Contains(value) ||
                //                x.Description.Contains(value)
                //            )
                //        );
                //}
                OnPropertyChanged();
            }
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
            set => Set(ref _discountOrderCost, value);
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
            set => Set(ref _totalCost, value);
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

        private DateOnly? _refundBuyDate;
        public DateOnly? RefundBuyDate
        {
            get => _refundBuyDate;
            set => Set(ref _refundBuyDate, value);
        }

        private string _shiftOpenTime = string.Empty;
        public string ShiftOpenTime
        {
            get
            {
                DailyReport CurrentShift = DailyReportViewModel.CurrentShift;
                if (CurrentShift == null)
                    return _shiftOpenTime = "нет";
                return _shiftOpenTime = $"{CurrentShift.Data} в {CurrentShift.OpenTime}";
            }
        }


        #endregion

        #region Command

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
            MessageBoxResult result = MessageBox.Show("Убрать товар из заказа?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
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
                            PurchaseСost = SelectedProduct.PurchaseСost,
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

        public RelayCommand OpenSellOrderPanelCommand { get; set; }
        private bool CanOpenSellOrderPanelCommandExecute(object p) => true;
        private void OnOpenSellOrderPanelCommandExecuted(object p)
        {
            SellPanelVisibility = Visibility.Visible;
            OrderPanelVisibility = Visibility.Collapsed;
            ReturnPanelVisibility = Visibility.Collapsed;
            CrackPanelVisibility = Visibility.Collapsed;
            MenuPanelVisibility = Visibility.Collapsed;
        }

        public RelayCommand OpenBasketOrderPanelCommand { get; set; }
        private bool CanOpenBasketOrderPanelCommandExecute(object p) => true;
        private async void OnOpenBasketOrderPanelCommandExecuted(object p)
        {
            if (OrderViewModel.OrderComposition == null)
            {
                await OrderViewModel.CreateOrder();
                SelectedOrder = Order.OrderComposition;
            }
            SellPanelVisibility = Visibility.Collapsed;
            OrderPanelVisibility = Visibility.Visible;
            ReturnPanelVisibility = Visibility.Collapsed;
            CrackPanelVisibility = Visibility.Collapsed;
            MenuPanelVisibility = Visibility.Collapsed;
        }

        public RelayCommand OpenReturnOrderPanelCommand { get; set; }
        private bool CanOpenReturnOrderPanelCommandExecute(object p) => true;
        private async void OnOpenReturnOrderPanelCommandExecuted(object p)
        {
            if (CurrentRefund == null)
            {
                await RefundViewModel.CreateRefund();
                CurrentRefund = RefundViewModel.CurrentRefund;
            }
            SellPanelVisibility = Visibility.Collapsed;
            OrderPanelVisibility = Visibility.Collapsed;
            ReturnPanelVisibility = Visibility.Visible;
            CrackPanelVisibility = Visibility.Collapsed;
            MenuPanelVisibility = Visibility.Collapsed;
        }

        public RelayCommand OpenCrackOrderPanelCommand { get; set; }
        private bool CanOpenCrackOrderPanelCommandExecute(object p) => true;
        private void OnOpenCrackOrderPanelCommandExecuted(object p)
        {
            SellPanelVisibility = Visibility.Collapsed;
            OrderPanelVisibility = Visibility.Collapsed;
            ReturnPanelVisibility = Visibility.Collapsed;
            CrackPanelVisibility = Visibility.Visible;
            MenuPanelVisibility = Visibility.Collapsed;
        }

        public RelayCommand OpenMenuPanelCommand { get; set; }
        private bool CanOpenMenuPanelCommandExecute(object p) => true;
        private void OnOpenMenuPanelCommandExecuted(object p)
        {
            SellPanelVisibility = Visibility.Collapsed;
            OrderPanelVisibility = Visibility.Collapsed;
            ReturnPanelVisibility = Visibility.Collapsed;
            CrackPanelVisibility = Visibility.Collapsed;
            MenuPanelVisibility = Visibility.Visible;
        }

        public RelayCommand RemoveCurrentCrackReturnCommand { get; set; }
        private bool CanRemoveCurrentCrackReturnCommandExecute(object p) => true;
        private async void OnRemoveCurrentCrackReturnCommandExecuted(object p)
        {
            if (CurrentRefund == null) return;
            await RefundViewModel.RemoveCurrentRefund();
            CurrentRefund = null;
        }

        public RelayCommand SellOrderCommand { get; set; }
        private bool CanSellOrderCommandExecute(object p) => true;
        private async void OnSellOrderCommandExecuted(object p)
        {
            MessageBoxResult result;
            switch (int.Parse(p.ToString()!))
            {
                case 1:
                    result = MessageBox.Show("Оплата картой", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                        if (await OrderViewModel.SellOrder(1, TotalCost, 0, [.. OrderProductsBasket]))
                            MessageBox.Show("Успех");
                    break;
                case 2:
                    result = MessageBox.Show("Оплата наличными", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                        if (await OrderViewModel.SellOrder(1, TotalCost, 0, [.. OrderProductsBasket]))
                            MessageBox.Show("Успех");
                    break;
                case 3:
                    result = MessageBox.Show("Оплата переводом", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                        if (await OrderViewModel.SellOrder(1, TotalCost, 0, [.. OrderProductsBasket]))
                            MessageBox.Show("Успех");
                    break;
            }
            OnOpenMenuPanelCommandExecuted(null!);
        }

        #endregion
        public CashRegisterViewModel()
        {
            AddProductInBasketCommand = new RelayCommand(OnAddProductInBasketCommandExecuted, CanAddProductInBasketCommandExecute);
            IncreaseAmountProductBasketCommand = new RelayCommand(OnIncreaseAmountProductBasketCommandExecuted, CanIncreaseAmountProductBasketCommandExecute);
            DecreaseAmountProductBasketCommand = new RelayCommand(OnDecreaseAmountProductBasketCommandExecuted, CanDecreaseAmountProductBasketCommandExecute);
            RemoveProductInBasketCommand = new RelayCommand(OnRemoveProductInBasketCommandExecuted, CanRemoveProductInBasketCommandExecute);
            RemoveCurrentOrderCommand = new RelayCommand(OnRemoveCurrentOrderCommandExecuted, CanRemoveCurrentOrderCommandExecute);
            RemoveCurrentCrackReturnCommand = new RelayCommand(OnRemoveCurrentCrackReturnCommandExecuted, CanRemoveCurrentCrackReturnCommandExecute);
            SellOrderCommand = new RelayCommand(OnSellOrderCommandExecuted, CanSellOrderCommandExecute);

            OpenSellOrderPanelCommand = new RelayCommand(OnOpenSellOrderPanelCommandExecuted, CanOpenSellOrderPanelCommandExecute);
            OpenBasketOrderPanelCommand = new RelayCommand(OnOpenBasketOrderPanelCommandExecuted, CanOpenBasketOrderPanelCommandExecute);
            OpenReturnOrderPanelCommand = new RelayCommand(OnOpenReturnOrderPanelCommandExecuted, CanOpenReturnOrderPanelCommandExecute);
            OpenCrackOrderPanelCommand = new RelayCommand(OnOpenCrackOrderPanelCommandExecuted, CanOpenCrackOrderPanelCommandExecute);
            OpenMenuPanelCommand = new RelayCommand(OnOpenMenuPanelCommandExecuted, CanOpenMenuPanelCommandExecute);
        }
    }
}
