using System;
using System.Collections.Generic;
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

namespace Cashbox.MVVM.Views.Pages.Employee
{
    /// <summary>
    /// Логика взаимодействия для CashRegisterView.xaml
    /// </summary>
    public partial class CashRegisterView : UserControl
    {
        public CashRegisterView()
        {
            InitializeComponent();
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char number = Convert.ToChar(e.Text);

            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }
    }
}
