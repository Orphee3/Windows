using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement;
using Orphee.RestApiManagement.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class NewConversationConfigPageViewModel : ViewModel, INewConversationConfigPageViewModel
    {
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand CreateConversationCommand { get; private set; }
        public ObservableCollection<User> FriendList { get; private set; }
        private readonly IUserFriendListGetter _userFriendListGetter;

        public NewConversationConfigPageViewModel(IUserFriendListGetter userFriendListGetter)
        {
            this._userFriendListGetter = userFriendListGetter;
            this.CreateConversationCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Friend", this.FriendList.Where(f => f.IsChecked)));
            this.GoBackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
        }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
            {
                var userFriends = await this._userFriendListGetter.GetUserFriendList();
                foreach (var user in userFriends)
                    this.FriendList.Add(user);
            }
        }
    }
}
