using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement;
using Orphee.RestApiManagement.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class ChannelInfoPageViewModel : ViewModel, IChannelInfoPageViewModel
    {
        public ObservableCollection<Creation> CreationList { get; private set; }
        public DelegateCommand BackCommand { get; private set; }
        private int _totalLikeNumber;

        public int TotalLikeNumber
        {
            get { return this._totalLikeNumber; }
            set
            {
                if (this._totalLikeNumber != value)
                    SetProperty(ref this._totalLikeNumber, value);
            }
        }
        private int _totalCommentNumber;
        public int TotalCommentNumber
        {
            get { return this._totalCommentNumber; }
            set
            {
                if (this._totalCommentNumber != value)
                    SetProperty(ref this._totalCommentNumber, value);
            }
        }
        private string _userName;
        public string UserName
        {
            get { return this._userName; }
            set
            {
                if (this._userName != value)
                    SetProperty(ref this._userName, value);
            }
        }

        private string _userPictureSource;
        public string UserPictureSource
        {
            get { return this._userPictureSource; }
            set
            {
                if (this._userPictureSource != value)
                    SetProperty(ref this._userPictureSource, value);
            }
        }

        private readonly IUserCreationGetter _userCreationGetter;

        public ChannelInfoPageViewModel(IUserCreationGetter userCreationGetter)
        {
            this._userCreationGetter = userCreationGetter;
            this.CreationList = new ObservableCollection<Creation>();
            this.BackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
        }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            this.CreationList.Clear();
            this.TotalCommentNumber = 0;
            this.TotalLikeNumber = 0;
            var user = navigationParameter as User;
            if (user == null)
            {
                this.UserName = "User";
                return;
            }
            this.UserName = user.Name;
            //this.TotalLikeNumber = user.Likes.Count;
            var creations = await this._userCreationGetter.GetUserCreations(user.Id);
            this.TotalCommentNumber = creations?.Count ?? 0;
            SetUserPicture(user.Picture);
            if (creations == null)
                return;
            foreach (var creation in creations)
            {
                creation.Name = creation.Name.Split('.')[0];
                this.CreationList.Add(creation);
            }
        }

        private void SetUserPicture(string pictureUri)
        {
            if (string.IsNullOrEmpty(pictureUri))
                this.UserPictureSource = "/Assets/defaultUser.png";
            else
                this.UserPictureSource = pictureUri;
        }
    }
}
