using Windows.UI.Xaml.Input;
using Microsoft.Practices.Prism.Mvvm;


namespace Orphee.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChannelInfoPage : IView
    {
        public ChannelInfoPage()
        {
            this.InitializeComponent();
        }

        private void CommentaryIcon_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            App.MyNavigationService.Navigate("CreationInfo", null);
        }
    }
}
