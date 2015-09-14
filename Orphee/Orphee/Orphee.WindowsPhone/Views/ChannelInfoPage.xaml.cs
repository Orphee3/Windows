using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Models;


namespace Orphee.Views
{
    public sealed partial class ChannelInfoPage : IView
    {
        public ChannelInfoPage()
        {
            this.InitializeComponent();
        }

        private void CommentaryIcon_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var creation = ((Button) sender).DataContext as Creation;
            App.MyNavigationService.Navigate("CreationInfo", creation);
        }
    }
}
