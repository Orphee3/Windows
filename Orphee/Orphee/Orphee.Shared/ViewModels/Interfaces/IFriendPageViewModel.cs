using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement.Models;

namespace Orphee.ViewModels.Interfaces
{
    /// <summary>
    /// FriendPageViewModel interface
    /// </summary>
    public interface IFriendPageViewModel
    {
        /// <summary>List of the user's friends </summary>
        ObservableCollection<User> FriendList { get; }
        /// <summary>Redirects to the precious page </summary>
        DelegateCommand GoBackCommand { get; }
        /// <summary>Deletes the selected friend </summary>
        DelegateCommand<User> DeleteFriendCommand { get; }
        /// <summary>Validates the creation of the new conversation </summary>
        DelegateCommand ValidateConversationCreationCommand { get; }
        /// <summary>Visible if the box is visible. Hidden otherwise </summary>
        Visibility CheckBoxVisibility { get; set; }
        /// <summary>Visible if the stackPanel is visible. Hidden otherwise </summary>
        Visibility InvitationStackPanelVisibility { get; set; }
        /// <summary>Name of the conversation to be created </summary>
        string ConversationName { get; set; }
        /// <summary>User picture source </summary>
        string UserPictureSource { get; set; }

        /// <summary>
        /// Called when navigated to
        /// </summary>
        /// <param name="navigationParameter"></param>
        /// <param name="navigationMode"></param>
        /// <param name="viewModelState"></param>
        void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState);
    }
}
