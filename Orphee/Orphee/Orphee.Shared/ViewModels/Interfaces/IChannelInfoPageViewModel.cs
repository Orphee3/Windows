using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement.Models;

namespace Orphee.ViewModels.Interfaces
{
    /// <summary>
    /// ChannelInfoPangeViewModel interface
    /// </summary>
    public interface IChannelInfoPageViewModel
    {
        /// <summary>List of the user's creation </summary>
        ObservableCollection<Creation> CreationList { get; }
        /// <summary>Redirects to the previous page </summary>
        DelegateCommand BackCommand { get; }
        /// <summary>Name of the user </summary>
        User Creator { get; }
        /// <summary>Number of like </summary>
        int LikeNumber { get; set; }
        /// <summary>Number of creations </summary>
        int CreationNumber { get; set; }
        /// <summary>User picture source </summary>
        string UserPictureSource { get; set; }
    }
}
