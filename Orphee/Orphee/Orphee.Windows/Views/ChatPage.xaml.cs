using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Microsoft.Practices.Prism.Mvvm;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels;

namespace Orphee.Views
{
    public sealed partial class ChatPage : IView
    {
        public ChatPage()
        {
            this.InitializeComponent();
            this.Loaded += (sender, args) => ScrollToBottom();
            if (RestApiManagerBase.Instance.IsConnected)
                RestApiManagerBase.Instance.UserData.User.PropertyChanged += OnNotificationReceiverPropertyChanged;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (RestApiManagerBase.Instance.IsConnected)
                RestApiManagerBase.Instance.UserData.User.PropertyChanged -= OnNotificationReceiverPropertyChanged;
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

        private void ScrollToBottom()
        {
            var selectedIndex = ConversationListView.Items.Count - 1;
            if (selectedIndex < 0)
                return;

            ConversationListView.SelectedIndex = selectedIndex;
            ConversationListView.UpdateLayout();

            ConversationListView.ScrollIntoView(ConversationListView.SelectedItem);
        }

        private void UserPicture_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var channel = ((Message) ((Ellipse) sender).DataContext).User;
            App.MyNavigationService.Navigate("ChannelInfo", JsonConvert.SerializeObject(channel));
        }
    }
}
