using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            FillPopularCreations();
        }

        private async void FillPopularCreations()
        {
            if (this.PopularCreations.Count == 0)
            {
                var popularCreation = await this._getter.GetInfo<List<Creation>>(RestApiManagerBase.Instance.RestApiPath["popular"]);
                foreach (var creation in popularCreation)
                {
                    creation.Name = creation.Name.Split('.')[0];
                    creation.CreatorList.Add((creation.Creator[0].ToObject<User>()));
                    this.PopularCreations.Add(creation);
                }
            }
        }
    }
}
