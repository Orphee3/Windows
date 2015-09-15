using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Orphee.RestApiManagement.Models;

namespace Orphee.ViewModels.Interfaces
{
    public interface IHomePageViewModel
    {
        ObservableCollection<Creation> FlowList { get; set; }
        void FillFlowListWithPopularCreations();
        void FillFlowListWithNewFriendCreations();
        SolidColorBrush PopularCreationsTitleTextBoxForegroundColor { get; }
        SolidColorBrush NewFriendsCreationsTitleTextBoxForegroundColor { get; }
        Visibility SearchBoxVisibility { get; }
    }
}
