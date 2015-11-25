using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Windows.UI.Popups;
using Windows.UI.Xaml;
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
            this._getter = getter;
            this.Test = new DelegateCommand(RequestPopularCreations);
            this.CreationInfoCommand = new DelegateCommand<Creation>((creation) => App.MyNavigationService.Navigate("CreationInfo", JsonConvert.SerializeObject(creation)));
            this.ChannelInfoCommand = new DelegateCommand<Creation>((creation) => App.MyNavigationService.Navigate("ChannelInfo", JsonConvert.SerializeObject(creation.CreatorList[0])));
            this.PopularCreations = new ObservableCollection<Creation>();
            if (!App.InternetAvailabilityWatcher.IsInternetUp)
                SetProgressRingVisibility(false);
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (App.InternetAvailabilityWatcher.IsInternetUp)
                RequestPopularCreations();
        }

        private async void RequestPopularCreations()
        {
            SetProgressRingVisibility(true);
            var popularCreations = await this._getter.GetInfo<List<Creation>>(RestApiManagerBase.Instance.RestApiPath["popular"]);
            if (VerifyReturnedValue(popularCreations, ""))
                AddRequestedPopularCreationsInCreationList(popularCreations);
        }

        private void AddRequestedPopularCreationsInCreationList(List<Creation> popularCreations)
        {
            foreach (var creation in popularCreations)
            {
                var existingCreation = this.PopularCreations.FirstOrDefault(c => c.Id == creation.Id);
                if (existingCreation == null || UpdatedCreation(existingCreation, creation))
                    this.PopularCreations.Add(creation);
            }
            if (this.PopularCreations.Count > 0)
                this.EmptyMessageVisibility = Visibility.Collapsed;
        }

        private bool UpdatedCreation(Creation oldCreation, Creation newCreation)
        {
            foreach (var item in oldCreation.GetType().GetRuntimeProperties())
            {
                if (item.Name == "NumberOfLike" && !Object.Equals(item.GetValue(oldCreation, null), newCreation.GetType().GetRuntimeProperty(item.Name).GetValue(newCreation, null)))
                {
                    this.PopularCreations.Remove(oldCreation);
                    return true;
                }
            }
            return false;
        }
    }
}
