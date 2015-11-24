using System;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    /// <summary>
    /// ProfilePage view model
    /// </summary>
    public class ProfilePageViewModel : ViewModelExtend, IProfilePageViewModel
    {
        private Visibility _disconnectedStackPanelVisibility;
        /// <summary>Visible if the user is disconnected. Hidden otherwise </summary>
        public Visibility DisconnectedStackPanelVisibility
        {
            get { return this._disconnectedStackPanelVisibility; }
            set
            {
                if (this._disconnectedStackPanelVisibility != value)
                    SetProperty(ref this._disconnectedStackPanelVisibility, value);
            }
        }
        private Visibility _connectedStackPanelVisibility;
        /// <summary>Visible if the user is connected. Hidden otherwise </summary>
        public Visibility ConnectedStackPanelVisibility
        {
            get { return this._connectedStackPanelVisibility; }
            set
            {
                if (this._connectedStackPanelVisibility != value)
                    SetProperty(ref this._connectedStackPanelVisibility, value);
            }
        }
        /// <summary>Redirects to MyCreationPage</summary>
        public DelegateCommand MyCreationsCommand { get; private set; }
        /// <summary>Logs the user out</summary>
        public DelegateCommand NotificationsCommand { get; private set; }
        /// <summary>Redirects to LoginPage</summary>
        public DelegateCommand FriendPageCommand { get; private set; }
        /// <summary>Redirects to FriendPage</summary>
        public DelegateCommand LoginCommand { get; private set; }
        /// <summary>Redirects to EditProfilePage</summary>
        public DelegateCommand ParametersCommand { get; private set; }
        public DelegateCommand ChangePictureCommand { get; private set; }

        public LoggedUser LoggedInUser { get; private set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public ProfilePageViewModel()
        {
            this.LoginCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Login", null));
            this.MyCreationsCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("MyCreations", null));
            this.NotificationsCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Notification", null));
            this.FriendPageCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Friend", null));
            this.ParametersCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Parameters", null));
            this.ChangePictureCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("ChangePicture", null));
        }

        /// <summary>
        /// Called when navigated to this page
        /// </summary>
        /// <param name="navigationParameter"></param>
        /// <param name="navigationMode"></param>
        /// <param name="viewModelState"></param>
        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            App.MyNavigationService.CurrentPageName = "Profile";
            if (RestApiManagerBase.Instance.IsConnected)
                this.LoggedInUser = RestApiManagerBase.Instance.UserData.User;
            SetDisconnectedAndConnectedStackVisibility();
        }

        private void SetDisconnectedAndConnectedStackVisibility()
        {
            var isUserConnected = RestApiManagerBase.Instance.IsConnected;
            this.ConnectedStackPanelVisibility = isUserConnected ? Visibility.Visible : Visibility.Collapsed;
            this.DisconnectedStackPanelVisibility = isUserConnected ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
