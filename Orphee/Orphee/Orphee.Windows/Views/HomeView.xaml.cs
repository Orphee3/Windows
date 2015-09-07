using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Microsoft.Practices.Prism.Mvvm;
using Newtonsoft.Json.Linq;
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
                ((HomePageViewModel)this.DataContext).FillFlowListWithNewFriendCreations();
            else if (this._x1 < this._x2 && (this.FriendNewsTextBox.Foreground as SolidColorBrush).Color == Color.FromArgb(100, 13, 71, 161))
                ((HomePageViewModel)this.DataContext).FillFlowListWithPopularCreations();
        }

        private void UIElement_OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            this._x1 = (int)e.Position.X;
        }

        private void UserPicture_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var creation = sender as JObject;
            App.MyNavigationService.Navigate("ChannelInfo", creation);
        }
    }
}
