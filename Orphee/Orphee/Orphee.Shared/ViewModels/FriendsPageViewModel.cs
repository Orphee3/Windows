using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class FriendsPageViewModel : ViewModel, IFriendsPageViewModel
    {
        public ObservableCollection<JToken> FriendNameList { get; set; }
        public DelegateCommand LoginButton { get; private set; }
        public DelegateCommand RegisterButton { get; }
        public string DisconnectedMessage { get; }
        public Visibility ButtonsVisibility { get; private set; }
        public Visibility ListViewVisibility { get; private set; }

        public FriendsPageViewModel()
        {
            this.DisconnectedMessage = "To access your friend list info you have \nto login or to create an account";
            this.ButtonsVisibility = Visibility.Visible;
            this.ListViewVisibility = Visibility.Collapsed;
            this.FriendNameList = new ObservableCollection<JToken>();
            if (RestApiManagerBase.Instance.IsConnected)
            {
                this.ButtonsVisibility = Visibility.Collapsed;
                this.ListViewVisibility = Visibility.Visible;
                foreach (var friend in RestApiManagerBase.Instance.UserData.User.Friends)
                    this.FriendNameList.Add(friend);
            }
            this.LoginButton = new DelegateCommand(() => App.MyNavigationService.Navigate("Login", null));
            this.RegisterButton = new DelegateCommand(() => App.MyNavigationService.Navigate("Register", null));
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (RestApiManagerBase.Instance.IsConnected && this.ButtonsVisibility == Visibility.Visible)
            {
                this.ButtonsVisibility = Visibility.Collapsed;
                this.ListViewVisibility = Visibility.Visible;
                foreach (var friend in RestApiManagerBase.Instance.UserData.User.Friends)
                    this.FriendNameList.Add(friend);
            }
        }
    }
}
