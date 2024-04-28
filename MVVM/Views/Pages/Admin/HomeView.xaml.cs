using Cashbox.MVVM.ViewModels.Data;
using System.Windows.Controls;

namespace Cashbox.MVVM.Views.Pages.Admin
{
    /// <summary>
    /// Логика взаимодействия для HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
            RefreshDataPlot();
        }

        public async void RefreshDataPlot()
        {
            WpfPlot1.Plot.Clear();
            DateTime date = DateTime.Today;
            var startDate = new DateTime(date.Year, date.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            int days = DateTime.DaysInMonth(date.Year, date.Month);

            double[] xDays = new double[days];
            double[] yMoney = new double[days];

            for (int i = 0; i < days; i++)
                xDays[i] = i + 1;

            for (int i = 0; i < days; i++)
            {
                yMoney[i] = 0;
            }

            List<DailyReportViewModel> data = await DailyReportViewModel.GetPeriodReports(DateOnly.FromDateTime(startDate), DateOnly.FromDateTime(endDate));
            foreach (var item in data)
            {
                yMoney[item.Data!.Value.Day - 1] = item.AutoDreportVM.FullTransit!.Value;
            }

            var bars = WpfPlot1.Plot.Add.Bars(xDays, yMoney);

            foreach (var bar in bars.Bars)
            {
                bar.Label = bar.Value.ToString();
            }


            bars.ValueLabelStyle.Bold = false;
            bars.ValueLabelStyle.FontSize = 14;

            WpfPlot1.Plot.Axes.Margins(bottom: 0);
            WpfPlot1.Refresh();
        }
    }
}
