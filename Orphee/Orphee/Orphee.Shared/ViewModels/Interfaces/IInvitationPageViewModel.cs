using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement.Models;

namespace Orphee.ViewModels.Interfaces
{
    /// <summary>
    /// InvitationPageViewModel interface
    /// </summary>
    public interface IInvitationPageViewModel
    {
        /// <summary>Redirects to the previous page </summary>
        DelegateCommand GoBackCommand { get; }
        /// <summary>Accept the friend invitation </summary>
        DelegateCommand<UserBase> AcceptCommand { get; }
        /// <summary>Refuses the friend invitation</summary>
        DelegateCommand<UserBase> CancelCommand { get; }
        /// <summary>List of the pending invitations</summary>
        ObservableCollection<UserBase> InvitationList { get; }

        /// <summary>
        /// Called when navigated to
        /// </summary>
        /// <param name="navigationParameter"></param>
        /// <param name="navigationMode"></param>
        /// <param name="viewModelState"></param>
        void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState);
    }
}
