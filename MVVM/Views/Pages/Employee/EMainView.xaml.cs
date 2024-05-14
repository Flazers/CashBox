using Cashbox.Core;
using System.Windows;
using System.Windows.Controls;

namespace Cashbox.MVVM.Views.Pages.Employee
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class EMainView : UserControl
    {
        public EMainView()
        {
            InitializeComponent();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            scale.ScaleX = (UICommand.SizeWidth - 14) / 1920;
            scale.ScaleY = scale.ScaleX;
        }
    }
}
