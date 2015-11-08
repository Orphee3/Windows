using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Mvvm;
using Newtonsoft.Json;
using Orphee.RestApiManagement;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels;

namespace Orphee.Views
{
    public sealed partial class FriendPage : IView
    {
        public FriendPage()
        {
            this.InitializeComponent();
            if (!RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
                this.TextBlock.Visibility = Visibility.Visible;
        }

        private void UserStackPanel_OnTapped(object sender, ItemClickEventArgs e)
        {
            var user = e.ClickedItem as User;
            App.MyNavigationService.Navigate("ChannelInfo", JsonConvert.SerializeObject(user));
        }

        private void ForwardSign_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            App.MyNavigationService.Navigate("Invitation", null);
        }
    }
}
 