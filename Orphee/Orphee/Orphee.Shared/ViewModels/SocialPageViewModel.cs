using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    /// <summary>
    /// ViewModel of the SocialPage
    /// </summary>
    public class SocialPageViewModel : ViewModelExtend, ISocialPageViewModel
    {
        /// <summary>List of the searched users </summary>
        public ObservableCollection<UserBase> UserList { get; set; }
        /// <summary>Login command</summary>
        public DelegateCommand LoginCommand { get; private set; }
        /// <summary>Add new friend command</summary>
        public DelegateCommand<UserBase> AddFriendCommand { get; private set; }

        private readonly IGetter _getter;

        /// <summary>
        /// Constructor initializing getter through 
        /// dependency injection
        /// </summary>
        /// <param name="getter"></param>
        public SocialPageViewModel(IGetter getter)
        {
            this._getter = getter;
            SetProgressRingVisibility(true);
            this.UserList = new ObservableCollection<UserBase>();
            this.LoginCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Login", null));
            this.AddFriendCommand = new DelegateCommand<UserBase>(NewFriendCommandExec);
        }

        /// <summary>
        /// Called when we navigate to this page
        /// </summary>
        /// <param name="navigationParameter"></param>
        /// <param name="navigationMode"></param>
        /// <param name="viewModelState"></param>
        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        { 
            if (!App.InternetAvailabilityWatcher.IsInternetUp)
            {
                SetProgressRingVisibility(false);
                return;
            }
            var requestedUserList = await this._getter.GetInfo<List<UserBase>>(RestApiManagerBase.Instance.RestApiPath["users"] + "?offset=" + 0 + "&size=" + 20);
            if (!VerifyReturnedValue(requestedUserList, "")) 
                return;
            RemoveUserFromList(requestedUserList);
            AddRequestedUsersToUserList(requestedUserList);
            SetProgressRingVisibility(false);
        }

        private void RemoveUserFromList(List<UserBase> requestedUserList)
        {
            if (RestApiManagerBase.Instance.IsConnected)
                requestedUserList.Remove(requestedUserList.FirstOrDefault(u => u.Name == RestApiManagerBase.Instance.UserData.User.Name));
        }

        private void AddRequestedUsersToUserList(List<UserBase> requestedUserList)
        {
            foreach (var user in requestedUserList)
            {
                if (!RestApiManagerBase.Instance.IsConnected)
                    user.AddButtonVisibility = Visibility.Collapsed;
                this.UserList.Add(user);
            }
        }

        private async void NewFriendCommandExec(UserBase friend)
        {
            if (App.InternetAvailabilityWatcher.IsInternetUp)
            {
                var requestResult = await this._getter.GetInfo<string>(RestApiManagerBase.Instance.RestApiPath["askfriend"] + "/" + friend.Id);
                if (!VerifyReturnedValue(requestResult, "Error : Friendship request was not sent"))
                    return;
                var stringToDisplay = requestResult == "already send" ? "Friendship already asked" : "Friendship request sent to " + friend.UserName;
                DisplayMessage(stringToDisplay);
            }
            else
                DisplayMessage("Internet connexion unavailable");
        }
    }
}
