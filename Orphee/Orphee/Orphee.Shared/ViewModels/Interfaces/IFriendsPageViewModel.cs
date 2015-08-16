using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement;

namespace Orphee.ViewModels.Interfaces
{
    public interface IFriendsPageViewModel
    {
        ObservableCollection<JToken> FriendNameList { get; set; }
        DelegateCommand LoginButton { get; }
        DelegateCommand RegisterButton { get; }
        string DisconnectedMessage { get; }
        Visibility ButtonsVisibility { get; }
        Visibility ListViewVisibility { get; }
    }
}
