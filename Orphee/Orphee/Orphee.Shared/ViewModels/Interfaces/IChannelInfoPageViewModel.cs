using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement;

namespace Orphee.ViewModels.Interfaces
{
    public interface IChannelInfoPageViewModel
    {
        ObservableCollection<Creation> CreationList { get; }
        DelegateCommand BackCommand { get; }
        string UserName { get; set; }
        int TotalLikeNumber { get; set; }
        int TotalCommentNumber { get; set; }
        string UserPictureSource { get; set; }
    }
}
