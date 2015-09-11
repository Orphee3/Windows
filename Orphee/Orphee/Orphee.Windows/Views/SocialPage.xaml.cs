using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement;

namespace Orphee.Views
{
    public sealed partial class SocialPage : IView
    {
        public SocialPage()
        {
            this.InitializeComponent();
        }

        private void User_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var user = e.ClickedItem as User;
            App.MyNavigationService.Navigate("ChannelInfo", user);
        }
    }
}
