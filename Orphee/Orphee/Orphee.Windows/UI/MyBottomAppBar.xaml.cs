using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Orphee.RestApiManagement;

namespace Orphee.UI
{
    public sealed partial class MyBottomAppBar
    {
        private Dictionary<string, Button> _buttonList; 
        public MyBottomAppBar()
        {
            this.InitializeComponent();
            this.InitButtonList();
            this.InitButtonColorForeground();
            if (RestApiManagerBase.Instance.IsConnected)
            {
                RestApiManagerBase.Instance.UserData.User.PropertyChanged += OnNotificationReceiverPropertyChanged;
                this.Profile.NotificationDotVisibility = RestApiManagerBase.Instance.UserData.User.HasReceivedFriendNotification ? Visibility.Visible : Visibility.Collapsed;
                this.Messages.NotificationDotVisibility = RestApiManagerBase.Instance.UserData.User.HasReceivedMessageNotification ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void Button_Tapped(object sender, RoutedEventArgs e)
        {
            var button = (AppBarButton) sender;
            if (button.Name == "Profile" || button.Name == "Messages")
                ResetNotificationDotVisibility((MyAppBarButton) button);
            App.MyNavigationService.Navigate(button.Name, null);
            App.MyNavigationService.SetNewAppBarButtonColorValue();
        }

        private static void ResetNotificationDotVisibility(MyAppBarButton button)
        {
            if (button.Name == "Profile" && button.NotificationDotVisibility == Visibility.Visible)
            {
                button.NotificationDotVisibility = Visibility.Collapsed;
                RestApiManagerBase.Instance.UserData.User.HasReceivedFriendNotification = false;
            }
            else if (button.Name == "Messages" && button.NotificationDotVisibility == Visibility.Visible)
            {
                button.NotificationDotVisibility = Visibility.Collapsed;
                RestApiManagerBase.Instance.UserData.User.HasReceivedMessageNotification = false;
            }
        }

        private async void OnNotificationReceiverPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "_hasReceivedFriendNotification" || e.PropertyName == "_hasReceivedFriendValidationNotification")
                await Task.Run(() => Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { this.Profile.NotificationDotVisibility = Visibility.Visible; }));
            else if (e.PropertyName == "_hasReceivedMessageNotification")
                await Task.Run(() => Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { this.Messages.NotificationDotVisibility = Visibility.Visible; }));
        }

        private void InitButtonColorForeground()
        {
            for (var i = 0; i < 4; i++)
               this._buttonList.Values.ElementAt(i).Foreground = App.MyNavigationService.ButtonForegroundColorList.Values.ElementAt(i);
        }

        private void InitButtonList()
        {
            this._buttonList = new Dictionary<string, Button>()
            {
                { "Home", this.Home },
                { "Friends", this.Friends },
                { "Messages", this.Messages },
                { "Profile", this.Profile},
            };
        }
    }
}
