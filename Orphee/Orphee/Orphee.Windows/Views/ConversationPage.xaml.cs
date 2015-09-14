using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement;
using Orphee.RestApiManagement.Models;

namespace Orphee.Views
{
    public sealed partial class ConversationPage : IView
    {
        public ConversationPage()
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
