using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cashbox.MVVM.ViewModels.Employee
{
    public class ShiftViewModel : ViewModelBase
    {
        #region Props
        public static UserViewModel? User { get => Models.User.CurrentUser; }

        #region Visibility
        private Visibility _startShiftVisibility = Visibility.Visible;
        public Visibility StartShiftVisibility
        {
            get => _startShiftVisibility;
            set => Set(ref _startShiftVisibility, value);
        }

        private Visibility _processShiftVisibility = Visibility.Collapsed;
        public Visibility ProcessShiftVisibility
        {
            get => _processShiftVisibility;
            set => Set(ref _processShiftVisibility, value);
        }

        private Visibility _processDoShiftVisibility = Visibility.Collapsed;
        public Visibility ProcessDoShiftVisibility
        {
            get => _processDoShiftVisibility;
            set => Set(ref _processDoShiftVisibility, value);
        }

        private Visibility _endShiftVisibility = Visibility.Collapsed;
        public Visibility EndShiftVisibility
        {
            get => _endShiftVisibility;
            set => Set(ref _endShiftVisibility, value);
        }

        

        #endregion

        private double _startCash;
        public double StartCash
        {
            get => _startCash;
            set => Set(ref _startCash, value);
        }

        private double _newCash;
        public double NewCash
        {
            get => _newCash;
            set => Set(ref _newCash, value);
        }

        private double _nalTransit;
        public double NalTransit
        {
            get => _nalTransit;
            set => Set(ref _nalTransit, value);
        }

        private double _sendTransit;
        public double SendTransit
        {
            get => _sendTransit;
            set => Set(ref _sendTransit, value);
        }

        private double _cardTransit;
        public double CardTransit
        {
            get => _cardTransit;
            set => Set(ref _cardTransit, value);
        }

        private DateTime _startShiftTime;
        public DateTime StartShiftTime
        {
            get => _startShiftTime;
            set => Set(ref _startShiftTime, value);
        }

        private DateTime _endShiftTime;
        public DateTime EndShiftTime
        {
            get => _endShiftTime;
            set => Set(ref _endShiftTime, value);
        }
        #endregion

        #region Command

        #region VisibilityCommand
        public RelayCommand StartShiftCommand { get; set; }
        private bool CanStartShiftCommandExecute(object p) => true;
        private void OnStartShiftCommandExecuted(object p)
        {
            StartShiftTime = DateTime.Now;
            StartShiftVisibility = Visibility.Collapsed;
            ProcessShiftVisibility = Visibility.Visible;
            ProcessDoShiftVisibility = Visibility.Visible;
            EndShiftVisibility = Visibility.Collapsed;
        }

        public RelayCommand EndShiftCommand { get; set; }
        private bool CanEndShiftCommandExecute(object p) => true;
        private void OnEndShiftCommandExecuted(object p)
        {
            EndShiftTime = DateTime.Now;
            StartShiftVisibility = Visibility.Collapsed;
            ProcessShiftVisibility = Visibility.Visible;
            ProcessDoShiftVisibility = Visibility.Collapsed;
            EndShiftVisibility = Visibility.Visible;
        }

        #endregion


        #endregion

        public override void OnLoad()
        {
            CardTransit = OrderViewModel.GetDayOrdersToMethod(DateOnly.FromDateTime(StartShiftTime), 1).Result.Sum(x => (double)x.SellCost!);
            NalTransit = OrderViewModel.GetDayOrdersToMethod(DateOnly.FromDateTime(StartShiftTime), 2).Result.Sum(x => (double)x.SellCost!);
            SendTransit = OrderViewModel.GetDayOrdersToMethod(DateOnly.FromDateTime(StartShiftTime), 3).Result.Sum(x => (double)x.SellCost!);
        }

        public ShiftViewModel()
        {
            StartShiftCommand = new RelayCommand(OnStartShiftCommandExecuted, CanStartShiftCommandExecute);
            EndShiftCommand = new RelayCommand(OnEndShiftCommandExecuted, CanEndShiftCommandExecute);
        }
    }
}
