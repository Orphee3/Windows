using System.ComponentModel;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Microsoft.Practices.Prism.Mvvm;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels;

namespace Orphee.Views
{
    public sealed partial class CreationInfoPage : IView
    {
        public CreationInfoPage()
        {
            this.InitializeComponent();
            if (RestApiManagerBase.Instance.IsConnected)
            {
                RestApiManagerBase.Instance.UserData.User.PropertyChanged += OnNotificationReceiverPropertyChanged;
                ((CreationInfoPageViewModel)this.DataContext).PropertyChanged += OnPropertyChanged;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (RestApiManagerBase.Instance.IsConnected)
            {
                RestApiManagerBase.Instance.UserData.User.PropertyChanged -= OnNotificationReceiverPropertyChanged;
                ((CreationInfoPageViewModel)this.DataContext).PropertyChanged -= OnPropertyChanged;
            }
        }

        private async void OnNotificationReceiverPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            await Task.Run(() => Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (e.PropertyName == "_hasReceivedCommentNotification" && RestApiManagerBase.Instance.UserData.User.HasReceivedCommentNotification)
                    ((CreationInfoPageViewModel)this.DataContext).UpdateCommentList(RestApiManagerBase.Instance.UserData.User.PendingCommentList);
                RestApiManagerBase.Instance.UserData.User.HasReceivedCommentNotification = false;
                RestApiManagerBase.Instance.UserData.User.PendingCommentList.Clear();
            }));
        }

        public void UserPicture_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var channel = ((Comment)((Ellipse)sender).DataContext).Creator;
            App.MyNavigationService.Navigate("ChannelInfo", JsonConvert.SerializeObject(channel));
        }

        private void OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (e.Key == VirtualKey.Enter)
            {
                ((CreationInfoPageViewModel) this.DataContext).SendComment(textbox.Text);
                textbox.Text = string.Empty;
            }
        }

        private async void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var result = await ((CreationInfoPageViewModel) this.DataContext).LikeCommandExec();
            if (result == true)
                FlipOpen.Begin();
            else if (result == false)
                FlipClose.Begin();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsLiked" && ((CreationInfoPageViewModel) this.DataContext).IsLiked == true)
                FlipOpen.Begin();
        }
    }
}
