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

        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            //MessageBoxResult res = MessageBox.Show("закрыть окно", "", MessageBoxButton.YesNo);
            //if (res == MessageBoxResult.Yes)
            //{
            //    e.Cancel = false;
            //    return;
            //}
            //MessageBox.Show("не закрыл");
            //e.Cancel = true;
        }

    }
}