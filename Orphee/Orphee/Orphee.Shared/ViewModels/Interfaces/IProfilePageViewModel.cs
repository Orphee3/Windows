using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;

namespace Orphee.ViewModels.Interfaces
{
    /// <summary>
    /// ProfilePageViewModel interface
    /// </summary>
    public interface IProfilePageViewModel
    {
        /// <summary>Logged user name</summary>
        string UserName { get; }

        /// <summary>Number of creation of the logged user </summary>
        int NumberOfCreations { get; }

        /// <summary>Number of comments of the logged user </summary>
        int NumberOfComments { get; }

        /// <summary>Number of follows of the logged user </summary>
        int NumberOfFollows { get; }

        /// <summary>Number of followers of the logged user</summary>
        int NumberOfFollowers { get; }

        /// <summary>User picture source</summary>
        string UserPictureSource { get; }

        /// <summary>Background color of the profile page </summary>
        SolidColorBrush BackgroundPictureColor { get; }

        /// <summary>Visible if the user is disconnected. Hidden otherwise </summary>
        Visibility DisconnectedStackPanelVisibility { get; set; }

        /// <summary>Visible if the user is connected. Hidden otherwise </summary>
        Visibility ConnectedStackPanelVisibility { get; set; }

        /// <summary>Redirects to MyCreationPage</summary>
        DelegateCommand MyCreationsCommand { get; }

        /// <summary>Logs the user out</summary>
        DelegateCommand NotificationsCommand { get; }

        /// <summary>Redirects to LoginPage</summary>
        DelegateCommand LoginCommand { get; }

        /// <summary>Redirects to FriendPage</summary>
        DelegateCommand FriendPageCommand { get; }

        /// <summary>Redirects to EditProfilePage</summary>
        DelegateCommand ParametersCommand { get; }

        /// <summary>
        /// Updates the picture source
        /// </summary>
        void UpdatePictureSource();

        /// <summary>
        /// Called when navigated to this page
        /// </summary>
        /// <param name="navigationParameter"></param>
        /// <param name="navigationMode"></param>
        /// <param name="viewModelState"></param>
        void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState);
    }
}
