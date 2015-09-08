using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement;

namespace Orphee.ViewModels.Interfaces
{
    public interface IFriendPageViewModel
    {
        ObservableCollection<User> FriendList { get; }
        DelegateCommand GoBackCommand { get; }
        DelegateCommand<User> DeleteFriendCommand { get; }
    }
}
