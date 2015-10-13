using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels;

namespace Orphee.Views
{
    public sealed partial class ChatPage : IView
    {
        public ChatPage()
        {
            this.InitializeComponent();
            RestApiManagerBase.Instance.UserData.User.PropertyChanged += OnNotificationReceiverPropertyChanged;
        }

        private async void OnNotificationReceiverPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            await Task.Run(() => Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (e.PropertyName == "_hasReceivedMessageNotification")
                    ((ChatPageViewModel) this.DataContext).InitConversation(RestApiManagerBase.Instance.UserData.User.PendingMessageList);
                RestApiManagerBase.Instance.UserData.User.HasReceivedMessageNotification = false;
                RestApiManagerBase.Instance.UserData.User.PendingMessageList.Clear();
            }));
        }

        private void UserPicture_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var channel = ((Message) ((Ellipse) sender).DataContext).User;
            App.MyNavigationService.Navigate("ChannelInfo", channel);
        }
    }
}
