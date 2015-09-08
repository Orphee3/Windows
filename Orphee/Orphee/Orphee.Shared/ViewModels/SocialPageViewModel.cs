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
        public ObservableCollection<User> FriendNameList { get; set; }
        public DelegateCommand LoginButton { get; private set; }
        public DelegateCommand<User> NewFriendCommand { get; private set; }
        public DelegateCommand RegisterButton { get; private set; }
        public string DisconnectedMessage { get; private set; }
        public Visibility ButtonsVisibility { get; private set; }
        public Visibility ListViewVisibility { get; private set; }
        private readonly IFriendshipAsker _friendshipAsker;

        public SocialPageViewModel(IFriendshipAsker friendshipAsker)
        {
            this._friendshipAsker = friendshipAsker;
            this.DisconnectedMessage = "To access your friend list info you have \nto login or to create an account";
            this.ButtonsVisibility = Visibility.Visible;
            this.ListViewVisibility = Visibility.Collapsed;
            this.FriendNameList = new ObservableCollection<User>();
            if (RestApiManagerBase.Instance.IsConnected && RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
            {
                this.ButtonsVisibility = Visibility.Collapsed;
                this.ListViewVisibility = Visibility.Visible;
                foreach (var friend in RestApiManagerBase.Instance.UserNameList)
                    this.FriendNameList.Add(friend);
            }
            this.LoginButton = new DelegateCommand(() => App.MyNavigationService.Navigate("Login", null));
            this.RegisterButton = new DelegateCommand(() => App.MyNavigationService.Navigate("Register", null));
            this.NewFriendCommand = new DelegateCommand<User>(NewFriendCommandExec);
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (RestApiManagerBase.Instance.IsConnected && RestApiManagerBase.Instance.NotificationRecieiver.IsInternet() && this.ButtonsVisibility == Visibility.Visible)
            {
                this.ButtonsVisibility = Visibility.Collapsed;
                this.ListViewVisibility = Visibility.Visible;
                foreach (var friend in RestApiManagerBase.Instance.UserNameList)
                    this.FriendNameList.Add(friend);
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
