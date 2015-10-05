using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class ChannelInfoPageViewModel : ViewModel, IChannelInfoPageViewModel
    {
        public ObservableCollection<Creation> CreationList { get; private set; }
        public DelegateCommand BackCommand { get; private set; }
        private int _creationNumber;
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
        public int CreationNumber
        {
            get { return this._creationNumber; }
            set
            {
                if (this._creationNumber != value)
                    SetProperty(ref this._creationNumber, value);
            }
        }
        private int _likeNumber;
        public int LikeNumber
        {
            get { return this._likeNumber; }
            set
            {
                if (this._likeNumber != value)
                    SetProperty(ref this._likeNumber, value);
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

        private readonly IGetter _getter;

        public ChannelInfoPageViewModel(IGetter getter)
        {
            this._getter = getter;
            this.CreationList = new ObservableCollection<Creation>();
            this.BackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
        }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            this.CreationList.Clear();

            var user = navigationParameter as User;
            this._userName = user.Name;
            this.LikeNumber = user.Likes?.Count ?? 0;
            var creations = await this._getter.GetInfo<List<Creation>>(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + user.Id + "/creation");
            this.CreationNumber = creations?.Count ?? 0;
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
            this.UserPictureSource = string.IsNullOrEmpty(pictureUri) ? "/Assets/defaultUser.png" : pictureUri;
        }
    }
}
