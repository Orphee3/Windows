using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Microsoft.Practices.Prism.Mvvm;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels;

namespace Orphee.Views
{
    public sealed partial class HomePage : IView
    {
        private int _x1, _x2;
        public HomePage()
        {
            this.InitializeComponent();
        }

        private void Play(object sender, TappedRoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void UIElement_OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            this._x2 = (int) e.Position.X;
            if (this._x1 > this._x2 && (this.PopularTextBox.Foreground as SolidColorBrush).Color == Color.FromArgb(100, 13, 71, 161))
            {
                if (RestApiManagerBase.Instance.IsConnected)
                    ((HomePageViewModel) this.DataContext).FillFlowListWithNewFriendCreations();
                else
                    App.MyNavigationService.Navigate("Login", null);
            }
            else if (this._x1 < this._x2 && (this.FriendNewsTextBox.Foreground as SolidColorBrush).Color == Color.FromArgb(100, 13, 71, 161))
                ((HomePageViewModel) this.DataContext).FillFlowListWithPopularCreations();
        }

        private void UIElement_OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            this._x1 = (int)e.Position.X;
        }

        private void UserPicture_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var channel = ((Ellipse) sender).DataContext as User;
            App.MyNavigationService.Navigate("ChannelInfo", channel);
        }

        private void InfoStackPannel_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var creation = ((Button)sender).DataContext as User;
            App.MyNavigationService.Navigate("CreationInfo", creation);
        }

        private void LikeButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            
        }
    }
}
