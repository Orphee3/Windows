using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Practices.Prism.Commands;

namespace Orphee.ViewModels.Interfaces
{
    public interface IProfilePageViewModel
    {
        string UserName { get; }
        int NumberOfCreations { get; }
        int NumberOfComments { get; }
        int NumberOfFollows { get; }
        int NumberOfFollowers { get; }
        string UserPictureSource { get; }
        SolidColorBrush BackgroundPictureColor { get; }
        Visibility DisconnectedStackPanelVisibility { get; set; }
        Visibility ConnectedStackPanelVisibility { get; set; }
        DelegateCommand LoginCommand { get; }
        DelegateCommand LogoutCommand { get; }
        DelegateCommand FriendPageCommand { get; }
    }
}
