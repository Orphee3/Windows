using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels;

namespace Orphee.Views
{
    public sealed partial class ConversationPage : IView
    {
        public ConversationPage()
        {
            this.InitializeComponent();
            if (RestApiManagerBase.Instance.IsConnected && RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
                RestApiManagerBase.Instance.UserData.User.PropertyChanged += UserOnPropertyChanged; 
        }

        private async void UserOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
             if (e.PropertyName == "_hasReceivedMessageNotification")
                await Task.Run(() => Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { ((ConversationPageViewModel)this.DataContext).InitConversation(); }));
        }

        private void Creation_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var conversation = ((Grid)sender).DataContext as Conversation;
            App.MyNavigationService.Navigate("Chat", conversation);
        }
    }
}
