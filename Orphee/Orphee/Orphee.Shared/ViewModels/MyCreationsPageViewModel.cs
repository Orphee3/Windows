using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;
using Orphee.RestApiManagement.Getters.Interfaces;
using System.Collections.Generic;
using Orphee.Models.Interfaces;

namespace Orphee.ViewModels
{
    /// <summary>
    /// MyCreationsPage view model
    /// </summary>
    public class MyCreationsPageViewModel : ViewModelExtend, IMyCreationsPageViewModel
    {
        /// <summary>List of the user's creation </summary>
        public ObservableCollection<Creation> CreationList { get; set; }
        /// <summary>Redirects to the previous page </summary>
        public DelegateCommand BackCommand { get; private set; }
        public DelegateCommand<Creation> DeleteCreationCommand { get; private set; }
        private readonly IGetter _getter;

        /// <summary>
        /// Constructor initializing getter
        /// through dependency injection
        /// </summary>
        /// <param name="getter">Manages the sending of the "Get" requests</param>
        public MyCreationsPageViewModel(IGetter getter, IOnUserLoginNewsGetter onUserLoginNewsGetter)
        {
            this._getter = getter;
            this._onUserLoginNewsGetter = onUserLoginNewsGetter;
            SetProgressRingVisibility(true);
            this.CreationList = new ObservableCollection<Creation>();
            this.DeleteCreationCommand = new DelegateCommand<Creation>(DeleteCreationCommandExec);
            this.BackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
            InitCreationList();
        }

        private async void InitCreationList()
        {
            if (App.InternetAvailabilityWatcher.IsInternetUp)
            {
                var result = await this._getter.GetInfo<List<Creation>>(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + RestApiManagerBase.Instance.UserData.User.Id + "/creation");
                if (VerifyReturnedValue(result, ""))
                    AddRequestedCreationInCreationList(result);
            }
            SetProgressRingVisibility(false);
        }

        private async void DeleteCreationCommandExec(Creation creation)
        {
            
        }
        private void AddRequestedCreationInCreationList(List<Creation> creations)
        {
            foreach (var creation in creations)
                this.CreationList.Add(creation);
        } 
    }
}
