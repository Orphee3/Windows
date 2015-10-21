using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    /// <summary>
    /// NewConversationConfigPage view model
    /// </summary>
    //public class NewConversationConfigPageViewModel : ViewModel, INewConversationConfigPageViewModel
    //{
    //    /// <summary>Redirected to the previous page </summary>
    //    public DelegateCommand GoBackCommand { get; private set; }
    //    /// <summary>Redirects to the FriendPage in order to select the users to include in the new conversation </summary>
    //    public DelegateCommand CreateConversationCommand { get; private set; }
    //    /// <summary>User's friend list </summary>
    //    public ObservableCollection<User> FriendList { get; private set; }
    //    private readonly IGetter _getter;

    //    /// <summary>
    //    /// Constructor initializing getter
    //    /// through dependency injection
    //    /// </summary>
    //    /// <param name="getter">Manages the sending of the "Get" requests</param>
    //    public NewConversationConfigPageViewModel(IGetter getter)
    //    {
    //        this._getter = getter;
    //        this.CreateConversationCommand = new DelegateCommand(() => App.MyNavigationService.Navigate("Friend", this.FriendList.Where(f => f.IsChecked)));
    //        this.GoBackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
    //    }

    //    /// <summary>
    //    /// Called when navigated to this page
    //    /// </summary>
    //    /// <param name="navigationParameter"></param>
    //    /// <param name="navigationMode"></param>
    //    /// <param name="viewModelState"></param>
    //    public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
    //    {
    //        if (RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
    //        {
    //            var userFriends = (await this._getter.GetInfo<List<User>>(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + RestApiManagerBase.Instance.UserData.User.Id + "/friends")).OrderBy(f => f.Name);
    //            foreach (var user in userFriends)
    //                this.FriendList.Add(user);
    //        }
    //    }
    //}
}
