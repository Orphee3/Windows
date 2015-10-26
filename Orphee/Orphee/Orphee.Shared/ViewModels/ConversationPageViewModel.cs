using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Models.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    /// <summary>
    /// ConversationPage view model
    /// </summary>
    public class ConversationPageViewModel : ViewModel, IConversationPageViewModel, ILoadingScreenComponents
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
                this.ProgressRingVisibility = Visibility.Visible;
                this.IsProgressRingActive = true;
            }
            this.CreateNewConversationCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Friend", ""));
            this.LoginButton = new DelegateCommand(() => App.MyNavigationService.Navigate("Login", null));
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            App.MyNavigationService.CurrentPageName = "Messages";
            if (!RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
            {
                DisplayMessage("Connexion unavailable");
                return;
            }
            if (navigationParameter != null)
            {
                CreateNewConversation(navigationParameter as Conversation);
                return;
            }
            if (RestApiManagerBase.Instance.IsConnected)
                InitConversationList();
            else
                ResetVisibility(false);
        }

        private void ResetVisibility(bool isConnected)
        {
            if (isConnected)
            {
                this.ButtonsVisibility = Visibility.Collapsed;
                this.ListViewVisibility = Visibility.Visible;
            }
            else
            {
                this.ButtonsVisibility = Visibility.Visible;
                this.ListViewVisibility = Visibility.Collapsed;
            }
        }

        private async void InitConversationList()
        {
            ResetVisibility(true);
            foreach (var conversation in RestApiManagerBase.Instance.UserData.User.ConversationList)
            {
                Conversation conversationWithMessages;
                if ((conversationWithMessages = await RetrieveConversationMessages(conversation)) == null)
                    return;
                this.ConversationList.Add(conversationWithMessages);
            }
            this.IsProgressRingActive = false;
            this.ProgressRingVisibility = Visibility.Collapsed;
        }

        private async Task<Conversation> RetrieveConversationMessages(Conversation conversation)
        {
            try
            {
                conversation.Messages = await this._getter.GetInfo<List<Message>>(RestApiManagerBase.Instance.RestApiPath["roomMessages"] + "/" + conversation.UserList[0].Id);
            }
            catch (Exception)
            {
                DisplayMessage("Request failed");
                return null;
            }
            foreach (var message in conversation.Messages)
                message.SetProperties();
            conversation.Messages.Reverse();
            conversation.LastMessagePreview = conversation.Messages.Last().ReceivedMessage;
            return conversation;
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
            this.IsProgressRingActive = false;
            this.ProgressRingVisibility = Visibility.Collapsed;
        }

        public void InitConversation()
        {
            foreach (var message in RestApiManagerBase.Instance.UserData.User.PendingMessageList)
            {
                if (message.Type == "private message" && this.ConversationList.All(t => t.UserList[0].Id != message.User.Id))
                    this.ConversationList.Add(new Conversation() {Name = message.User.Name, UserList = new List<User>() {message.User}, ConversationPictureSource = message.UserPictureSource});
            }
        }

        private async void DisplayMessage(string message)
        {
            var messageDialog = new MessageDialog(message);

            await messageDialog.ShowAsync();
        }
    }
}
