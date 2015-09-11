using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement;

namespace Orphee.ViewModels.Interfaces
{
    public interface IHomePageViewModel
    {
        ObservableCollection<User> FlowList { get; set; }
        void FillFlowListWithPopularCreations();
        void FillFlowListWithNewFriendCreations();
        SolidColorBrush PopularCreationsTitleTextBoxForegroundColor { get; }
        SolidColorBrush NewFriendsCreationsTitleTextBoxForegroundColor { get; }
        Visibility SearchBoxVisibility { get; }
    }
}
