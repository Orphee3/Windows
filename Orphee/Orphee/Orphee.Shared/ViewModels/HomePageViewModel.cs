using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class HomePageViewModel : ViewModel, IHomePageViewModel
    {
        public ObservableCollection<Creation> FlowList { get; set; }
        private List<News> _friendNewsList;
        private List<Creation> _popularCreationList; 
        private readonly IUserNewsGetter _userFluxGetter;
        private readonly IPopularCreationGetter _popularCreationGetter;
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

        public HomePageViewModel(IUserNewsGetter userFluxGetter, IPopularCreationGetter popularCreationGetter)
        {
            this._userFluxGetter = userFluxGetter;
            this._popularCreationGetter = popularCreationGetter;
            this.FlowList = new ObservableCollection<Creation>();
            this._popularCreationList = new List<Creation>();
            this._friendNewsList = new List<News>();
            this.NewFriendsCreationsTitleTextBoxForegroundColor = new SolidColorBrush(Colors.White);
            this.PopularCreationsTitleTextBoxForegroundColor = new SolidColorBrush(Color.FromArgb(100, 13, 71, 161));
            this.SearchBoxVisibility = Visibility.Collapsed;
            FillFlowListWithPopularCreations();
        }

        public async void FillFlowListWithNewFriendCreations()
        {
            if (RestApiManagerBase.Instance.IsConnected && this._friendNewsList.Count == 0)
            {
                this._friendNewsList = await this._userFluxGetter.GetUserNews();
                for (var i = 0; i < 12; i++)
                    this.FlowList.Add(new Creation {Name = "Friend Boucle " + i});
                SetTitleTexBoxForegroundColor(false);
            }
        }

        public async void FillFlowListWithPopularCreations()
        {
            var popularCreation = await this._popularCreationGetter.GetpopularCreation();
            foreach (var creation in popularCreation)
            {
                creation.Name = creation.Name.Split('.')[0];
                creation.CreatorList = new List<User> {creation.Creator[0].ToObject<User>()};
                this.FlowList.Add(creation);
            }
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
