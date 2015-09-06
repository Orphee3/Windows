using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Orphee.RestApiManagement;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class ProfilePageViewModel : ViewModel, IProfilePageViewModel, INavigationAware
    {
        public string UserName { get; private set; }
        public int NumberOfCreations { get; private set; }
        public int NumberOfComments { get; private set; }
        public int NumberOfFollows { get; private set; }
        public int NumberOfFollowers { get; private set; }
        public Visibility DisconnectedStackPanelVisibility { get; private set; }
        public Visibility ConnectedStackPanelVisibility { get; private set; }
        public DelegateCommand LoginCommand { get; private set; }

        public ProfilePageViewModel()
        {
            SetPropertiesSependingOnConnectionState();
            this.LoginCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Login", null));
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            App.MyNavigationService.CurrentPageName = "Profile";
            SetPropertiesSependingOnConnectionState();
        }

        private void SetPropertiesSependingOnConnectionState()
        {
            if (RestApiManagerBase.Instance.IsConnected && RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
            {
                //this.UserPhoto = RestApiManagerBase.Instance.UserPhoto;
                this.DisconnectedStackPanelVisibility = Visibility.Collapsed;
                this.ConnectedStackPanelVisibility = Visibility.Visible;
                this.UserName = RestApiManagerBase.Instance.UserData.User.UserName;
                this.NumberOfCreations = RestApiManagerBase.Instance.UserData.User.Creations.Count;
                this.NumberOfComments = RestApiManagerBase.Instance.UserData.User.Comments.Count;
                this.NumberOfFollows = 0;
                this.NumberOfFollowers = 0;
            }
            else
            {
                this.DisconnectedStackPanelVisibility = Visibility.Visible;
                this.ConnectedStackPanelVisibility = Visibility.Collapsed;
            }
        }
    }
}
