using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;
using Orphee.RestApiManagement.Getters.Interfaces;

namespace Orphee.ViewModels
{
    /// <summary>
    /// InvitationPage view model
    /// </summary>
    public class InvitationPageViewModel : ViewModel, IInvitationPageViewModel
    {
        /// <summary>Redirects to the previous page </summary>
        public DelegateCommand GoBackCommand { get; private set; }
        /// <summary>Accept the friend invitation </summary>
        public DelegateCommand<User> AcceptCommand { get; private set; }
        /// <summary>Refuses the friend invitation</summary>
        public DelegateCommand<User> CancelCommand { get; private set; }
        /// <summary>List of the pending invitations</summary>
        public ObservableCollection<User> InvitationList { get; private set; }
        private readonly IGetter _getter;

        /// <summary>
        /// Constructor initializing getter
        /// through dependency injection
        /// </summary>
        /// <param name="getter">Manages the sending of the "Get" requests</param>
        public InvitationPageViewModel(IGetter getter)
        {
            this._getter = getter;
            this.AcceptCommand = new DelegateCommand<User>(AcceptCommandExec);
            this.CancelCommand = new DelegateCommand<User>(CancelCommandExec);
            this.GoBackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
            this.InvitationList = new ObservableCollection<User>();
        }

        /// <summary>
        /// Called when navigated to
        /// </summary>
        /// <param name="navigationParameter"></param>
        /// <param name="navigationMode"></param>
        /// <param name="viewModelState"></param>
        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            foreach (var pendingInvitation in RestApiManagerBase.Instance.UserData.User.PendingFriendList)
                this.InvitationList.Add(pendingInvitation);
        }

        private void AcceptCommandExec(User user)
        {
            this.InvitationList.Remove(user);
            RestApiManagerBase.Instance.UserData.User.PendingFriendList.Remove(user);
            this._getter.GetInfo<string>(RestApiManagerBase.Instance.RestApiPath["acceptfriend"] + "/" + user.Id);
        }

        private void CancelCommandExec(User user)
        {
            this.InvitationList.Remove(user);
            RestApiManagerBase.Instance.UserData.User.PendingFriendList.Remove(user);
        }
    }
}
