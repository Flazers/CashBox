using System.Windows.Controls;
using System.Windows.Input;

namespace Cashbox.MVVM.Views.Pages.Admin
{
    /// <summary>
    /// Логика взаимодействия для EmployeesView.xaml
    /// </summary>
    public partial class EmployeesView : UserControl
    {
        public EmployeesView()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (((TextBox)sender).Text.Length >= 6)
            {
                e.Handled = true;
                return;
            }
            char number = Convert.ToChar(e.Text);
            if (number == '0' && ((TextBox)sender).Text.Length == 0)
                e.Handled = true;
            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }
    }
}
