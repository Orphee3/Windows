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

        public int CreationNumber
        {
            get { return this._creationNumber; }
            set
            {
                if (this._creationNumber != value)
                    SetProperty(ref this._creationNumber, value);
            }
        }
        private int _friendNumber;
        public int FriendNumber
        {
            get { return this._friendNumber; }
            set
            {
                if (this._friendNumber != value)
                    SetProperty(ref this._friendNumber, value);
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
            this.FriendNumber = 0;
            this.CreationNumber = 0;
            var user = navigationParameter as User;
            if (user == null)
            {
                this.UserName = "User";
                return;
            }
            this.UserName = user.Name;
            this.FriendNumber = user.Friends?.Count ?? 0;
            var creations = await this._userCreationGetter.GetUserCreations(user.Id);
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
