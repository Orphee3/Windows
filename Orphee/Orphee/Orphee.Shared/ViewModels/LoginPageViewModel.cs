using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Models.Interfaces;
using Orphee.RestApiManagement.Posters.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    /// <summary>
    /// LoginPage view model
    /// </summary>
    public class LoginPageViewModel : ViewModel, ILoginPageViewModel, ILoadingScreenComponents
    {
        /// <summary>Logs the user to the server </summary>
        public DelegateCommand LoginCommand { get; private set; }
        /// <summary>Redirects to the previous page </summary>
        public DelegateCommand BackCommand { get; private set; }
        /// <summary>User name of the user </summary>
        public string UserName { get; set; }
        /// <summary>User's password </summary>
        public string Password { get; set; }

        private bool _isProgressRingActive;

        public bool IsProgressRingActive
        {
            get { return this._isProgressRingActive; }
            set
            {
                if (this._isProgressRingActive != value)
                    SetProperty(ref this._isProgressRingActive, value);
            }
        }
        private Visibility _progressRingVisibility;

        public Visibility ProgressRingVisibility
        {
            get { return this._progressRingVisibility; }
            set
            {
                if (this._progressRingVisibility != value)
                    SetProperty(ref this._progressRingVisibility, value);
            }
        }
        private readonly IConnectionManager _connectionManager;
        private readonly IConversationParser _conversationParser;
        private readonly INewsParser _newsParser;
        private readonly IGetter _getter;

        /// <summary>
        /// Constructor initializing connectionManager
        /// through dependency injection
        /// </summary>
        /// <param name="connectionmanager">Manages the connetion of the user so it gets logged in</param>
        public LoginPageViewModel(IConnectionManager connectionmanager, INewsParser newsParser, IGetter getter, IConversationParser conversationParser)
        {
            this._conversationParser = conversationParser;
            this._connectionManager = connectionmanager;
            this._newsParser = newsParser;
            this._getter = getter;
            this.LoginCommand = new DelegateCommand(LoginCommandExec);
            this.BackCommand = new DelegateCommand(App.MyNavigationService.GoBack);
        }

        private async void LoginCommandExec()
        {
            if (!CheckInternetConnexion())
                return;
            this.IsProgressRingActive = true;
            this.ProgressRingVisibility = Visibility.Visible;
            bool requestResult;
            try
            {
                requestResult = await this._connectionManager.ConnectUser(this.UserName, this.Password);
            }
            catch
            {
                DisplayErrorMessage("Request failed");
                this.IsProgressRingActive = false;
                this.ProgressRingVisibility = Visibility.Collapsed;
                return;
            }
            if (!requestResult)
                DisplayErrorMessage("Wrong user name/password");
            else
            { 
                var result1 = await  GetUserFriends();
                var result = await GetUserConversations();
                var result2 = await GetUserNotifications();
                App.MyNavigationService.GoBack();
            }
            this.IsProgressRingActive = false;
            this.ProgressRingVisibility = Visibility.Collapsed;
        }

        private async Task<bool> GetUserConversations()
        {
            if (!CheckInternetConnexion())
                return false;
            List<Conversation> conversationList;
            try
            {
                conversationList = await this._getter.GetInfo<List<Conversation>>(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + RestApiManagerBase.Instance.UserData.User.Id + "/rooms");
            }
            catch (Exception)
            {
                DisplayErrorMessage("Request failed : Unable to retreive user's conversations");
                return false;
            }
            if (RestApiManagerBase.Instance.UserData.User.ConversationList == null)
            {
                DisplayErrorMessage("Request failed : Unable to access the remote server");
                return false;
            }
            this._conversationParser.ParseConversationList(conversationList);
            return true;
        }

        private async Task<bool> GetUserFriends()
        {
            if (!CheckInternetConnexion())
                return false;
            try
            {
               RestApiManagerBase.Instance.UserData.User.FriendList = (await this._getter.GetInfo<List<User>>(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + RestApiManagerBase.Instance.UserData.User.Id + "/friends")).OrderBy(f => f.Name).ToList();
            }
            catch (Exception)
            { 
               DisplayErrorMessage("Request failed : Unable to retreive user's friends");
            }
            if (RestApiManagerBase.Instance.UserData.User.FriendList == null)
            {
                RestApiManagerBase.Instance.UserData.User.FriendList = new List<User>();
                DisplayErrorMessage("Request failed : Unable to access the remote server");
            }
            return true;
        }

        private async Task<bool> GetUserNotifications()
        {
            if (!CheckInternetConnexion())
                return false;
            try
            {
                RestApiManagerBase.Instance.UserData.User.NotificationList = (await this._getter.GetInfo<List<News>>(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + RestApiManagerBase.Instance.UserData.User.Id + "/news")).Where(t => t.Creator.Id != RestApiManagerBase.Instance.UserData.User.Id).ToList();
            }
            catch (Exception)
            {
                DisplayErrorMessage("Request failed : Unable to retreive user's notifications");
                return false;
            }
            if (RestApiManagerBase.Instance.UserData.User.NotificationList == null)
            {
                RestApiManagerBase.Instance.UserData.User.NotificationList = new List<News>();
                DisplayErrorMessage("Request failed : Unable to access the remote server");
                return false;
            }
            this._newsParser.ParseNewsList();
            return true;
        }

        private bool CheckInternetConnexion()
        {
            if (!RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
            {
                DisplayErrorMessage("Connexion unavailable");
                return false;
            }
            return true;
        }

        private async void DisplayErrorMessage(string message)
        {
            var messageDialog = new MessageDialog(message); 

            await messageDialog.ShowAsync();
        }
    }
}
