﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    /// <summary>
    /// FriendPage view model
    /// </summary>
    public class FriendPageViewModel : ViewModelExtend, IFriendPageViewModel
    {
        /// <summary>Redirects to the precious page </summary>
        public DelegateCommand GoBackCommand { get; private set; }
        /// <summary>Deletes the selected friend </summary>
        public DelegateCommand<UserBase> DeleteFriendCommand { get; private set; }
        /// <summary>List of the user's friends </summary>
        public ObservableCollection<UserBase> FriendList { get; private set; }
        /// <summary>Validates the creation of the new conversation </summary>
        public DelegateCommand ValidateConversationCreationCommand { get; private set; }
        private Visibility _checkBoxVisibility;
        /// <summary>Visible if the box is visible. Hidden otherwise </summary>
        public Visibility CheckBoxVisibility
        {
            get { return this._checkBoxVisibility; }
            set
            {
                if (this._checkBoxVisibility != value)
                    SetProperty(ref this._checkBoxVisibility, value);
            }
        }
        private Visibility _invitationStackPanelVisibility;
        /// <summary>Visible if the stackPanel is visible. Hidden otherwise </summary>
        public Visibility InvitationStackPanelVisibility
        {
            get { return this._invitationStackPanelVisibility; }
            set
            {
                if (this._invitationStackPanelVisibility != value)
                    SetProperty(ref this._invitationStackPanelVisibility, value);
            }
        }

        private IGetter _getter;

        /// <summary>
        /// Constructor initializing getter
        /// through dependency injection
        /// </summary>
        public FriendPageViewModel(IGetter getter)
        {
            this._getter = getter;
            this.GoBackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
            this.DeleteFriendCommand = new DelegateCommand<UserBase>(RemoveFriendship);
            this.ValidateConversationCreationCommand = new DelegateCommand(ValidateConversationCommandExec);
            SetProgressRingVisibility(true);
            this.FriendList = new ObservableCollection<UserBase>();
        }

        /// <summary>
        /// Called when navigated to
        /// </summary>
        /// <param name="navigationParameter"></param>
        /// <param name="navigationMode"></param>
        /// <param name="viewModelState"></param>
        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            App.InternetAvailabilityWatcher.PropertyChanged += InternetAvailabilityWatcherOnPropertyChanged;
            this.CheckBoxVisibility = navigationParameter != null ? Visibility.Visible : Visibility.Collapsed;
            this.InvitationStackPanelVisibility = navigationParameter != null ? Visibility.Collapsed : Visibility.Visible;
            if (!VerifyReturnedValue(RestApiManagerBase.Instance.UserData.User.FriendList, ""))
                return;
            foreach (var friend in RestApiManagerBase.Instance.UserData.User.FriendList.Where(friend => this.FriendList.All(f => f.Id != friend.Id)))
                this.FriendList.Add(friend);
        }

        private async void ValidateConversationCommandExec()
        {
            var friendList = this.FriendList.Where(f => f.IsChecked).ToList();
            if (friendList == null || friendList.Count == 0 || CheckForExistingConversation())
                return;
            var conversation = new Conversation { UserList = friendList, IsNew = true, Name = GenerateConversationName(friendList)};
            conversation.IsPrivate = friendList.Count <= 1;
            if (friendList.Count > 1)
            {
                RestApiManagerBase.Instance.UserData.User.ConversationList.Add(conversation);
                var result = await App.InternetAvailabilityWatcher.SocketManager.SocketEmitter.CreateGroupChat(friendList.Select(u => u.Id).ToList());
            }
            App.MyNavigationService.Navigate("Chat", JsonConvert.SerializeObject(conversation));
        }

        private async void RemoveFriendship(UserBase user)
        {
            if (App.InternetAvailabilityWatcher.IsInternetUp && RestApiManagerBase.Instance.IsConnected)
            {
                var result = await this._getter.GetInfo<string>(RestApiManagerBase.Instance.RestApiPath["remove friend"] + user.Id);
                if (string.IsNullOrEmpty(result))
                    return;
                this.FriendList.Remove(user);
                RestApiManagerBase.Instance.UserData.User.FriendList.Remove(RestApiManagerBase.Instance.UserData.User.FriendList.FirstOrDefault(u => u.Id == user.Id));
            }
        }

        private bool CheckForExistingConversation()
        {
            var friendList = this.FriendList.Where(f => f.IsChecked).ToList();
            foreach (var conversation in from conversation in RestApiManagerBase.Instance.UserData.User.ConversationList let matchedUser = conversation.UserList.Count(user => friendList.Any(u => u.Id == user.Id)) where matchedUser == friendList.Count select conversation)
            {
                App.MyNavigationService.Navigate("Chat", JsonConvert.SerializeObject(conversation));
                return true;
            }
            return false;
        }

        private string GenerateConversationName(List<UserBase> friendList)
        {
            var conversationName = "";
            foreach (var user in friendList)
            {
                conversationName += user.Name;
                if (user != friendList.Last())
                    conversationName += ", ";
            }
            return conversationName;
        }
    }
}
