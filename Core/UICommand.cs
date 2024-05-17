using System.Windows;

namespace Cashbox.Core
{
    public class UICommand : ViewModelBase
    {

        private static double _sizeWidth = 1920;
        public static double SizeWidth 
        { 
            get => _sizeWidth; 
            set => _sizeWidth = value;
        }
    }
}
