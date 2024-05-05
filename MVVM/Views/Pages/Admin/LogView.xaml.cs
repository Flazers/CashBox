using System.Windows.Controls;
using System.Windows.Input;

namespace Cashbox.MVVM.Views.Pages.Admin
{
    public partial class LogView : UserControl
    {
        public LogView()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char number = Convert.ToChar(e.Text);
            if (number == '0' && ((TextBox)sender).Text.Length == 0)
                e.Handled = true;
            if (!Char.IsDigit(number))
                e.Handled = true;
        }

        private void Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ReloadBtn.Focus();
                ReloadBtn.Command.Execute(null);
            }
        }
    }
}
