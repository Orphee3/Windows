using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement;
using Orphee.RestApiManagement.Models;

namespace Orphee.ViewModels.Interfaces
{
    public interface IChannelInfoPageViewModel
    {
        ObservableCollection<Creation> CreationList { get; }
        DelegateCommand BackCommand { get; }
        string UserName { get; set; }
        int FriendNumber { get; set; }
        int CreationNumber { get; set; }
        string UserPictureSource { get; set; }
    }
}
