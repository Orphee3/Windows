﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    public class FriendPageViewModel : ViewModel, IFriendPageViewModel
    {
        /// <summary>Redirects to the precious page </summary>
        public DelegateCommand GoBackCommand { get; private set; }
        /// <summary>Deletes the selected friend </summary>
        public DelegateCommand<User> DeleteFriendCommand { get; private set; }
        /// <summary>List of the user's friends </summary>
        public ObservableCollection<User> FriendList { get; private set; }
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
        private readonly IGetter _getter;
        /// <summary>Name of the conversation to be created </summary>
        public string ConversationName { get; set; }
        /// <summary>User picture source </summary>
        public string UserPictureSource { get; set; }

        /// <summary>
        /// Constructor initializing getter
        /// through dependency injection
        /// </summary>
        /// <param name="getter">Manages the sending of the "Get" requests</param>
        public FriendPageViewModel(IGetter getter)
        {
            this._getter = getter;
            this.GoBackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
            this.DeleteFriendCommand = new DelegateCommand<User>(user => this.FriendList.Remove(user));
            this.ValidateConversationCreationCommand = new DelegateCommand(() =>
            {
                var conversation  = new Conversation { UserList = this.FriendList.Where(f => f.IsChecked).ToList(), Name = ConversationName };
                App.MyNavigationService.Navigate("Conversation", conversation);
            });
            this.FriendList = new ObservableCollection<User>();
        }

        /// <summary>
        /// Called when navigated to
        /// </summary>
        /// <param name="navigationParameter"></param>
        /// <param name="navigationMode"></param>
        /// <param name="viewModelState"></param>
        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            this.FriendList.Clear();
            this.CheckBoxVisibility = navigationParameter != null ? Visibility.Visible : Visibility.Collapsed;
            this.InvitationStackPanelVisibility = navigationParameter != null ? Visibility.Collapsed : Visibility.Visible;
            var friendList = (await this._getter.GetInfo<List<User>>(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + RestApiManagerBase.Instance.UserData.User.Id + "/friends")).OrderBy(f => f.Name);
            foreach (var friend in friendList)
            {
                friend.Picture = friend.Picture ?? "/Assets/defaultUser.png";
                this.FriendList.Add(friend);
            }
        }
    }
}
