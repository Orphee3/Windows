using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;
using Q42.WinRT.Data;

namespace Orphee.ViewModels
{
    /// <summary>
    /// ChannelInfoPage view model
    /// </summary>
    public class ChannelInfoPageViewModel : ViewModelExtend, IChannelInfoPageViewModel
    {
        /// <summary>List of the user's creation </summary>
        public ObservableCollection<Creation> CreationList { get; private set; }
        /// <summary>Redirects to the previous page </summary>
        public DelegateCommand BackCommand { get; private set; }
        private int _creationNumber;
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
        public User Creator { get; private set; }
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
            SetProgressRingVisibility(true);
            this.BackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            this.Creator = JsonConvert.DeserializeObject<User>(navigationParameter as string);
            if (!RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
                DisplayMessage("Connexion unavailable");
            this.CreationList.Clear();
            //this.LikeNumber = user.Likes?.Count ?? 0;
            List<Creation> creations = null;
            try
            {
                 creations = await DataCache.GetAsync("ChannelInfoPage-" + this.Creator.Id, async () => await this._getter.GetInfo<List<Creation>>(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + this.Creator.Id + "/creation"));
            }
            catch (Exception)
            {
                DisplayMessage("Request failed");
                SetProgressRingVisibility(false);
            }
            // this.CreationNumber = creations?.Count ?? 0;
            if (creations == null)
                return;
            foreach (var creation in creations)
            {
                creation.Name = creation.Name.Split('.')[0];
                this.CreationList.Add(creation);
            }
            SetProgressRingVisibility(false);
        }
    }
}
