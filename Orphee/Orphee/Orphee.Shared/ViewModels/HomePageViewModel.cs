using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml;
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
        private Visibility _searchBoxVisibility;
        public Visibility SearchBoxVisibility
        {
            get { return this._searchBoxVisibility; }
            set
            {
                if (this._searchBoxVisibility != value)
                {
                    this._searchBoxVisibility = value;
                    OnPropertyChanged(nameof(this.SearchBoxVisibility));
                }
            }
        }

        public HomePageViewModel(INewsFlowGetter newsFlowGetter)
        {
            this._newsFlowGetter = newsFlowGetter;
            this.FlowList = new ObservableCollection<string>();
            this.NewFriendsCreationsTitleTextBoxForegroundColor = new SolidColorBrush(Colors.White);
            this.PopularCreationsTitleTextBoxForegroundColor = new SolidColorBrush(Color.FromArgb(100, 13, 71, 161));
            this.SearchBoxVisibility = Visibility.Collapsed;
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
                this.SearchBoxVisibility = Visibility.Collapsed;
            }
            else
            {
                this.PopularCreationsTitleTextBoxForegroundColor.Color = Colors.White;
                this.NewFriendsCreationsTitleTextBoxForegroundColor.Color = Color.FromArgb(100, 13, 71, 161);
                this.SearchBoxVisibility = Visibility.Visible;
            }
        }
    }
}
