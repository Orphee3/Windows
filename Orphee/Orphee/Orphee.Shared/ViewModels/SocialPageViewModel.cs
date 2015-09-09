using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement;
using Orphee.RestApiManagement.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class SocialPageViewModel : ViewModel, ISocialPageViewModel
    {
        public ObservableCollection<User> UserNameList { get; set; }
        public DelegateCommand LoginCommand { get; private set; }
        public DelegateCommand<User> NewFriendCommand { get; private set; }
        public Visibility ButtonsVisibility { get; private set; }
        public Visibility ListViewVisibility { get; private set; }
        private readonly IFriendshipAsker _friendshipAsker;

        public SocialPageViewModel(IFriendshipAsker friendshipAsker)
        {
            this._friendshipAsker = friendshipAsker;
            this.ButtonsVisibility = Visibility.Visible;
            this.ListViewVisibility = Visibility.Collapsed;
            this.UserNameList = new ObservableCollection<User>();
            if (RestApiManagerBase.Instance.IsConnected && RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
            {
                this.ButtonsVisibility = Visibility.Collapsed;
                this.ListViewVisibility = Visibility.Visible;
                foreach (var friend in RestApiManagerBase.Instance.UserNameList)
                    this.UserNameList.Add(friend);
            }
            this.LoginCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Login", null));
            this.NewFriendCommand = new DelegateCommand<User>(NewFriendCommandExec);
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (RestApiManagerBase.Instance.IsConnected && RestApiManagerBase.Instance.NotificationRecieiver.IsInternet() && this.ButtonsVisibility == Visibility.Visible)
            {
                this.ButtonsVisibility = Visibility.Collapsed;
                this.ListViewVisibility = Visibility.Visible;
                foreach (var friend in RestApiManagerBase.Instance.UserNameList)
                    this.UserNameList.Add(friend);
            }
        }

        private async void NewFriendCommandExec(User friend)
        {
            if (RestApiManagerBase.Instance.IsConnected && RestApiManagerBase.Instance.NotificationRecieiver.IsInternet() && await this._friendshipAsker.SendFriendshipRequestToRestApi(friend.Id))
            {
                var messageDialog = new MessageDialog("Friendship request sent to " + friend.UserName);

                await messageDialog.ShowAsync();
            }
        }
    }
}
