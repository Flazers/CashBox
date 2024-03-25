using System.Windows.Controls;
using System.Windows.Input;

namespace Cashbox.MVVM.Views.Pages.Employee
{
    /// <summary>
    /// Логика взаимодействия для ShiftView.xaml
    /// </summary>
    public partial class ShiftView : UserControl
    {
        public ShiftView()
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
