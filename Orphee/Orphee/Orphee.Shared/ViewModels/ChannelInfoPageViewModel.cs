using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    /// <summary>
    /// ChannelInfoPage view model
    /// </summary>
    public class ChannelInfoPageViewModel : ViewModelExtend, IChannelInfoPageViewModel
    {
        /// <summary>List of the user's creation </summary>
        public ObservableCollection<Creation> CreationList { get; private set; }

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
        /// <summary>Redirects to the previous page </summary>
        public DelegateCommand BackCommand { get; private set; }
        public UserBase Creator { get; private set; }
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
            this.Creator = JsonConvert.DeserializeObject<UserBase>(navigationParameter as string);
            if (!App.InternetAvailabilityWatcher.IsInternetUp)
                DisplayMessage("Connexion unavailable");
            this.CreationList.Clear();
            var creations = await this._getter.GetInfo<List<Creation>>(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + this.Creator.Id + "/creation");
            if (VerifyReturnedValue(creations, ""))
                AddRequestedCreationsInCreationList(creations);
            this.CreationNumber = this.CreationList.Count;
            this.LikeNumber = this.Creator.Likes?.Count ?? 0;
        }

        private void AddRequestedCreationsInCreationList(List<Creation> creations)
        {
            foreach (var creation in creations)
                this.CreationList.Add(creation);
        }
    }
}
