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

        public static void UpdateColor(String path)
        {
            var uri = new Uri(@path, UriKind.Relative);
            ResourceDictionary? Dictionary = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(Dictionary);
        }

        public static void SwipeDictionary(string colorname)
        {
            //Properties.Settings.Default.Color = colorname;
            //Properties.Settings.Default.Save();
            //UpdateColor("Styles/Colors/" + Properties.Settings.Default.Color + ".xaml");
        }
    }
}
