using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels;

namespace Orphee.Views
{
    public sealed partial class HomePage : IView
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        private void Play(object sender, TappedRoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }


        private void UserPicture_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var channel = ((Creation)((Ellipse) sender).DataContext).CreatorList[0];
            App.MyNavigationService.Navigate("ChannelInfo", channel);
        }

        private void InfoStackPannel_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var creation = ((Button)sender).DataContext as Creation;
            App.MyNavigationService.Navigate("CreationInfo", creation);
        }

        private void LikeButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            
        }
    }
}
 