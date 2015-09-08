using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement;

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
            var user = e.ClickedItem as User;
            App.MyNavigationService.Navigate("ChannelInfo", user);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
 