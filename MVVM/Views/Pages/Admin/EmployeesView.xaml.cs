using Cashbox.MVVM.ViewModels.Admin;
using OpenTK.Windowing.GraphicsLibraryFramework;
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

        private void Salary_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char number = Convert.ToChar(e.Text);
            if (number == ',' && ((TextBox)sender).Text.Length == 0)
                e.Handled = true;
            if (number == ',')
                if (((TextBox)sender).Text.Contains(number)) e.Handled = true;
                else return;
            if (!Char.IsDigit(number))
                e.Handled = true;
        }

        private void EditSurname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                EditName.Focus();
        }

        private void EditName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                EditPatronymic.Focus();
        }

        private void EditPatronymic_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                EditLocation.Focus();
        }

        private void EditLocation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                EditPhone.Focus();
        }

        private void EditPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                EditPin.Focus();
        }

        private void Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchBtn.Focus();
                SearchBtn.Command.Execute(null);
                Search.Focus();
            }
        }

        private void Surname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Name.Focus();
        }

        private void Name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Patronymic.Focus();
        }

        private void Patronymic_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Location.Focus();
        }

        private void Location_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Phone.Focus();
        }
    }
}
