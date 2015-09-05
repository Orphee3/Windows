using Windows.UI;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Microsoft.Practices.Prism.Mvvm;
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

        private void Info(object sender, TappedRoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void UIElement_OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            this._x2 = (int) e.Position.X;
            if (this._x1 > this._x2 && (this.PopularTextBox.Foreground as SolidColorBrush).Color == Colors.Red)
                ((HomePageViewModel)this.DataContext).FillFlowListWithNewFriendCreations();
            else if (this._x1 < this._x2 && (this.FriendNewsTextBox.Foreground as SolidColorBrush).Color == Colors.Red)
                ((HomePageViewModel)this.DataContext).FillFlowListWithPopularCreations();
        }

        private void UIElement_OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            this._x1 = (int)e.Position.X;
        }
    }
}
