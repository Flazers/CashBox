using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.Models;
using Cashbox.MVVM.ViewModels.Data;
using System.Collections.ObjectModel;
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

        private double _startCash = MoneyBoxViewModel.GetMoney;
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

        private string? _currentCash;
        public string? CurrentCash
        {
            get => _currentCash;
            set => Set(ref _currentCash, value);
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

        private double _fullTransit;
        public double FullTransit
        {
            get => _fullTransit;
            set => Set(ref _fullTransit, value);
        }

        private ObservableCollection<OrderViewModel>? _collectionOrders;
        public ObservableCollection<OrderViewModel>? CollectionOrders
        {
            get => _collectionOrders;
            set => Set(ref _collectionOrders, value);
        }

        private TimeOnly? _startShiftTime;
        public TimeOnly? StartShiftTime
        {
            get => _startShiftTime;
            set => Set(ref _startShiftTime, value);
        }

        private TimeOnly? _endShiftTime;
        public TimeOnly? EndShiftTime
        {
            get => _endShiftTime;
            set => Set(ref _endShiftTime, value);
        }

        private DateOnly _currentDate = DateOnly.FromDateTime(DateTime.Today);
        public DateOnly CurrentDate
        {
            get => _currentDate;
            set => Set(ref _currentDate, value);
        }
        #endregion

        #region Command

        #region VisibilityCommand
        public RelayCommand StartShiftCommand { get; set; }
        private bool CanStartShiftCommandExecute(object p)
        {
            return true;
        }
        private async void OnStartShiftCommandExecuted(object p)
        {
            StartShiftTime = TimeOnly.FromDateTime(DateTime.Now);
            DailyReportViewModel drvm = await DailyReportViewModel.StartShift(CurrentDate, (TimeOnly)StartShiftTime);
            StartShiftVisibility = Visibility.Collapsed;
            ProcessShiftVisibility = Visibility.Visible;
            ProcessDoShiftVisibility = Visibility.Visible;
            EndShiftVisibility = Visibility.Collapsed;
            MessageBox.Show($"Смена {drvm.Id} открыта", "Уведомление", MessageBoxButton.OK);
        }

        public RelayCommand EndShiftCommand { get; set; }
        private bool CanEndShiftCommandExecute(object p) => true;
        private async void OnEndShiftCommandExecuted(object p)
        {
            if (string.IsNullOrEmpty(CurrentCash))
            {
                MessageBox.Show("Пересчитайте деньги в кассе и введите значение в поле \"Денег в кассе\"", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            MessageBoxResult result = MessageBox.Show("После закрытия смены, новую можно открыть только на следующий день. \nВы уверены, что хотите ее закрыть?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                return;
            EndShiftTime = TimeOnly.FromDateTime(DateTime.Now);
            double cash = double.Parse(CurrentCash);
            DailyReportViewModel drvm = await DailyReportViewModel.EndShift(CurrentDate, EndShiftTime, cash);
            StartShiftVisibility = Visibility.Collapsed;
            ProcessShiftVisibility = Visibility.Visible;
            ProcessDoShiftVisibility = Visibility.Collapsed;
            EndShiftVisibility = Visibility.Visible;
            MessageBox.Show($"Смена {drvm.Id} закрыта", "Уведомление", MessageBoxButton.OK);
        }

        #endregion


        #endregion

        public override async void OnLoad()
        {
            DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Today);
            CardTransit = (await OrderViewModel.GetDayOrdersToMethod(dateOnly, 1)).Sum(x => (double)x.SellCost!);
            NalTransit = (await OrderViewModel.GetDayOrdersToMethod(dateOnly, 2)).Sum(x => (double)x.SellCost!);
            SendTransit = (await OrderViewModel.GetDayOrdersToMethod(dateOnly, 3)).Sum(x => (double)x.SellCost!);
            NewCash = StartCash;
            if (DailyReport.CurrentShift != null)
                if (DailyReport.CurrentShift.CloseTime != null)
                    NewCash = StartCash + NalTransit;
            FullTransit = SendTransit + CardTransit + NalTransit;
            if (StartShiftVisibility == Visibility.Collapsed)
                CollectionOrders = new(Order.GetAllDayOrders(dateOnly).Result);
        }

        public ShiftViewModel()
        {
            StartShiftCommand = new RelayCommand(OnStartShiftCommandExecuted, CanStartShiftCommandExecute);
            EndShiftCommand = new RelayCommand(OnEndShiftCommandExecuted, CanEndShiftCommandExecute);
            DailyReport CurrentShift = DailyReportViewModel.CurrentShift;
            if (CurrentShift != null)
            {
                StartShiftTime = CurrentShift.OpenTime;
                StartShiftVisibility = Visibility.Collapsed;
                ProcessShiftVisibility = Visibility.Visible;
                if (CurrentShift.CloseTime != null)
                {
                    EndShiftTime = CurrentShift.CloseTime;
                    ProcessDoShiftVisibility = Visibility.Collapsed;
                    EndShiftVisibility = Visibility.Visible;
                }
                else
                {
                    ProcessDoShiftVisibility = Visibility.Visible;
                    EndShiftVisibility = Visibility.Collapsed;
                }

            }
        }
    }
}
