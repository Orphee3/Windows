using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    /// <summary>
    /// HomePage view model
    /// </summary>
    public class HomePageViewModel : ViewModelExtend, IHomePageViewModel
    {
        public DelegateCommand<Creation> CreationInfoCommand { get; private set; }
        public DelegateCommand<Creation> ChannelInfoCommand { get; private set; }
        public DelegateCommand Test { get; private set; }
        private bool _isPageCreated = false;

        /// <summary>List of popular creations</summary>
        public ObservableCollection<Creation> PopularCreations { get; set; }
        private readonly IGetter _getter;

        /// <summary>
        /// Constructor initializing getter through
        /// dependency injection
        /// </summary>
        /// <param name="getter">Manages the sending of the "Get" requests</param>
        public HomePageViewModel(IGetter getter)
        {
            this._isPageCreated = true;
            this._getter = getter;
            this.Test = new DelegateCommand(LoadMore);
            this.CreationInfoCommand = new DelegateCommand<Creation>((creation) =>
            {
                App.MyNavigationService.Navigate("CreationInfo", JsonConvert.SerializeObject(creation));
            });
            this.ChannelInfoCommand = new DelegateCommand<Creation>((creation) => App.MyNavigationService.Navigate("ChannelInfo", JsonConvert.SerializeObject(creation.CreatorList[0])));
            this.PopularCreations = new ObservableCollection<Creation>();
            SetProgressRingVisibility(true);
            if (!RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
            {
                DisplayMessage("Connexion unavailable");
                SetProgressRingVisibility(false);
            }
            else
                FillPopularCreations();
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (this.PopularCreations.Count == 0 && this._isPageCreated == false && RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
                FillPopularCreations();
            else
                this._isPageCreated = false;
        }

        private async void LoadMore()
        {
            List<Creation> popularCreations;
            try
            {
                popularCreations = await this._getter.GetInfo<List<Creation>>(RestApiManagerBase.Instance.RestApiPath["popular"] + "?offset=" + this.PopularCreations.Count + "&size=" + 5);
            }
            catch (Exception)
            {
                DisplayMessage("Request failed");
                SetProgressRingVisibility(false);
                return;
            }
            foreach (var creation in popularCreations)
            {
                creation.Name = creation.Name.Split('.')[0];
                creation.CreatorList.Add((creation.Creator[0].ToObject<User>()));
            }
            foreach (var creation in popularCreations)
                this.PopularCreations.Add(creation);
        }

        private async void FillPopularCreations()
        {
            List<Creation> popularCreations;
            try
            {
                popularCreations = await this._getter.GetInfo<List<Creation>>(RestApiManagerBase.Instance.RestApiPath["popular"] + "?offset=" + 0 + "&size=" + 5);
            }
            catch (Exception)
            {
                DisplayMessage("Request failed");
                SetProgressRingVisibility(false);
                return;
            }
            foreach (var creation in popularCreations)
            {
                creation.Name = creation.Name.Split('.')[0];
                creation.CreatorList.Add((creation.Creator[0].ToObject<User>()));
            }
            foreach (var creation in popularCreations)
                this.PopularCreations.Add(creation);
            SetProgressRingVisibility(false);
        }
    }
}
