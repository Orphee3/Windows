using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Core;
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
    }
}
