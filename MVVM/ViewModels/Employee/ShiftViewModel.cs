using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.Models;
using Cashbox.MVVM.ViewModels.Data;
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

        private double _processed;
        public double Processed
        {
            get => _processed;
            set => Set(ref _processed, value);
        }

        private TimeOnly _startShiftTime;
        public TimeOnly StartShiftTime
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
        private bool CanStartShiftCommandExecute(object p) 
        {
            return true;
        }
        private async void OnStartShiftCommandExecuted(object p)
        {
            StartShiftTime = TimeOnly.FromDateTime(DateTime.Now);
            DailyReportViewModel drvm = await DailyReportViewModel.StartShift(DateOnly.FromDateTime(DateTime.Now), StartShiftTime);
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
            MessageBoxResult result = MessageBox.Show("После закрытия смены, новую можно открыть только на следующий день. \nВы уверены, что хотите ее закрыть?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                return;
            EndShiftTime = DateTime.Now;
            DailyReportViewModel drvm = await DailyReportViewModel.EndShift(EndShiftTime, Processed);
            StartShiftVisibility = Visibility.Collapsed;
            ProcessShiftVisibility = Visibility.Visible;
            ProcessDoShiftVisibility = Visibility.Collapsed;
            EndShiftVisibility = Visibility.Visible;
            MessageBox.Show($"Смена {drvm.Id} закрыта", "Уведомление", MessageBoxButton.OK);
        }

        #endregion


        #endregion

        public override async void Clear()
        {
            CardTransit = (await OrderViewModel.GetDayOrdersToMethod(DateOnly.FromDateTime(DateTime.Now), 1)).Sum(x => (double)x.SellCost!);
            NalTransit = (await OrderViewModel.GetDayOrdersToMethod(DateOnly.FromDateTime(DateTime.Now), 2)).Sum(x => (double)x.SellCost!);
            SendTransit = (await OrderViewModel.GetDayOrdersToMethod(DateOnly.FromDateTime(DateTime.Now), 3)).Sum(x => (double)x.SellCost!);
        }

        public override void OnLoad()
        {
            if (EndShiftTime.Year != DateTime.Now.Year)
                return;
            StartShiftVisibility = Visibility.Visible;
            ProcessShiftVisibility = Visibility.Collapsed;
            ProcessDoShiftVisibility = Visibility.Collapsed;
            EndShiftVisibility = Visibility.Collapsed;

        }

        public ShiftViewModel()
        {
            StartShiftCommand = new RelayCommand(OnStartShiftCommandExecuted, CanStartShiftCommandExecute);
            EndShiftCommand = new RelayCommand(OnEndShiftCommandExecuted, CanEndShiftCommandExecute);
            DailyReport CurrentShift = DailyReportViewModel.CurrentShift;
            if (CurrentShift != null)
            {
                StartShiftVisibility = Visibility.Collapsed;
                ProcessShiftVisibility = Visibility.Visible;
                ProcessDoShiftVisibility = Visibility.Visible;
                EndShiftVisibility = Visibility.Collapsed;
                StartShiftTime = CurrentShift.OpenTime!.Value;
            }
        }
    }
}
