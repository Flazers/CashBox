using Cashbox.Core;
using System.ComponentModel;
using System.Windows;


namespace Cashbox.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UICommand.SizeWidth = ActualWidth;
        }
    }
}