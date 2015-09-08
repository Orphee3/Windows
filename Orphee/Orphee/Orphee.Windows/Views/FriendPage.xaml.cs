using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement;
using Orphee.ViewModels;

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

        private void ForwardSign_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            App.MyNavigationService.Navigate("Invitation", null);
        }
    }
}
 