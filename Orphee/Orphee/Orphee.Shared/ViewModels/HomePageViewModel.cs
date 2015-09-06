using System.Collections.ObjectModel;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class HomePageViewModel : ViewModel, IHomePageViewModel
    {
        public ObservableCollection<string> FlowList { get; set; }
        private readonly INewsFlowGetter _newsFlowGetter;
        public SolidColorBrush PopularCreationsTitleTextBoxForegroundColor { get; set; }
        public SolidColorBrush NewFriendsCreationsTitleTextBoxForegroundColor { get; set; }

        public HomePageViewModel(INewsFlowGetter newsFlowGetter)
        {
            this._newsFlowGetter = newsFlowGetter;
            this.FlowList = new ObservableCollection<string>();
            this.NewFriendsCreationsTitleTextBoxForegroundColor = new SolidColorBrush(Colors.White);
            this.PopularCreationsTitleTextBoxForegroundColor = new SolidColorBrush(Color.FromArgb(100, 13, 71, 161));
            FillFlowListWithPopularCreations();
        }

        public void FillFlowListWithNewFriendCreations()
        {
            this.FlowList.Clear();
            for (var i = 0; i < 12; i++)
                this.FlowList.Add("Friend Boucle " + i);
            SetTitleTexBoxForegroundColor(false);
        }

        public void FillFlowListWithPopularCreations()
        {
            if (this.FlowList.Count > 0)
                this.FlowList.Clear();
            for (var i = 0; i < 12; i++)
                this.FlowList.Add("Popular Boucle " + i);
            SetTitleTexBoxForegroundColor(true);
        }

        private void SetTitleTexBoxForegroundColor(bool option)
        {
            if (option)
            {
                this.PopularCreationsTitleTextBoxForegroundColor.Color = Color.FromArgb(100, 13, 71, 161);
                this.NewFriendsCreationsTitleTextBoxForegroundColor.Color = Colors.White;
            }
            else
            {
                this.PopularCreationsTitleTextBoxForegroundColor.Color = Colors.White;
                this.NewFriendsCreationsTitleTextBoxForegroundColor.Color = Color.FromArgb(100, 13, 71, 161);
            }
        }
    }
}
