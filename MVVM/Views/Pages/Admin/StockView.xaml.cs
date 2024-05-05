using System.Windows.Controls;
using System.Windows.Input;

namespace Cashbox.MVVM.Views.Pages.Admin
{
    /// <summary>
    /// Логика взаимодействия для StockView.xaml
    /// </summary>
    public partial class StockView : UserControl
    {
        public StockView()
        {
            InitializeComponent();
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char number = Convert.ToChar(e.Text);
            if (number == ',' && ((TextBox)sender).Text.Length == 0)
                e.Handled = true;
            if (number == ',')
                if (((TextBox)sender).Text.Contains(number)) e.Handled = true;
                else return;
            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }

        private void Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchBtn.Focus();
                SearchBtn.Command.Execute(null);
            }
        }
    }
}
