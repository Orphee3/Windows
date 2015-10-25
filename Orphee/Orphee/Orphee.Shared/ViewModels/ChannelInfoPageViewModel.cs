using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    /// <summary>
    /// ChannelInfoPage view model
    /// </summary>
    public class ChannelInfoPageViewModel : ViewModel, IChannelInfoPageViewModel, ILoadingScreenComponents
    {
        /// <summary>List of the user's creation </summary>
        public ObservableCollection<Creation> CreationList { get; private set; }
        /// <summary>Redirects to the previous page </summary>
        public DelegateCommand BackCommand { get; private set; }
        private int _creationNumber;
        private string _userName;
        /// <summary>Name of the user </summary>
        public string UserName
        {
            get { return this._userName; }
            set
            {
                if (this._userName != value)
                    SetProperty(ref this._userName, value);
            }
        }
        /// <summary>Number of creations </summary>
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
        /// /// <summary>Number of like </summary>
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
        /// <summary>User picture source </summary>
        public string UserPictureSource
        {
            get { return this._userPictureSource; }
            set
            {
                if (this._userPictureSource != value)
                    SetProperty(ref this._userPictureSource, value);
            }
        }
        private bool _isProgressRingActive;
        public bool IsProgressRingActive
        {
            get { return this._isProgressRingActive; }
            set
            {
                if (this._isProgressRingActive != value)
                    SetProperty(ref this._isProgressRingActive, value);
            }
        }
        private Visibility _progressRingVisibility;

        public Visibility ProgressRingVisibility
        {
            get { return this._progressRingVisibility; }
            set
            {
                if (this._progressRingVisibility != value)
                    SetProperty(ref this._progressRingVisibility, value);
            }
        }

        private readonly IGetter _getter;

        /// <summary>
        /// Constructor initializing getter
        /// through dependency injection
        /// </summary>
        /// <param name="getter">Manages the sending of the "Get" requests</param>
        public ChannelInfoPageViewModel(IGetter getter)
        {
            this._getter = getter;
            this.CreationList = new ObservableCollection<Creation>();
            this.ProgressRingVisibility = Visibility.Visible;
            this.IsProgressRingActive = true;
            this.BackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
        }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (!RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
            {
                DisplayMessage("Connexion unavailable");
                return;
            }
            this.CreationList.Clear();
            var user = navigationParameter as User;
            this._userName = user.Name;
            //this.LikeNumber = user.Likes?.Count ?? 0;
            List<Creation> creations;
            try
            {
                creations = await this._getter.GetInfo<List<Creation>>(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + user.Id + "/creation");
            }
            catch (Exception)
            {
               DisplayMessage("Request failed");
                return;
            }
            // this.CreationNumber = creations?.Count ?? 0;
            SetUserPicture(user.Picture);
            if (creations == null)
                return;
            foreach (var creation in creations)
            {
                creation.Name = creation.Name.Split('.')[0];
                this.CreationList.Add(creation);
            }
            this.IsProgressRingActive = false;
            this.ProgressRingVisibility = Visibility.Collapsed;
        }

        private void SetUserPicture(string pictureUri)
        {
            this.UserPictureSource = string.IsNullOrEmpty(pictureUri) ? "/Assets/defaultUser.png" : pictureUri;
        }

        private async void DisplayMessage(string message)
        {
            var messageDialog = new MessageDialog(message);

            await messageDialog.ShowAsync();
        }
    }
}
