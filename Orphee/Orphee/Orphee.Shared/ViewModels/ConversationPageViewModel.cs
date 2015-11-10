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
using Orphee.RestApiManagement.Models.Interfaces;
using Orphee.ViewModels.Interfaces;
using Q42.WinRT.Data;

namespace Orphee.ViewModels
{
    /// <summary>
    /// ConversationPage view model
    /// </summary>
    public class ConversationPageViewModel : ViewModelExtend, IConversationPageViewModel
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
        private readonly IConversationParser _conversationParser;

        /// <summary>
        /// Constructor initializing getter
        /// through dependency injection
        /// </summary>
        /// <param name="getter">Manages the sending of the "Get" requests</param>
        public ConversationPageViewModel(IGetter getter, IConversationParser conversationParser)
        {
            this._getter = getter;
            this._conversationParser = conversationParser;
            this.ConversationList = new ObservableCollection<Conversation>();
            if (RestApiManagerBase.Instance.IsConnected)
            {
                ResetVisibility(true);
                SetProgressRingVisibility(true);
            }
            this.CreateNewConversationCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Friend", ""));
            this.LoginButton = new DelegateCommand(() => App.MyNavigationService.Navigate("Login", null));
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            App.MyNavigationService.CurrentPageName = "Conversation";
            if (RestApiManagerBase.Instance.IsConnected)
                InitConversationList();
            else
                ResetVisibility(false);
            SetProgressRingVisibility(false);
        }


        private void InitConversationList()
        {
            if (this.ButtonsVisibility == Visibility.Collapsed)
                ResetVisibility(true);
            foreach (var conversation in RestApiManagerBase.Instance.UserData.User.ConversationList)
                this.ConversationList.Add(conversation);
        }

        /// <summary>
        /// Creates a new conversation
        /// </summary>
        /// <param name="conversation">Contains the data related to the conversation to be created</param>
        public void CreateNewConversation(Conversation conversation)
        {
            if (string.IsNullOrEmpty(conversation.Name))
                SetConversationName(conversation);
            if (conversation.UserList.Count == 0 || conversation.UserList == null)
                conversation.ConversationPictureSource = conversation.UserList[0].Picture;           
            SetProgressRingVisibility(false);
        }

        private void SetConversationName(Conversation conversation)
        {
            if (conversation.UserList.Count == 0 || conversation.UserList == null)
                return;
            foreach (var user in conversation.UserList)
            {
                conversation.Name += user.Name;
                if (user != conversation.UserList.Last())
                    conversation.Name += " ,";
            }
            if (conversation.Name.Length >= 30)
                conversation.Name = conversation.Name.Substring(0, 30) + "...";
        }

        public void InitConversation()
        {
            foreach (var message in RestApiManagerBase.Instance.UserData.User.PendingMessageList)
            {
                var conversationList = message.Type == "private message" ? this.ConversationList.Where(t => t.IsPrivate).Where(t => t.UserList[0].Id == message.User.Id).ToList() : this.ConversationList.Where(t => !t.IsPrivate).Where(t => t.Id == message.TargetRoom).ToList();
                if (conversationList.Count == 0)
                    GetNewConversation(message);
                else
                    AddMessageToConversationMessageList(message, conversationList[0]);
            }
            RestApiManagerBase.Instance.UserData.User.PendingMessageList.Clear();
        }

        private async void GetNewConversation(Message message)
        {
            var conversationList = await this._getter.GetInfo<List<Conversation>>(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + RestApiManagerBase.Instance.UserData.User.Id + "/rooms");
            if (!VerifyReturnedValue(conversationList, ""))
                conversationList = RestApiManagerBase.Instance.UserData.User.ConversationList;
            this._conversationParser.ParseConversationList(conversationList);
            this.ConversationList.Add(conversationList.First());
            RestApiManagerBase.Instance.UserData.User.ConversationList.Add(conversationList.First());
            AddMessageToConversationMessageList(message, this.ConversationList.Last());
        }

        private void AddMessageToConversationMessageList(Message message, Conversation conversation)
        {
            message.SetProperties();
            conversation.Messages.Add(message);
            conversation.LastMessageDateString = DateTime.Now.ToString("HH:mm");
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
    }
}
