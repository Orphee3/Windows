using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Mvvm;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Models;


namespace Orphee.Views
{
    public sealed partial class ChannelInfoPage : IView
    {
        public ChannelInfoPage()
        {
            this.InitializeComponent();
            if (!RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
                this.TextBlock.Visibility = Visibility.Visible;
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private void CommentaryIcon_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var creation = ((Button) sender).DataContext as Creation;
            App.MyNavigationService.Navigate("CreationInfo", JsonConvert.SerializeObject(creation));
        }
    }
}
