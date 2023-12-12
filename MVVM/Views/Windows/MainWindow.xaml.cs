using Cashbox.Core;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Cashbox.Core.AppCommand;


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
            //Closing += MainWindow_Closing;
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