using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement;

namespace Orphee.ViewModels.Interfaces
{
    public interface ISocialPageViewModel
    {
        ObservableCollection<User> FriendNameList { get; set; }
        DelegateCommand LoginButton { get; }
        DelegateCommand<User> NewFriendCommand { get; }
        DelegateCommand RegisterButton { get; }
        string DisconnectedMessage { get; }
        Visibility ButtonsVisibility { get; }
        Visibility ListViewVisibility { get; }
    }
}
