using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.Models;
using Cashbox.MVVM.ViewModels.Data;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cashbox.MVVM.ViewModels.Employee
{
    public class CashRegisterViewModel : ViewModelBase
    {
        #region Props

        #region Visibility
        private Visibility _sellPanelVisibility = Visibility.Collapsed;
        public Visibility SellPanelVisibility
        {
            get => _sellPanelVisibility;
            set => Set(ref _sellPanelVisibility, value);
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
        public static UserViewModel? User { get => Models.User.CurrentUser; }

        private ObservableCollection<ProductViewModel> _collectionProducts = new(ProductViewModel.GetProducts().Result);
        public ObservableCollection<ProductViewModel> CollectionProducts
        {
            get =>_collectionProducts;
            set => Set(ref _collectionProducts, value);
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
            if (CollectionProducts.Count == 0) 
                return false;
            if (p == null)
                return false;
            return true;
        }
        private async void OnAddProductInBasketCommandExecuted(object p)
        {
            if (OrderViewModel.OrderComposition == null)
                await OrderViewModel.CreateOrder();
            SelectedOrder = Order.OrderComposition;

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
                    { ProductVM = SelectedProduct}
                );
            }
        }

        public RelayCommand RemoveCurrentOrderCommand {  get; set; }
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
            SellPanelVisibility = Visibility.Collapsed;
            MenuPanelVisibility = Visibility.Visible;
        }

        public RelayCommand OpenSellOrderPanelCommand { get; set; }
        private bool CanOpenSellOrderPanelCommandExecute(object p) => true;
        private async void OnOpenSellOrderPanelCommandExecuted(object p)
        {
            await OrderViewModel.CreateOrder();
            SelectedOrder = Order.OrderComposition;
            SellPanelVisibility = Visibility.Visible;
            ReturnPanelVisibility = Visibility.Collapsed;
            CrackPanelVisibility = Visibility.Collapsed;
            MenuPanelVisibility = Visibility.Collapsed;
        }

        public RelayCommand OpenReturnOrderPanelCommand { get; set; }
        private bool CanOpenReturnOrderPanelCommandExecute(object p) => true;
        private void OnOpenReturnOrderPanelCommandExecuted(object p)
        {
            SellPanelVisibility = Visibility.Collapsed;
            ReturnPanelVisibility = Visibility.Visible;
            CrackPanelVisibility = Visibility.Collapsed;
            MenuPanelVisibility = Visibility.Collapsed;
        }

        public RelayCommand OpenCrackOrderPanelCommand { get; set; }
        private bool CanOpenCrackOrderPanelCommandExecute(object p) => true;
        private void OnOpenCrackOrderPanelCommandExecuted(object p)
        {
            SellPanelVisibility = Visibility.Collapsed;
            ReturnPanelVisibility = Visibility.Collapsed;
            CrackPanelVisibility = Visibility.Visible;
            MenuPanelVisibility = Visibility.Collapsed;
        }

        public RelayCommand OpenMenuPanelCommand { get; set; }
        private bool CanOpenMenuPanelCommandExecute(object p) => true;
        private void OnOpenMenuPanelCommandExecuted(object p)
        {
            SellPanelVisibility = Visibility.Collapsed;
            ReturnPanelVisibility = Visibility.Collapsed;
            CrackPanelVisibility = Visibility.Collapsed;
            MenuPanelVisibility = Visibility.Visible;
        }


        #endregion
        public CashRegisterViewModel()
        {
            AddProductInBasketCommand = new RelayCommand(OnAddProductInBasketCommandExecuted, CanAddProductInBasketCommandExecute);
            IncreaseAmountProductBasketCommand = new RelayCommand(OnIncreaseAmountProductBasketCommandExecuted, CanIncreaseAmountProductBasketCommandExecute);
            DecreaseAmountProductBasketCommand = new RelayCommand(OnDecreaseAmountProductBasketCommandExecuted, CanDecreaseAmountProductBasketCommandExecute);
            RemoveProductInBasketCommand = new RelayCommand(OnRemoveProductInBasketCommandExecuted, CanRemoveProductInBasketCommandExecute);
            RemoveCurrentOrderCommand = new RelayCommand(OnRemoveCurrentOrderCommandExecuted, CanRemoveCurrentOrderCommandExecute);

            OpenSellOrderPanelCommand = new RelayCommand(OnOpenSellOrderPanelCommandExecuted, CanOpenSellOrderPanelCommandExecute);
            OpenReturnOrderPanelCommand = new RelayCommand(OnOpenReturnOrderPanelCommandExecuted, CanOpenReturnOrderPanelCommandExecute);
            OpenCrackOrderPanelCommand = new RelayCommand(OnOpenCrackOrderPanelCommandExecuted, CanOpenCrackOrderPanelCommandExecute);
            OpenMenuPanelCommand = new RelayCommand(OnOpenMenuPanelCommandExecuted, CanOpenMenuPanelCommandExecute);
        }
    }
}
