using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    /// <summary>
    /// ChatPage view model
    /// </summary>
    public class ChatPageViewModel : ViewModel, IChatPageViewModel
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
                    this.IsEnabled = !string.IsNullOrEmpty(this._message);
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
        private bool _isEnabled;

        public bool IsEnabled
        {
            get { return this._isEnabled; }
            set
            {
                if (this._isEnabled != value)
                    SetProperty(ref this._isEnabled, value);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ChatPageViewModel()
        {
            this.BackCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Conversation", null));
            this.Conversation = new ObservableCollection<Message>();
            this.SendCommand = new DelegateCommand(SendCommandExec);
           
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
            if (!RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
            {
                DisplayMessage("Connexion unavailable");
                return;
            }
            this._actualConversation = navigationParameter as Conversation;
            this.ConversationName = this._actualConversation.Name;
            var conversationMessages = this._actualConversation.Messages;
            InitConversation(conversationMessages);
        }

        private void SendCommandExec()
        {
            var newMessage = new Message
            {
                User = RestApiManagerBase.Instance.UserData.User,
                Date = DateTime.Now,
                ReceivedMessage = this.Message
            };
            newMessage.SetProperties();
            this.Conversation.Add(newMessage);
            if (!RestApiManagerBase.Instance.NotificationRecieiver.SendMessage(this.Message, this._actualConversation.UserList))
                DisplayMessage("This message wasn't sent");
            this.Message = string.Empty;
        }

        private async void DisplayMessage(string message)
        {
            var messageDialog = new MessageDialog(message);

            await messageDialog.ShowAsync();
        }
    }
}
