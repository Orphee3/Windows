using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Newtonsoft.Json;
using Orphee.RestApiManagement;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class ConversationPageViewModel : ViewModel, IConversationPageViewModel
    {
        public ObservableCollection<Conversation> ConversationList { get; set; }
        public DelegateCommand CreateNewConversationCommand { get; private set; }
        public DelegateCommand LoginButton { get; private set; }
        private Visibility _buttonsVisibility;
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
        public Visibility ListViewVisibility
        {
            get { return this._listViewVisibility; }
            set
            {
                if (this._listViewVisibility != value)
                    SetProperty(ref this._listViewVisibility, value);
            }
        }
        private readonly IConversationGetter _conversationGetter;
        private readonly IUserFriendListGetter _userFriendListGetter;
        private readonly IMessageListGetter _messageListGetter;

        public ConversationPageViewModel(IConversationGetter conversationGetter, IUserFriendListGetter userFriendListGetter, IMessageListGetter messageListGetter)
        {
            this._messageListGetter = messageListGetter;
            this._conversationGetter = conversationGetter;
            this._userFriendListGetter = userFriendListGetter;
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

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            App.MyNavigationService.CurrentPageName = "Messages";
            if (RestApiManagerBase.Instance.IsConnected)
            {
                var conversationList = await this._conversationGetter.GetUserConversations();
                var userFriends = await this._userFriendListGetter.GetUserFriendList();
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
                        conversation.Messages = await this._messageListGetter.GetRoomMessageList(conversation.UserList[0].Id);
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

        public void CreateNewConversation(Conversation conversation)
        {
            if (conversation.UserList.Count == 0)
                return;
            string channelName = "";
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

        public void CreateNewConversationOnMessageReceived(Message message)
        {
            
        }
    }
}
