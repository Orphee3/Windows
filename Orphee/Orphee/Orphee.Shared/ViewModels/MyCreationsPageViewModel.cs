using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.FileManagement.Interfaces;
using Orphee.RestApiManagement;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class MyCreationsPageViewModel : ViewModel, IMyCreationsPageViewModel
    {
        public ObservableCollection<Creation> CreationList { get; set; }
        public DelegateCommand BackCommand { get; private set; }
        private readonly IUserCreationGetter _userCreationGetter;

        public MyCreationsPageViewModel(IUserCreationGetter userCreationGetter)
        {
            this._userCreationGetter = userCreationGetter;
            InitCreationList();
            this.CreationList = new ObservableCollection<Creation>();
            this.BackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
        }

        private async void InitCreationList()
        {
            if (RestApiManagerBase.Instance.IsConnected &&
                RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
            {
                var result = await this._userCreationGetter.GetUserCreations(RestApiManagerBase.Instance.UserData.User.Id);
                foreach (var creation in result)
                {
                    creation.Name = creation.Name.Split('.')[0];
                    this.CreationList.Add(creation);
                }
            }
        }
    }
}
