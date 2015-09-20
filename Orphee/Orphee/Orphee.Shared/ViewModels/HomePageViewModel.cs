using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class HomePageViewModel : ViewModel, IHomePageViewModel
    {
        public ObservableCollection<Creation> FlowList { get; set; }
        private List<News> _friendNewsList;
        private readonly List<Creation> _popularCreationList;
        private IGetter _getter;
        private readonly IUserNewsGetter _userFluxGetter;
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

        public HomePageViewModel(IUserNewsGetter userFluxGetter, IGetter getter)
        {
            this._getter = getter;
            this._userFluxGetter = userFluxGetter;
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
            if (RestApiManagerBase.Instance.IsConnected)
            {
                this.FlowList.Clear();
                this._friendNewsList = await this._getter.GetInfo<List<News>>(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + RestApiManagerBase.Instance.UserData.User.Id + "/news");
                foreach (var news in this._friendNewsList)
                {
                    var creation = new Creation();
                    creation.CreatorList.Add(new User());
                    //this.FlowList.Add(new Creation {Name = "Friend Boucle " + i});
                }
                SetTitleTexBoxForegroundColor(false);
            }
        }

        public async void FillFlowListWithPopularCreations()
        {
            if (this._popularCreationList.Count == 0)
            {
                this.FlowList.Clear();
                var popularCreation = await this._getter.GetInfo<List<Creation>>(RestApiManagerBase.Instance.RestApiPath["popular"]);
                foreach (var creation in popularCreation)
                {
                    creation.NumberOfComment = creation.Comments?.Count ?? 0;
                    creation.NumberOfLike = 0;
                    creation.Name = creation.Name.Split('.')[0];
                    creation.CreatorList = new List<User> {creation.Creator[0].ToObject<User>()};
                    this.FlowList.Add(creation);
                }
                SetTitleTexBoxForegroundColor(true);
            }
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
