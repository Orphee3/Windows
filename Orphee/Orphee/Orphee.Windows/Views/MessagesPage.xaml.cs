using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement;

namespace Orphee.Views
{
    public sealed partial class MessagesPage : IView
    {
        public MessagesPage()
        {
            this.InitializeComponent();
        }

        private void Creation_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var conversation = ((StackPanel)sender).DataContext as Conversation;
            App.MyNavigationService.Navigate("Chat", conversation);
        }
    }
}
