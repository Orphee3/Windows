using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Mvvm;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Models;

namespace Orphee.Views
{
    public sealed partial class MyCreationsPage : IView
    {
        public MyCreationsPage()
        {
            this.InitializeComponent();
            if (!RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
                this.TextBlock.Visibility = Visibility.Visible;
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private void Creation_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var creation = ((Grid) sender).DataContext as Creation;
            App.MyNavigationService.Navigate("CreationInfo", JsonConvert.SerializeObject(creation));
        }
    }
}
