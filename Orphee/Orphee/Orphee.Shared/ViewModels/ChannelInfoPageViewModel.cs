using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
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
            var creations = await DataCache.GetAsync("ChannelInfoPage-" + this.Creator.Id, async () => await this._getter.GetInfo<List<Creation>>(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + this.Creator.Id + "/creation"), null, true);
            if (VerifyReturnedValue(creations, ""))
                AddRequestedCreationsInCreationList(creations);
        }

        private void AddRequestedCreationsInCreationList(List<Creation> creations)
        {
            foreach (var creation in creations)
                this.CreationList.Add(creation);
        }
    }
}
