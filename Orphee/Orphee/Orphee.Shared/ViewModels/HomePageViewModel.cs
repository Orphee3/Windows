using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    /// <summary>
    /// HomePage view model
    /// </summary>
    public class HomePageViewModel : ViewModel, IHomePageViewModel
    {
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
            this.PopularCreations = new ObservableCollection<Creation>();
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
                    return;
                }
                foreach (var creation in popularCreations)
                {
                    creation.Name = creation.Name.Split('.')[0];
                    creation.CreatorList.Add((creation.Creator[0].ToObject<User>()));
                    this.PopularCreations.Add(creation);
                }
            }
        }

        private async void DisplayMessage(string message)
        {
            var messageDialog = new MessageDialog(message);

            await messageDialog.ShowAsync();
        }
    }
}
