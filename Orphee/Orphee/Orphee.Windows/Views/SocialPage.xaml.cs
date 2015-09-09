using Windows.UI.Xaml.Input;
using Microsoft.Practices.Prism.Mvvm;

namespace Orphee.Views
{
    public sealed partial class SocialPage : IView
    {
        public SocialPage()
        {
            this.InitializeComponent();
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            App.MyNavigationService.Navigate("ChannelInfo", null);
        }
    }
}
