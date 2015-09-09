using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement;

namespace Orphee.ViewModels.Interfaces
{
    public interface ISocialPageViewModel
    {
        ObservableCollection<User> UserNameList { get; set; }
        DelegateCommand LoginCommand { get; }
        DelegateCommand<User> NewFriendCommand { get; }
        Visibility ButtonsVisibility { get; }
        Visibility ListViewVisibility { get; }
    }
}
