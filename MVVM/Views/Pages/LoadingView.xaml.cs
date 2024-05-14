using Cashbox.Core;
using System.Windows;
using System.Windows.Controls;

namespace Cashbox.MVVM.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoadingView.xaml
    /// </summary>
    public partial class LoadingView : UserControl
    {
        public LoadingView()
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
