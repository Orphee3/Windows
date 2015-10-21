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
    /// <summary>
    /// ViewModel of the SocialPage
    /// </summary>
    public class SocialPageViewModel : ViewModel, ISocialPageViewModel
    {
        /// <summary>List of the searched users </summary>
        public ObservableCollection<User> UserList { get; set; }
        /// <summary>Login command</summary>
        public DelegateCommand LoginCommand { get; private set; }
        /// <summary>Add new friend command</summary>
        public DelegateCommand<User> AddFriendCommand { get; private set; }
        private readonly IGetter _getter;

        /// <summary>
        /// Constructor initializing getter through 
        /// dependency injection
        /// </summary>
        /// <param name="getter"></param>
        public SocialPageViewModel(IGetter getter)
        {
            this._getter = getter;
            this.UserList = new ObservableCollection<User>();
            this.LoginCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Login", null));
            this.AddFriendCommand = new DelegateCommand<User>(NewFriendCommandExec);
        }

        /// <summary>
        /// Called when we navigate to this page
        /// </summary>
        /// <param name="navigationParameter"></param>
        /// <param name="navigationMode"></param>
        /// <param name="viewModelState"></param>
        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (!RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
            {
                DisplayMessage("Connexion unavailable");
                return;
            }
            List<User> temporaryList;
            try
            {
                temporaryList = (await this._getter.GetInfo<List<User>>(RestApiManagerBase.Instance.RestApiPath["users"] + "?offset=" + 0 + "&size=" + 20)).OrderBy(u => u.UserName).ToList();
            }
            catch (Exception)
            {
                DisplayMessage("Request failed");
                return;
            }

            if (RestApiManagerBase.Instance.IsConnected)
                temporaryList.Remove(temporaryList.FirstOrDefault(u => u.Name == RestApiManagerBase.Instance.UserData.User.Name));
            foreach (var user in temporaryList)
            {
                if (string.IsNullOrEmpty(user.Picture))
                    user.Picture = "/Assets/defaultUser.png";
                if (!RestApiManagerBase.Instance.IsConnected)
                    user.AddButtonVisibility = Visibility.Collapsed;
                this.UserList.Add(user);
            }
        }

        private async void NewFriendCommandExec(User friend)
        {
            string result = "";
            try
            {
                if (RestApiManagerBase.Instance.IsConnected && RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
                    result = await this._getter.GetInfo<string>(RestApiManagerBase.Instance.RestApiPath["askfriend"] + "/" + friend.Id);
            }
            catch (Exception)
            {
                DisplayMessage("Request failed");
                return;
            }
            var stringToDisplay = result == "already send" ? "Friendship already asked" : "Friendship request sent to " + friend.UserName;
            DisplayMessage(stringToDisplay);
        }

        private async void DisplayMessage(string message)
        {
            var messageDialog = new MessageDialog(message);

            await messageDialog.ShowAsync();
        }
    }
}
