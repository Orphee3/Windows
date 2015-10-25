using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    /// <summary>
    /// HomePage view model
    /// </summary>
    public class HomePageViewModel : ViewModel, IHomePageViewModel, ILoadingScreenComponents
    {
        public DelegateCommand<Creation> CreationInfoCommand { get; private set; }
        public DelegateCommand<Creation> ChannelInfoCommand { get; private set; }
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
            get { return this._progressRingVisibility;  }
            set
            {
                if (this._progressRingVisibility != value)
                    SetProperty(ref this._progressRingVisibility, value);
            }
        }

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
            this.CreationInfoCommand = new DelegateCommand<Creation>((creation) => App.MyNavigationService.Navigate("CreationInfo", creation));
            this.ChannelInfoCommand = new DelegateCommand<Creation>((creation) => App.MyNavigationService.Navigate("ChannelInfo", creation.CreatorList[0]));
            this.PopularCreations = new ObservableCollection<Creation>();
            this.ProgressRingVisibility = Visibility.Visible;
            this.IsProgressRingActive = true;
            if (RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
                FillPopularCreations();
            else
                DisplayMessage("Connexion unavailable");
        }

        private async void FillPopularCreations()
        {
            if (this.PopularCreations.Count == 0)
            {
                List<Creation> popularCreations;
                try
                {
                    popularCreations = await this._getter.GetInfo<List<Creation>>(RestApiManagerBase.Instance.RestApiPath["popular"]);
                }
                catch (Exception)
                {
                    DisplayMessage("Request failed");
                    this.IsProgressRingActive = false;
                    this.ProgressRingVisibility = Visibility.Collapsed;
                    return;
                }
                foreach (var creation in popularCreations)
                {
                    creation.Name = creation.Name.Split('.')[0];
                    creation.CreatorList.Add((creation.Creator[0].ToObject<User>()));
                }
                var orderedPopularCreations = popularCreations.OrderBy(t => t.CreatorList[0].Name);
                string preivousCreationName = "";
                foreach (var creation in orderedPopularCreations)
                {
                    if (preivousCreationName == "" || preivousCreationName != creation.CreatorList[0].Name)
                        creation.ChannelStackPanelVisibility = Visibility.Visible;
                    this.PopularCreations.Add(creation);
                    preivousCreationName = creation.CreatorList[0].Name;
                }
            }
            this.IsProgressRingActive = false;
            this.ProgressRingVisibility = Visibility.Collapsed;
        }

        private async void DisplayMessage(string message)
        {
            var messageDialog = new MessageDialog(message);

            await messageDialog.ShowAsync();
        }
    }
}
