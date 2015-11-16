using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;
using Q42.WinRT.Data;

namespace Orphee.ViewModels
{
    /// <summary>
    /// ChatPage view model
    /// </summary>
    public class ChatPageViewModel : ViewModelExtend, IChatPageViewModel
    {
        /// <summary>Redirects to the previous page </summary>
        public DelegateCommand BackCommand { get; private set; }
        /// <summary>Sends a message to the other users participating at the conversation </summary>
        public DelegateCommand SendCommand { get; private set; }
        private string _message;
        /// <summary>Message to be sent </summary>
        public string Message
        {
            get { return this._message; }
            set
            {
                if (this._message != value)
                {
                    SetProperty(ref this._message, value);
                }
            }
        }
        /// <summary>List of messages </summary>
        public ObservableCollection<Message> Conversation { get; private set; }
        private string _conversationName;
        /// <summary>Name of the conversation </summary>
        public string ConversationName
        {
            get { return this._conversationName; }
            set
            {
                if (this._conversationName != value)
                    SetProperty(ref this._conversationName, value);
            }
        }
        private Conversation _actualConversation;
        private readonly IGetter _getter;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChatPageViewModel(IGetter getter)
        {
            this.BackCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Conversation", null));
            this.Conversation = new ObservableCollection<Message>();
            this.SendCommand = new DelegateCommand(SendCommandExec);
            this._getter = getter;
        }

        /// <summary>
        /// Initialize the conversation with the given
        /// message list
        /// </summary>
        /// <param name="messages">Messages used to initialize the conversation</param>
        public void InitConversation(List<Message> messages)
        {
            foreach (var message in messages)
            {
                message.SetProperties();
                this.Conversation.Add(message);
            }
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            this._actualConversation = JsonConvert.DeserializeObject<Conversation>(navigationParameter as string);
            this.Conversation.Clear();
            this._actualConversation.Messages.Clear();
            this.ConversationName = this._actualConversation.Name;
            GetConversationMessages();
            InitConversation(this._actualConversation.Messages);
        }

        private async void GetConversationMessages()
        {
            var request = !this._actualConversation.IsPrivate ? RestApiManagerBase.Instance.RestApiPath["group room"] + this._actualConversation.Id + "/groupMessage" : RestApiManagerBase.Instance.RestApiPath["private room"] + this._actualConversation.UserList[0].Id;
            this._actualConversation.Messages = await this._getter.GetInfo<List<Message>>(request);
            if (!VerifyReturnedValue(this._actualConversation.Messages, ""))
                this._actualConversation.Messages = RestApiManagerBase.Instance.UserData.User.ConversationList.FirstOrDefault(c => c.Id == this._actualConversation.Id).Messages;
            if (this._actualConversation.Messages == null || this._actualConversation.Messages.Count <= 0)
                return;
            AddNewMessagesToMessageList();
        }

        private void AddNewMessagesToMessageList()
        {
            foreach (var message in this._actualConversation.Messages)
            {
                message.SetProperties();
                this.Conversation.Insert(0, message);
                if (RestApiManagerBase.Instance.UserData.User.ConversationList.FirstOrDefault(c => c.Id == this._actualConversation.Id).Messages.All(m => m.Id != message.Id))
                    RestApiManagerBase.Instance.UserData.User.ConversationList.FirstOrDefault(c => c.Id == this._actualConversation.Id).Messages.Add(message);
            }
            this._actualConversation.Messages.Reverse();
        }

        private void SendCommandExec()
        {
            if (string.IsNullOrEmpty(this.Message))
                return;
            var newMessage = new Message
            {
                User = RestApiManagerBase.Instance.UserData.User,
                Date = DateTime.Now,
                ReceivedMessage = this.Message
            };
            newMessage.SetProperties();
            this.Conversation.Add(newMessage);
            SendMessage();
            this.Message = string.Empty;
        }

        private async void SendMessage()
        {
            if (!(await App.InternetAvailabilityWatcher.SocketManager.SocketEmitter.SendMessage(this.Message, this._actualConversation.IsPrivate ? this._actualConversation.UserList[0].Id : this._actualConversation.Id, this._actualConversation.IsPrivate)))
                DisplayMessage("This message wasn't sent");
        }
    }
}
