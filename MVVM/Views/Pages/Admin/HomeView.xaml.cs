using ScottPlot;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            
        }

        public void RefreshDataPlot()
        {
            DateTime date = DateTime.Today;
            int days = DateTime.DaysInMonth(date.Year, date.Month);

            double[] xDays = new double[days];
            double[] yMoney = new double[days];

            for (int i = 0; i < days; i++)
                xDays[i] = i+1;
            

            for (int i = 0; i < days; i++)
            {
                Random rnd = new();
                yMoney[i] = rnd.Next(2000, 20000);
            }

            var bars = WpfPlot1.Plot.Add.Bars(xDays, yMoney);

            foreach (var bar in bars.Bars) 
            { 
                bar.Label = bar.Value.ToString();
            }


            bars.ValueLabelStyle.Bold = false;
            bars.ValueLabelStyle.FontSize = 14;

            WpfPlot1.Plot.Axes.Margins(bottom: 0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WpfPlot1.Plot.Clear();
            RefreshDataPlot();
            WpfPlot1.Refresh();
        }
    }
}
