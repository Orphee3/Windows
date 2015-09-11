using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement;
using Orphee.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Orphee.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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
