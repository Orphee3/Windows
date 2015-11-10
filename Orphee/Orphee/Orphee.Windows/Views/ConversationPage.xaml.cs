using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Mvvm;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels;

namespace Orphee.Views
{
    public sealed partial class ConversationPage : IView
    {
        public ConversationPage()
        {
            this.InitializeComponent();
            if (App.InternetAvailabilityWatcher.IsInternetUp && RestApiManagerBase.Instance.IsConnected)
                RestApiManagerBase.Instance.UserData.User.PropertyChanged += UserOnPropertyChanged; 
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (App.InternetAvailabilityWatcher.IsInternetUp && RestApiManagerBase.Instance.IsConnected)
                RestApiManagerBase.Instance.UserData.User.PropertyChanged -= UserOnPropertyChanged;
            this.MyBottomAppBar.Unload();
        }

        private async void UserOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
             if (e.PropertyName == "_hasReceivedMessageNotification")
                await Task.Run(() => Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ((ConversationPageViewModel)this.DataContext).InitConversation();
                    RestApiManagerBase.Instance.UserData.User.HasReceivedMessageNotification = false;
                }));
        }

        private void Creation_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var conversation = ((Grid)sender).DataContext as Conversation;
            App.MyNavigationService.Navigate("Chat", JsonConvert.SerializeObject(conversation));
        }
    }
}
