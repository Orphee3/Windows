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
    /// ConversationPage view model
    /// </summary>
    public class ConversationPageViewModel : ViewModel, IConversationPageViewModel
    {
        /// <summary>List of conversation that the user has </summary>
        public ObservableCollection<Conversation> ConversationList { get; set; }
        /// <summary>Redirects to the FriendPage so the user can select which friend to include in the new conversation </summary>
        public DelegateCommand CreateNewConversationCommand { get; private set; }
        /// <summary>Redirect to the LoginPage </summary>
        public DelegateCommand LoginButton { get; private set; }
        private Visibility _buttonsVisibility;
        /// <summary>Visible if the user is disconnected. Hidden otherwise </summary>
        public Visibility ButtonsVisibility
        {
            get { return this._buttonsVisibility; }
            set
            {
                if (this._buttonsVisibility != value)
                    SetProperty(ref this._buttonsVisibility, value);
            }
        }
        private Visibility _listViewVisibility;
        /// <summary>Visible if the user is connected. Hidder otherwise </summary>
        public Visibility ListViewVisibility
        {
            get { return this._listViewVisibility; }
            set
            {
                if (this._listViewVisibility != value)
                    SetProperty(ref this._listViewVisibility, value);
            }
        }
        private readonly IGetter _getter;

        /// <summary>
        /// Constructor initializing getter
        /// through dependency injection
        /// </summary>
        /// <param name="getter">Manages the sending of the "Get" requests</param>
        public ConversationPageViewModel(IGetter getter)
        {
            this._getter = getter;
            this.ButtonsVisibility = Visibility.Visible;
            this.ListViewVisibility = Visibility.Collapsed;
            this.ConversationList = new ObservableCollection<Conversation>();
            if (RestApiManagerBase.Instance.IsConnected)
            {
                this.ButtonsVisibility = Visibility.Collapsed;
                this.ListViewVisibility = Visibility.Visible;
            }
            this.CreateNewConversationCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Friend", ""));
            this.LoginButton = new DelegateCommand(() => App.MyNavigationService.Navigate("Login", null));
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode,
            Dictionary<string, object> viewModelState)
        {
            App.MyNavigationService.CurrentPageName = "Messages";
            if (!RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
            {
                DisplayMessage("Connexion unavailable");
                return;
            }
            if (RestApiManagerBase.Instance.IsConnected)
            {
                List<Conversation> conversationList;
                List<User> userFriends;
                try
                {
                    conversationList = await this._getter.GetInfo<List<Conversation>>(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + RestApiManagerBase.Instance.UserData.User.Id + "/rooms");
                    userFriends = await this._getter.GetInfo<List<User>>(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + RestApiManagerBase.Instance.UserData.User.Id + "/friends");
                }
                catch (Exception)
                {
                    DisplayMessage("Request failed");
                    return;
                }
                foreach (var conversation in conversationList)
                    if (this.ConversationList.Count(c => c.Id == conversation.Id) == 0)
                    {
                        foreach (var user in conversation.Users)
                        {
                            if (user.ToString() != RestApiManagerBase.Instance.UserData.User.Id)
                            {
                                conversation.ConversationPictureSource = userFriends.FirstOrDefault(uf => uf.Id == user.ToString()).Picture ?? "/Assets/defaultUser.png";
                                conversation.UserList.Add(userFriends.FirstOrDefault(uf => uf.Id == user.ToString()));
                            }
                        }
                        if (conversation.UserList.Count == 1)
                            conversation.Name = conversation.UserList[0].Name;
                        conversation.Messages = await this._getter.GetInfo<List<Message>>(RestApiManagerBase.Instance.RestApiPath["roomMessages"] + "/" + conversation.UserList[0].Id);
                        foreach (var message in conversation.Messages)
                            message.SetProperties();
                        conversation.Messages.Reverse();
                        conversation.LastMessagePreview = conversation.Messages.Last().ReceivedMessage;
                        this.ConversationList.Add(conversation);
                    }
                this.ButtonsVisibility = Visibility.Collapsed;
                this.ListViewVisibility = Visibility.Visible;
            }
            else
            {
                this.ButtonsVisibility = Visibility.Visible;
                this.ListViewVisibility = Visibility.Collapsed;
            }
            if (navigationParameter != null)
                CreateNewConversation(navigationParameter as Conversation);
        }

        /// <summary>
        /// Creates a new conversation
        /// </summary>
        /// <param name="conversation">Contains the data related to the conversation to be created</param>
        public void CreateNewConversation(Conversation conversation)
        {
            if (conversation.UserList.Count == 0)
                return;
            string channelName;
            if (string.IsNullOrEmpty(conversation.Name))
            {
                channelName = conversation.UserList.Aggregate("", (current, user) => current + user.Name);
                if (channelName.Length >= 30)
                    channelName = channelName.Substring(0, 30) + "...";
            }
            else
                channelName = conversation.Name;
            this.ConversationList.Add(new Conversation() {Name = channelName, UserList = conversation.UserList, ConversationPictureSource = conversation.UserList.Count > 1 ? "/Assets/defaultUser.png" : conversation.UserList[0].Picture });
        }

        private async void DisplayMessage(string message)
        {
            var messageDialog = new MessageDialog(message);

            await messageDialog.ShowAsync();
        }
    }
}
