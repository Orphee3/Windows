﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Orphee.RestApiManagement.Models;

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
                this.Conversation.NotificationDotVisibility = RestApiManagerBase.Instance.UserData.User.HasReceivedMessageNotification ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public void Unload()
        {
            if (RestApiManagerBase.Instance.IsConnected)
                RestApiManagerBase.Instance.UserData.User.PropertyChanged -= OnNotificationReceiverPropertyChanged;
        }

        private void Button_Tapped(object sender, RoutedEventArgs e)
        {
            var button = (AppBarButton) sender;
            if (button.Name == App.MyNavigationService.CurrentPageName)
                return;
            if (button.Name == "Profile" || button.Name == "Conversation")
                ResetNotificationDotVisibility((MyAppBarButton) button);
            App.MyNavigationService.Navigate(button.Name, null);
            App.MyNavigationService.SetNewAppBarButtonColorValue();
        }

        private static void ResetNotificationDotVisibility(MyAppBarButton button)
        {
            if (!RestApiManagerBase.Instance.IsConnected)
                return;
            if (button.Name == "Profile" && button.NotificationDotVisibility == Visibility.Visible)
            {
                button.NotificationDotVisibility = Visibility.Collapsed;
                RestApiManagerBase.Instance.UserData.User.HasReceivedFriendNotification = false;
            }
            else if (button.Name == "Conversation" && button.NotificationDotVisibility == Visibility.Visible)
            {
                button.NotificationDotVisibility = Visibility.Collapsed;
                RestApiManagerBase.Instance.UserData.User.HasReceivedMessageNotification = false;
            }
        }

        private async void OnNotificationReceiverPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "_hasReceivedFriendNotification" || e.PropertyName == "_hasReceivedFriendConfirmationNotification")
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { this.Profile.NotificationDotVisibility = Visibility.Visible; });
            else if (e.PropertyName == "_hasReceivedMessageNotification")
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { this.Conversation.NotificationDotVisibility = Visibility.Visible; });
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
                { "Friends", this.Social },
                { "Conversation", this.Conversation },
                { "Profile", this.Profile},
            };
        }
    }
}
