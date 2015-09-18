using System.ComponentModel;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;
using Microsoft.Practices.Prism.Mvvm;
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
                RestApiManagerBase.Instance.UserData.User.PropertyChanged += OnNotificationReceiverPropertyChanged;
        }

        private async void OnNotificationReceiverPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            await Task.Run(() => Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (e.PropertyName == "_hasReceivedCommentNotification")
                    ((CreationInfoPageViewModel)this.DataContext).UpdateCommentList(RestApiManagerBase.Instance.UserData.User.PendingCommentList);
                RestApiManagerBase.Instance.UserData.User.HasReceivedCommentNotification = false;
                RestApiManagerBase.Instance.UserData.User.PendingCommentList.Clear();
            }));
        }

        public void UserPicture_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var channel = ((Comment)((Ellipse)sender).DataContext).Creator;
            App.MyNavigationService.Navigate("ChannelInfo", channel);
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
    }
}
