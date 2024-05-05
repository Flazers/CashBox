using System.Windows.Controls;

namespace Cashbox.MVVM.Views.Pages.Admin
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

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
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
