using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Orphee.UI
{
    public sealed partial class MyBottomAppBar : UserControl
    { 
        public MyBottomAppBar()
        {
            this.InitializeComponent();
        }

        private void Button_Tapped(object sender, RoutedEventArgs e)
        {
            App.MyNavigationService.Navigate(((Button)sender).Name, null);
        }
    }
}
