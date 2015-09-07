using Windows.UI.Input;
using Windows.UI.Xaml.Input;
using Microsoft.Practices.Prism.Mvvm;


namespace Orphee.Views
{
    public sealed partial class MessagesPage : IView
    {
        public MessagesPage()
        {
            this.InitializeComponent();
        }

        private void Convesation_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            App.MyNavigationService.Navigate("Chat", null);
        }
    }
}
