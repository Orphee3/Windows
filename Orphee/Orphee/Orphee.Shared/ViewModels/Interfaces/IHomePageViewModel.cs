using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media;

namespace Orphee.ViewModels.Interfaces
{
    public interface IHomePageViewModel
    {
        ObservableCollection<string> FlowList { get; set; }
        void FillFlowListWithPopularCreations();
        void FillFlowListWithNewFriendCreations();
        SolidColorBrush PopularCreationsTitleTextBoxForegroundColor { get; }
        SolidColorBrush NewFriendsCreationsTitleTextBoxForegroundColor { get; }
    }
}
