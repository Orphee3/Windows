using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Practices.Prism.Mvvm;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Models;

namespace Orphee.Views
{
    public sealed partial class FriendPage : IView
    {
        public FriendPage()
        {
            this.InitializeComponent();
        }

        private void UserStackPanel_OnTapped(object sender, ItemClickEventArgs e)
        {
            var user = e.ClickedItem as UserBase;
            App.MyNavigationService.Navigate("ChannelInfo", JsonConvert.SerializeObject(user));
        }

        private void ForwardSign_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            App.MyNavigationService.Navigate("Invitation", null);
        }
    }
}
 