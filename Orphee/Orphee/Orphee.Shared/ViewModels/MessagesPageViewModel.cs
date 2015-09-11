using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using MidiDotNet.ExportModule;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class MessagesPageViewModel : ViewModel, IMessagesPageViewModel
    {
        public ObservableCollection<Conversation> ConversationList { get; set; }
        public DelegateCommand CreateNewConversationCommand { get; private set; }
        public DelegateCommand LoginButton { get; private set; }
        public string DisconnectedMessage { get; private set; }
        public Visibility ButtonsVisibility { get; private set; }
        public Visibility ListViewVisibility { get; private set; }

        public MessagesPageViewModel()
        {
            this.DisconnectedMessage = "To access the message functionnality you have \nto login or to create an account";
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

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            App.MyNavigationService.CurrentPageName = "Messages";
            if (RestApiManagerBase.Instance.IsConnected)
            {
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
            this.ConversationList.Add(new Conversation() {Name = channelName, UserList = conversation.UserList});
        }
    }
}
