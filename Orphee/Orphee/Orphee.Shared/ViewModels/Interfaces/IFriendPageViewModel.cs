using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement;
using Orphee.RestApiManagement.Annotations;
using Orphee.RestApiManagement.Models;

namespace Orphee.ViewModels.Interfaces
{
    public interface IFriendPageViewModel
    {
        ObservableCollection<User> FriendList { get; }
        DelegateCommand GoBackCommand { get; }
        DelegateCommand<User> DeleteFriendCommand { get; }
        DelegateCommand ValidateConversationCreationCommand { get; }
        Visibility CheckBoxVisibility { get; set; }
        Visibility InvitationStackPanelVisibility { get; set; }
        string ConversationName { get; set; }
        string UserPictureSource { get; set; }
    }
}
