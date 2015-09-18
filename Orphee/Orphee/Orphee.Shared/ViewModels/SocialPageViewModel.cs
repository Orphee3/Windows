using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class SocialPageViewModel : ViewModel, ISocialPageViewModel
    {
        public ObservableCollection<User> UserList { get; set; }
        public DelegateCommand LoginCommand { get; private set; }
        public DelegateCommand<User> AddFriendCommand { get; private set; }
        private readonly IFriendshipAsker _friendshipAsker;
        private readonly IUserListGetter _userListGetter;

        public SocialPageViewModel(IFriendshipAsker friendshipAsker, IUserListGetter userListGetter)
        {
            this._friendshipAsker = friendshipAsker;
            this._userListGetter = userListGetter;
            this.UserList = new ObservableCollection<User>();
            this.LoginCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Login", null));
            this.AddFriendCommand = new DelegateCommand<User>(NewFriendCommandExec);
        }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
            {
                var temporaryList = (await this._userListGetter.GetUserList(0, 30)).OrderBy(u => u.UserName);
                foreach (var user in temporaryList)
                {
                    if (string.IsNullOrEmpty(user.Picture))
                        user.Picture = "/Assets/defaultUser.png";
                    if (!RestApiManagerBase.Instance.IsConnected)
                        user.AddButtonVisibility = Visibility.Collapsed;
                    this.UserList.Add(user);
                }
            }
        }

        private async void NewFriendCommandExec(User friend)
        {
            bool? result;
            if (RestApiManagerBase.Instance.IsConnected && RestApiManagerBase.Instance.NotificationRecieiver.IsInternet() && (result = await this._friendshipAsker.SendFriendshipRequestToRestApi(friend.Id)) != null)
            { 
                var messageDialog = result == true ? new MessageDialog("Friendship request sent to " + friend.UserName) : new MessageDialog("Friendship already asked");

                await messageDialog.ShowAsync();
            }
        }
    }
}
