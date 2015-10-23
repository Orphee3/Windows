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
            RestApiManagerBase.Instance.UserData.User.PropertyChanged += OnNotificationReceiverPropertyChanged;
        }

        private async void OnNotificationReceiverPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        //    await Task.Run(() => Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
        //    {
        //        if (e.PropertyName == "_hasReceivedMessageNotification")
        //            ((ConversationPageViewModel)this.DataContext).InitNewConversation(RestApiManagerBase.Instance.UserData.User.PendingMessageList);
        //        RestApiManagerBase.Instance.UserData.User.HasReceivedMessageNotification = false;
        //        RestApiManagerBase.Instance.UserData.User.PendingMessageList.Clear();
        //    }));
        }

        private void Creation_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var conversation = ((StackPanel)sender).DataContext as Conversation;
            App.MyNavigationService.Navigate("Chat", conversation);
        }
    }
}
