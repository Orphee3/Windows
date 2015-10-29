using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels;

namespace Orphee.Views
{
    public sealed partial class ProfilePage : IView
    {
        public ProfilePage()
        {
            this.InitializeComponent();
            if (RestApiManagerBase.Instance.IsConnected)
                RestApiManagerBase.Instance.UserData.User.PropertyChanged += OnNotificationReceiverPropertyChanged;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (RestApiManagerBase.Instance.IsConnected)
            {
                RestApiManagerBase.Instance.UserData.User.PropertyChanged -= OnNotificationReceiverPropertyChanged;
                this.MyBottomAppBar.Unload();
            }
        }

        private async void OnNotificationReceiverPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "_pictureHasBeenUplaodedWithSuccess" && RestApiManagerBase.Instance.UserData.User.PictureHasBeenUplaodedWithSuccess)
                await Task.Run(() => Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ((ProfilePageViewModel)this.DataContext).UpdatePictureSource();
                    RestApiManagerBase.Instance.UserData.User.PictureHasBeenUplaodedWithSuccess = false;
                }));
        }
    }
}
