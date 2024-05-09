using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cashbox.MVVM.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthView.xaml
    /// </summary>
    public partial class AuthView : UserControl
    {
        public AuthView()
        {
            InitializeComponent();
        }

        private void Pin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char number = Convert.ToChar(e.Text);
            if (number == '0' && ((PasswordBox)sender).Password.Length == 0)
                e.Handled = true;
            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }

        private void Pin_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null) { ((dynamic)DataContext).SecurePassword = ((PasswordBox)sender).Password; }
        }

        private void Pin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                unpin.Focus();
                unpin.Command.Execute(null);
                Pin.Focus();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext != null) 
            {
                Pin.Password = string.Empty;
                ((dynamic)DataContext).SecurePassword = Pin.Password;
                Pin.Focus();
            }
        }
    }
}
