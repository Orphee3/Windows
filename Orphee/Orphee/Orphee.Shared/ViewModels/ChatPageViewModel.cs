using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.Models;
using Orphee.RestApiManagement;
using Orphee.RestApiManagement.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class ChatPageViewModel : ViewModel, IChatPageViewModel
    {
        public DelegateCommand BackCommand { get; private set; }
        public DelegateCommand SendCommand { get; private set; }
        private string _message;
        public string Message
        {
            get { return this._message; }
            set
            {
                if (this._message != value)
                    SetProperty(ref this._message, value);
            }
        }
        public ObservableCollection<MyDictionary> Conversation { get; private set; }
        private string _conversationName;

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

        public ChatPageViewModel()
        {
            this.BackCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Messages", null));
            this.Conversation = new ObservableCollection<MyDictionary>();
            this.SendCommand = new DelegateCommand(SendCommandExec);
           
        }

        public void InitConversation(List<Message> messages)
        {
            foreach (var message in messages)
                this.Conversation.Add(new MyDictionary(message.User.Id == RestApiManagerBase.Instance.UserData.User.Id ? Visibility.Visible: Visibility.Collapsed,message.User.Id == RestApiManagerBase.Instance.UserData.User.Id ? Visibility.Collapsed : Visibility.Visible, message.ReceivedMessage,  message.Date));
        }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            this._actualConversation = navigationParameter as Conversation;
            this.ConversationName = this._actualConversation.Name;
            var conversationMessages = this._actualConversation.Messages;
            InitConversation(conversationMessages);
        }

        private void SendCommandExec()
        {
            if (this.Message.Any())
            {
                this.Conversation.Add(new MyDictionary(Visibility.Visible, Visibility.Collapsed, this.Message, DateTime.Now));
                RestApiManagerBase.Instance.NotificationRecieiver.SendMessage(this.Message, this._actualConversation.UserList);
                this.Message = String.Empty;
            }
        }
    }
}
