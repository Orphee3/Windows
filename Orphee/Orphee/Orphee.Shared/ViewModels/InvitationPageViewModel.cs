using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement;
using Orphee.RestApiManagement.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class InvitationPageViewModel : ViewModel, IInvitationPageViewModel
    {
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand<User> AcceptCommand { get; private set; }
        public DelegateCommand<User> CancelCommand { get; private set; }
        public ObservableCollection<User> InvitationList { get; private set; }
        private readonly IFriendAccepter _friendAccepter;

        public InvitationPageViewModel(IFriendAccepter friendAccepter)
        {
            this._friendAccepter = friendAccepter;
            this.AcceptCommand = new DelegateCommand<User>(AcceptCommandExec);
            this.CancelCommand = new DelegateCommand<User>(CancelCommandExec);
            this.GoBackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
            this.InvitationList = new ObservableCollection<User>();
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            foreach (var pendingInvitation in RestApiManagerBase.Instance.UserData.User.PendingFriendList)
                this.InvitationList.Add(pendingInvitation);
        }

        private void AcceptCommandExec(User user)
        {
            this.InvitationList.Remove(user);
            RestApiManagerBase.Instance.UserData.User.PendingFriendList.Remove(user);
            this._friendAccepter.AcceptFriend(user.Id);
        }

        private void CancelCommandExec(User user)
        {
            this.InvitationList.Remove(user);
            RestApiManagerBase.Instance.UserData.User.PendingFriendList.Remove(user);
        }
    }
}
