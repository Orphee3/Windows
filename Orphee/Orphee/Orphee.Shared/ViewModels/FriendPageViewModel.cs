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
    /// FriendPage view model
    /// </summary>
    public class FriendPageViewModel : ViewModel, IFriendPageViewModel, ILoadingScreenComponents
    {
        /// <summary>Redirects to the precious page </summary>
        public DelegateCommand GoBackCommand { get; private set; }
        /// <summary>Deletes the selected friend </summary>
        public DelegateCommand<User> DeleteFriendCommand { get; private set; }
        /// <summary>List of the user's friends </summary>
        public ObservableCollection<User> FriendList { get; private set; }
        /// <summary>Validates the creation of the new conversation </summary>
        public DelegateCommand ValidateConversationCreationCommand { get; private set; }
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
        /// <summary>Name of the conversation to be created </summary>
        public string ConversationName { get; set; }
        /// <summary>User picture source </summary>
        public string UserPictureSource { get; set; }

        /// <summary>
        /// Constructor initializing getter
        /// through dependency injection
        /// </summary>
        public FriendPageViewModel()
        {
            this.GoBackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
            this.DeleteFriendCommand = new DelegateCommand<User>(user => this.FriendList.Remove(user));
            this.ValidateConversationCreationCommand = new DelegateCommand(ValidateConversationCommandExec);
            this.ProgressRingVisibility = Visibility.Visible;
            this.IsProgressRingActive = true;
            this.FriendList = new ObservableCollection<User>();
        }

        /// <summary>
        /// Called when navigated to
        /// </summary>
        /// <param name="navigationParameter"></param>
        /// <param name="navigationMode"></param>
        /// <param name="viewModelState"></param>
        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            this.CheckBoxVisibility = navigationParameter != null ? Visibility.Visible : Visibility.Collapsed;
            this.InvitationStackPanelVisibility = navigationParameter != null ? Visibility.Collapsed : Visibility.Visible;
            this.FriendList.Clear();
            foreach (var friend in RestApiManagerBase.Instance.UserData.User.FriendList)
                this.FriendList.Add(friend);
            this.IsProgressRingActive = false;
            this.ProgressRingVisibility = Visibility.Collapsed;
        }

        private void ValidateConversationCommandExec()
        {
            if (CheckForExistingConversation())
                return;
            var friendList = this.FriendList.Where(f => f.IsChecked).ToList();
            var conversation = new Conversation { UserList = friendList, Name = GetChannelName(friendList) };
            if (friendList.Count > 1)
            {
                RestApiManagerBase.Instance.NotificationRecieiver.CreateGroupChat(friendList.Select(u => u.Id).ToList());
                RestApiManagerBase.Instance.UserData.User.ConversationList.Add(conversation);
            }
            App.MyNavigationService.Navigate("Chat", conversation);
        }

        private string GetChannelName(List<User> userList)
        {
            var channelName = "";
            foreach (var user in userList)
            {
                channelName += user.Name;
                if (user != userList.Last())
                    channelName += ", ";
            }
            return channelName;
        }

        private bool CheckForExistingConversation()
        {
            var friendList = this.FriendList.Where(f => f.IsChecked).ToList();
            foreach (var conversation in RestApiManagerBase.Instance.UserData.User.ConversationList)
            {
                var matchedUser = 0;
                foreach (var user in conversation.UserList)
                {
                    if (friendList.Any(u => u.Id == user.Id))
                        matchedUser++;
                }
                if (matchedUser == friendList.Count)
                {
                    App.MyNavigationService.Navigate("Chat", conversation);
                    return true;
                }
            }
            return false;
        }
    }
}
