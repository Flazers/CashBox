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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cashbox.MVVM.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoadScreen.xaml
    /// </summary>
    public partial class LoadScreen : UserControl
    {
        public LoadScreen()
        {
            InitializeComponent();
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = 0,
                To = 360,
                Duration = TimeSpan.FromSeconds(1),
                RepeatBehavior = RepeatBehavior.Forever,
            };
            angle.BeginAnimation(RotateTransform.AngleProperty, animation);
        }
    }
}
