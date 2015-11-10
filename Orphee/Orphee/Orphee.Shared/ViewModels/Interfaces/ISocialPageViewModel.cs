using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement.Models;

namespace Orphee.ViewModels.Interfaces
{
    /// <summary>
    /// SocialPageViewModel interface
    /// </summary>
    public interface ISocialPageViewModel
    {
        /// <summary>List of the searched users </summary>
        ObservableCollection<UserBase> UserList { get; set; }
        /// <summary>Login command</summary>
        DelegateCommand LoginCommand { get; }
        /// <summary>Add new friend command</summary>
        DelegateCommand<UserBase> AddFriendCommand { get; }

        /// <summary>
        /// Called when we navigate to this page
        /// </summary>
        /// <param name="navigationParameter"></param>
        /// <param name="navigationMode"></param>
        /// <param name="viewModelState"></param>
        void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState);
    }
}
