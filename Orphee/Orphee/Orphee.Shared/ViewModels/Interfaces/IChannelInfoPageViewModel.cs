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
        UserBase Creator { get; }
    }
}
