using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement.Models;

namespace Orphee.ViewModels.Interfaces
{
    /// <summary>
    /// NewConversationConfigPageViewModel interface
    /// </summary>
    public interface INewConversationConfigPageViewModel
    {
        /// <summary>Redirected to the previous page </summary>
        DelegateCommand GoBackCommand { get; }
        /// <summary>Redirects to the FriendPage in order to select the users to include in the new conversation </summary>
        DelegateCommand CreateConversationCommand { get; }
        /// <summary>User's friend list </summary>
        ObservableCollection<User> FriendList { get; }

        /// <summary>
        /// Called when navigated to this page
        /// </summary>
        /// <param name="navigationParameter"></param>
        /// <param name="navigationMode"></param>
        /// <param name="viewModelState"></param>
        void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState);
    }
}
